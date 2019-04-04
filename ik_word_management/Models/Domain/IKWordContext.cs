using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ik_word_management.Models.Domain
{
    public partial class IKWordContext : DbContext
    {
        public IKWordContext()
        {
        }

        public IKWordContext(DbContextOptions<IKWordContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Refresh> Refresh { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;database=IKWord;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Groups>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cdt)
                    .HasColumnName("CDT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enable).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Udt)
                    .HasColumnName("UDT")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Refresh>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ExpiresIn).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserAccount>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cdt)
                    .HasColumnName("CDT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enable).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<Words>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cdt)
                    .HasColumnName("CDT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Enable).HasDefaultValueSql("((1))");

                entity.Property(e => e.GroupId)
                    .IsRequired()
                    .HasColumnName("GroupID")
                    .HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Udt)
                    .HasColumnName("UDT")
                    .HasColumnType("datetime");
            });
        }
    }
}
