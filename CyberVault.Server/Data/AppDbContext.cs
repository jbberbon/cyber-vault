using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CyberVault.Server.Models;

namespace CyberVault.Server.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Restrict delete behavior for Permission to prevent cascade delete
            // 01. FolderPermission
            modelBuilder.Entity<FolderPermission>()
                .HasOne(folderPermission => folderPermission.Folder)
                .WithMany(folder => folder.FolderPermissions)
                .HasForeignKey(folderPermission => folderPermission.FolderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<FolderPermission>()
                .HasOne(folderPermission => folderPermission.User)
                .WithMany(user => user.FolderPermissions)
                .HasForeignKey(folderPermission => folderPermission.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FolderPermission>()
                .HasOne(folderPermission => folderPermission.Permission)
                .WithMany(permission => permission.FolderPermissions)
                .HasForeignKey(folderPermission => folderPermission.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // 02. FilePermission
            modelBuilder.Entity<BlobFilePermission>()
                .HasOne(folderPermission => folderPermission.BlobFile)
                .WithMany(folder => folder.BlobFilePermissions)
                .HasForeignKey(folderPermission => folderPermission.FileId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<BlobFilePermission>()
                .HasOne(folderPermission => folderPermission.User)
                .WithMany(user => user.BlobFilePermissions)
                .HasForeignKey(folderPermission => folderPermission.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlobFilePermission>()
                .HasOne(folderPermission => folderPermission.Permission)
                .WithMany(permission => permission.BlobFilePermissions)
                .HasForeignKey(folderPermission => folderPermission.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // 03. Folder
            modelBuilder.Entity<Folder>()
                .HasOne(folder => folder.Owner)
                .WithMany(user => user.Folders)
                .HasForeignKey(folder => folder.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 04. File
            modelBuilder.Entity<BlobFile>()
                .HasOne(blobFile => blobFile.Owner)
                .WithMany(user => user.BlobFiles)
                .HasForeignKey(blobFile => blobFile.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<BlobFile> Files { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<BlobFilePermission> BlobFilePermissions { get; set; }
        public DbSet<FolderPermission> FolderPermissions { get; set; }
    }
}