using System;
using System.Collections.Generic;
using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Persistence.Database.RateIt;

public partial class DbContextRateIt : DbContext
{
    public DbContextRateIt(DbContextOptions<DbContextRateIt> options)
        : base(options)
    {
    }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<ProfilesMenu> ProfilesMenus { get; set; }

    public virtual DbSet<ProfilesUser> ProfilesUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MnuId);

            entity.ToTable("Menu");

            entity.Property(e => e.MnuId)
                .ValueGeneratedNever()
                .HasColumnName("mnuId");
            entity.Property(e => e.MnuDescription)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mnuDescription");
            entity.Property(e => e.MnuEnabled).HasColumnName("mnuEnabled");
            entity.Property(e => e.MnuImageUrl)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("mnuImageUrl");
            entity.Property(e => e.MnuInsertDate).HasColumnName("mnuInsertDate");
            entity.Property(e => e.MnuKey)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mnuKey");
            entity.Property(e => e.MnuLastUpdate).HasColumnName("mnuLastUpdate");
            entity.Property(e => e.MnuParentId).HasColumnName("mnuParentId");
            entity.Property(e => e.MnuValidFrom).HasColumnName("mnuValidFrom");
        });

        modelBuilder.Entity<ProfilesMenu>(entity =>
        {
            entity.HasKey(e => e.PrmId).HasName("PK_ProfileMenu");

            entity.ToTable("ProfilesMenu");

            entity.Property(e => e.PrmId).HasColumnName("prmId");
            entity.Property(e => e.MnuId).HasColumnName("mnuId");
            entity.Property(e => e.PrmEnabled).HasColumnName("prmEnabled");
            entity.Property(e => e.PrmInsertDate).HasColumnName("prmInsertDate");
            entity.Property(e => e.RolId).HasColumnName("rolId");

            entity.HasOne(d => d.Mnu).WithMany(p => p.ProfilesMenus)
                .HasForeignKey(d => d.MnuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfilesMenu_Menu");

            entity.HasOne(d => d.Rol).WithMany(p => p.ProfilesMenus)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfilesMenu_Roles");
        });

        modelBuilder.Entity<ProfilesUser>(entity =>
        {
            entity.HasKey(e => e.PruId);

            entity.Property(e => e.PruId).HasColumnName("pruId");
            entity.Property(e => e.PruEnabled).HasColumnName("pruEnabled");
            entity.Property(e => e.PruInsertDate).HasColumnName("pruInsertDate");
            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.UsrId).HasColumnName("usrId");

            entity.HasOne(d => d.Rol).WithMany(p => p.ProfilesUsers)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Roles");

            entity.HasOne(d => d.Usr).WithMany(p => p.ProfilesUsers)
                .HasForeignKey(d => d.UsrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId);

            entity.Property(e => e.RolId).HasColumnName("rolId");
            entity.Property(e => e.RolCategory)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("rolCategory");
            entity.Property(e => e.RolDescription)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rolDescription");
            entity.Property(e => e.RolEnabled).HasColumnName("rolEnabled");
            entity.Property(e => e.RolInsertDate).HasColumnName("rolInsertDate");
            entity.Property(e => e.RolLastUpdate).HasColumnName("rolLastUpdate");
            entity.Property(e => e.RolValidFrom).HasColumnName("rolValidFrom");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrId);

            entity.Property(e => e.UsrId).HasColumnName("usrId");
            entity.Property(e => e.UsrDescription)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usrDescription");
            entity.Property(e => e.UsrEmail)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("usrEMail");
            entity.Property(e => e.UsrEnabled).HasColumnName("usrEnabled");
            entity.Property(e => e.UsrInsertDate).HasColumnName("usrInsertDate");
            entity.Property(e => e.UsrLastUpdate).HasColumnName("usrLastUpdate");
            entity.Property(e => e.UsrPassword)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("usrPassword");
            entity.Property(e => e.UsrValidFrom).HasColumnName("usrValidFrom");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
