using Admin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Admin.Persistence.Database;

public partial class DbContextRateIt : DbContext
{
    public DbContextRateIt(DbContextOptions<DbContextRateIt> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProfilesMenu> ProfilesMenus { get; set; }

    public virtual DbSet<ProfilesUser> ProfilesUsers { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.CatEnabled).HasColumnName("catEnabled");
            entity.Property(e => e.CatInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("catInsertDate");
            entity.Property(e => e.CatLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("catLastUpdate");
            entity.Property(e => e.CatName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("catName");
            entity.Property(e => e.CatValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("catValidFrom");
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

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MnuId);

            entity.ToTable("Menu");

            entity.HasIndex(e => e.MnuHierarchyKey, "IX_Menu").IsUnique();

            entity.Property(e => e.MnuId)
                .ValueGeneratedNever()
                .HasColumnName("mnuId");
            entity.Property(e => e.MnuCategory)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("mnuCategory");
            entity.Property(e => e.MnuDescription)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mnuDescription");
            entity.Property(e => e.MnuEnabled).HasColumnName("mnuEnabled");
            entity.Property(e => e.MnuHierarchyKey)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("mnuHierarchyKey");
            entity.Property(e => e.MnuImageUrl)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("mnuImageUrl");
            entity.Property(e => e.MnuInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("mnuInsertDate");
            entity.Property(e => e.MnuKey)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mnuKey");
            entity.Property(e => e.MnuLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("mnuLastUpdate");
            entity.Property(e => e.MnuValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("mnuValidFrom");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PrdId);

            entity.Property(e => e.PrdId).HasColumnName("prdId");
            entity.Property(e => e.CatId).HasColumnName("catId");
            entity.Property(e => e.PrdDescription)
                .IsRequired()
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("prdDescription");
            entity.Property(e => e.PrdEnabled).HasColumnName("prdEnabled");
            entity.Property(e => e.PrdImage)
                .HasMaxLength(256)
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
            entity.Property(e => e.PrdThumbnail)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("prdThumbnail");
            entity.Property(e => e.PrdValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("prdValidFrom");

            entity.HasOne(d => d.Cat).WithMany(p => p.Products)
                .HasForeignKey(d => d.CatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProfilesMenu>(entity =>
        {
            entity.HasKey(e => e.PrmId).HasName("PK_ProfileMenu");

            entity.ToTable("ProfilesMenu");

            entity.HasIndex(e => new { e.RolId, e.MnuId }, "IX_ProfilesMenu").IsUnique();

            entity.Property(e => e.PrmId).HasColumnName("prmId");
            entity.Property(e => e.MnuId).HasColumnName("mnuId");
            entity.Property(e => e.PrmEnabled).HasColumnName("prmEnabled");
            entity.Property(e => e.PrmInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("prmInsertDate");
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
            entity.Property(e => e.PruInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("pruInsertDate");
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

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatId);

            entity.ToTable("Rating");

            entity.Property(e => e.RatId).HasColumnName("ratId");
            entity.Property(e => e.PrdId).HasColumnName("prdId");
            entity.Property(e => e.RatDescription)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("ratDescription");
            entity.Property(e => e.RatInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("ratInsertDate");
            entity.Property(e => e.RatLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("ratLastUpdate");
            entity.Property(e => e.RatPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("ratPrice");
            entity.Property(e => e.RatRating).HasColumnName("ratRating");
            entity.Property(e => e.StoId).HasColumnName("stoId");
            entity.Property(e => e.UsrId).HasColumnName("usrId");

            entity.HasOne(d => d.Prd).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.PrdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Products");

            entity.HasOne(d => d.Sto).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.StoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Stores");

            entity.HasOne(d => d.Usr).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UsrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rating_Users");
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
            entity.Property(e => e.RolInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("rolInsertDate");
            entity.Property(e => e.RolLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("rolLastUpdate");
            entity.Property(e => e.RolValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("rolValidFrom");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoId);

            entity.Property(e => e.StoId).HasColumnName("stoId");
            entity.Property(e => e.StoAddress)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("stoAddress");
            entity.Property(e => e.StoDescription)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("stoDescription");
            entity.Property(e => e.StoMobile)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("stoMobile");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrId);

            entity.Property(e => e.UsrId).HasColumnName("usrId");
            entity.Property(e => e.UsrDescription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usrDescription");
            entity.Property(e => e.UsrEmail)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("usrEMail");
            entity.Property(e => e.UsrEnabled).HasColumnName("usrEnabled");
            entity.Property(e => e.UsrInsertDate)
                .HasColumnType("datetime")
                .HasColumnName("usrInsertDate");
            entity.Property(e => e.UsrLastUpdate)
                .HasColumnType("datetime")
                .HasColumnName("usrLastUpdate");
            entity.Property(e => e.UsrPassword)
                .IsRequired()
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("usrPassword");
            entity.Property(e => e.UsrValidFrom)
                .HasColumnType("datetime")
                .HasColumnName("usrValidFrom");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
