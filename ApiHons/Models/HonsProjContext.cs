using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApiHons.Models
{
    public partial class HonsProjContext : DbContext
    {
        public HonsProjContext()
        {
        }

        public HonsProjContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Technologies> Technologies { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersTechnologies> UsersTechnologies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Technologies>(entity =>
            {
                entity.HasKey(e => e.TechnologyId);

                entity.Property(e => e.TechnologyId)
                    .HasColumnName("TechnologyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersTechnologies>(entity =>
            {
                entity.HasKey(e => new { e.UsersId, e.TechnologiesId });

                entity.Property(e => e.UsersId).HasColumnName("UsersID");

                entity.Property(e => e.TechnologiesId).HasColumnName("TechnologiesID");
            });
        }
    }
}
