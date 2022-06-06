using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IntroAsp.Models
{
    public partial class PubContext : DbContext
    {
        public PubContext()
        {
        }

        public PubContext(DbContextOptions<PubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Beer> Beers { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-55OJM29; Database=Pub; Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Beer>(entity =>
            {
                entity.ToTable("Beer");

                entity.Property(e => e.BeerId).HasColumnName("beerId");

                entity.Property(e => e.BrandId).HasColumnName("brandId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name")
                    .IsFixedLength();

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Beers)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Beer_Brand");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.BrandId)
                    .ValueGeneratedNever()
                    .HasColumnName("brandId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
