using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_Entity.Juction_Tables;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_Data
{
    public class ETIDriveContext : IdentityDbContext<User>
    {
        public ETIDriveContext(DbContextOptions<ETIDriveContext> options) : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<UserFolder> UserFolders { get; set; }
        public DbSet<Data> Datas { get; set; }
        public DbSet<DataPermission> DataPermissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<DataTag> DataTags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Identity.User tables
            modelBuilder.Entity<User>().Ignore(u => u.PasswordHash);
            modelBuilder.Entity<User>().Ignore(u => u.SecurityStamp);
            modelBuilder.Entity<User>().Ignore(u => u.ConcurrencyStamp);
            modelBuilder.Entity<User>().Ignore(u => u.TwoFactorEnabled);
            modelBuilder.Entity<User>().Ignore(u => u.AccessFailedCount);
            modelBuilder.Entity<User>().Ignore(u => u.LockoutEnabled);
            modelBuilder.Entity<User>().Ignore(u => u.LockoutEnd);

            // Configure the junction tables
            modelBuilder.Entity<UserFolder>()
                .HasKey(uf => new { uf.Id, uf.FolderId });
            modelBuilder.Entity<DataPermission>()
                .HasKey(fp => new { fp.Id, fp.DataId });
            modelBuilder.Entity<DataTag>()
                .HasKey(ft => new { ft.DataId, ft.TagId });
            modelBuilder.Entity<Data>()
                .HasOne(f => f.CreatedBy)
                .WithMany(u => u.CreatedFiles)
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Data>()
                .HasOne(f => f.ModifiedBy)
                .WithMany(u => u.LastModifiedFiles)
                .HasForeignKey(f => f.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Folder)
                .WithOne(f => f.Department)
                .HasForeignKey<Department>(d => d.FolderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
