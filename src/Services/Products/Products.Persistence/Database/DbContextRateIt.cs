using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Persistence.Database.RateIt;

public partial class DbContextRateIt : DbContext
{
    public DbContextRateIt(DbContextOptions<DbContextRateIt> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.CatEnabled).HasColumnName("catEnabled");
            entity.Property(e => e.CatInsertDate).HasColumnName("catInsertDate");
            entity.Property(e => e.CatLastUpdate).HasColumnName("catLastUpdate");
            entity.Property(e => e.CatName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("catName");
            entity.Property(e => e.CatValidFrom).HasColumnName("catValidFrom");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.LogId).HasColumnName("logId");
            entity.Property(e => e.LogCreated)
                .HasColumnType("datetime")
                .HasColumnName("logCreated");
            entity.Property(e => e.LogValues)
                .IsRequired()
                .IsUnicode(false)
                .HasColumnName("logValues");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PrdId);

            entity.Property(e => e.PrdId).HasColumnName("prdId");
            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.PrdDepth).HasColumnName("prdDepth");
            entity.Property(e => e.PrdDescription)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("prdDescription");
            entity.Property(e => e.PrdEnabled).HasColumnName("prdEnabled");
            entity.Property(e => e.PrdHeight).HasColumnName("prdHeight");
            entity.Property(e => e.PrdImage)
                .IsUnicode(false)
                .HasColumnName("prdImage");
            entity.Property(e => e.PrdInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("prdInsertDate");
            entity.Property(e => e.PrdLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("prdLastUpdate");
            entity.Property(e => e.PrdName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("prdName");
            entity.Property(e => e.PrdPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("prdPrice");
            entity.Property(e => e.PrdThumbnail)
                .IsUnicode(false)
                .HasColumnName("prdThumbnail");
            entity.Property(e => e.PrdUseType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("prdUseType");
            entity.Property(e => e.PrdValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("prdValidFrom");
            entity.Property(e => e.PrdWidth).HasColumnName("prdWidth");

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
