using CyberVault.Server.Miscs.Constants;
using Microsoft.Extensions.Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using static CyberVault.Server.Miscs.Utilities.AzureHelpers;

namespace CyberVault.Server.Services.Configs.Azure;

public class AzureStorageAccountConfig
{
    private readonly WebApplicationBuilder _builder;

    public AzureStorageAccountConfig(WebApplicationBuilder builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));

        // 01. Fetch the Environment variables from the configuration
        var blobAccountName = _builder.Configuration.GetValue<string>(UserSecretKeys.BlobAccountName);
        var blobAccountKey = _builder.Configuration.GetValue<string>(UserSecretKeys.BlobAccountKey);
        var blobEndpointProtocol = _builder.Configuration.GetValue<string>(UserSecretKeys.BlobEndpointProtocol);
        var blobEndpoint = _builder.Configuration.GetValue<string>(UserSecretKeys.BlobEndpoint);
    
        // 02. Validate the configuration values
        if (string.IsNullOrEmpty(blobAccountName) || 
            string.IsNullOrEmpty(blobAccountKey) || 
            string.IsNullOrEmpty(blobEndpointProtocol) || 
            string.IsNullOrEmpty(blobEndpoint))
        {
            throw new InvalidOperationException("One or more Azure Blob Storage configuration values are missing.");
        }

        // 03. Build the Blob Connection String
        var blobConnectionString = ConstructBlobConnectionString(blobAccountName, blobAccountKey, blobEndpointProtocol, blobEndpoint);
        
        // 04. Register Azure Storage Account clients
        _builder.Services.AddAzureClients(clientBuilder =>
        {
            // 04.01 Register Blob Service Client
            clientBuilder.AddBlobServiceClient(blobConnectionString);
            
            // 04.02 Register Data Lake Service Client using the Acc Name, Key and Endpoint
            var sharedKeyCredential = new StorageSharedKeyCredential(blobAccountName, blobAccountKey);
            clientBuilder.AddDataLakeServiceClient(new Uri(blobEndpoint), sharedKeyCredential);
        });
        
        // 05. Add BlobContainerClient as a service by resolving it from BlobServiceClient
        _builder.Services.AddScoped<BlobContainerClient>(serviceProvider =>
        {
            var blobServiceClient = serviceProvider.GetRequiredService<BlobServiceClient>();
            return blobServiceClient.GetBlobContainerClient("cybervault"); // Specify the container name here
        });
    }
}