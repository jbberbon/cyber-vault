using Azure.Storage.Blobs;
using CyberVault.Server.Data;
using CyberVault.Server.Miscs.Constants;
using CyberVault.Server.Miscs.Utilities.AuthHelpers;
using CyberVault.Server.Models;
using CyberVault.Server.Services.AzureBlobService;
using CyberVault.Server.Services.AuthService;
using CyberVault.Server.Services.Configs.Azure;
using CyberVault.Server.Services.DirectoryService;
using CyberVault.Server.Services.FilesService;
using CyberVault.Server.Services.ModelService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ---> 01. Setup DbContext and Identity
// 01.01 Set up connection string
var connectionString = builder.Configuration.GetConnectionString(UserSecretKeys.DefaultConnection) ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// 01.02 Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 01.03 Setup Identity
builder.Services.AddIdentity<User, Role>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ---> 02. Add packages to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/cybervault-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 500 * 1024 * 1024;
});
// Increase the maximum file size allowed for multipart/form-data (the default max limit is 128MB)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500MB
});

// ---> 03. Configure Cookies and 401 when no cookies in browser
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.SlidingExpiration = true;

    // Add this to return 401 instead of redirecting to login page
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            // This prevents the redirect and returns 401 instead
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    };
});


// ---> 04. Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAzureBlobService, AzureBlobService>();
builder.Services.AddScoped<IFilesService, FilesService>();
builder.Services.AddScoped<IDirectoryService, DirectoryService>();

builder.Services.AddSingleton<IAuthHelpers, AuthHelpers>();

// --> 04.01 Model Services
builder.Services.AddScoped<IFolderService, FolderService>();

// Lazy DI
builder.Services.AddScoped<Lazy<IFilesService>>(provider =>
    new Lazy<IFilesService>(() => provider.GetRequiredService<IFilesService>()));

// Azure Connections
new AzureStorageAccountConfig(builder);

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();