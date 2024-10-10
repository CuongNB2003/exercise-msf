﻿using Microsoft.EntityFrameworkCore;
using MsfServer.Domain.Entities;

namespace MsfServer.EntityFrameworkCore.Database
{
    public class MsfServerDbContext(DbContextOptions<MsfServerDbContext> options) : DbContext(options)
    {

        // Định nghĩa các DbSet cho các bảng trong cơ sở dữ liệu
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình thêm nếu cần
            // tạo các giá trị mặc định cho createdAt updateAt deleteAt cho từng bảng phía dưới
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.DeletedAt)
                    .HasDefaultValueSql("NULL");
            });
        }
    }
}
