using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Test.Model;

public partial class DemoKrosContext : DbContext
{
    public DemoKrosContext()
    {
    }

    public DemoKrosContext(DbContextOptions<DemoKrosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeliviryPoint> DeliviryPoints { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OredersItem> OredersItems { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<TypeBoot> TypeBoots { get; set; }

    public virtual DbSet<TypeProduct> TypeProducts { get; set; }

    public virtual DbSet<TypeRole> TypeRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=demo_kros", ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DeliviryPoint>(entity =>
        {
            entity.HasKey(e => e.DeliviryPointId).HasName("PRIMARY");

            entity.ToTable("deliviry_point");

            entity.Property(e => e.DeliviryPointId).HasColumnName("deliviry_point_id");
            entity.Property(e => e.City)
                .HasMaxLength(14)
                .HasColumnName("city");
            entity.Property(e => e.Code)
                .HasMaxLength(14)
                .HasColumnName("code");
            entity.Property(e => e.Number)
                .HasMaxLength(24)
                .HasColumnName("number");
            entity.Property(e => e.Street)
                .HasMaxLength(24)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PRIMARY");

            entity.ToTable("manufacturer");

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrdersId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.DeliviryPointId, "fk_del_idx");

            entity.HasIndex(e => e.PersonId, "fk_per_idx");

            entity.Property(e => e.OrdersId).HasColumnName("orders_id");
            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .HasColumnName("code");
            entity.Property(e => e.DateDeliviry).HasColumnName("date_deliviry");
            entity.Property(e => e.DateOrder).HasColumnName("date_order");
            entity.Property(e => e.DeliviryPointId).HasColumnName("deliviry_point_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.Status)
                .HasMaxLength(25)
                .HasColumnName("status");

            entity.HasOne(d => d.DeliviryPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliviryPointId)
                .HasConstraintName("fk_orders_deliviry_point");

            entity.HasOne(d => d.Person).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("fk_orders_person");
        });

        modelBuilder.Entity<OredersItem>(entity =>
        {
            entity.HasKey(e => e.OredersItemId).HasName("PRIMARY");

            entity.ToTable("oreders_item");

            entity.HasIndex(e => e.OrdersId, "fk_order_iet_idx");

            entity.HasIndex(e => e.ProductId, "fk_prod_iet_idx");

            entity.Property(e => e.OredersItemId).HasColumnName("oreders_item_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.OrdersId).HasColumnName("orders_id");
            entity.Property(e => e.ProductId)
                .HasMaxLength(6)
                .HasColumnName("product_id");

            entity.HasOne(d => d.Orders).WithMany(p => p.OredersItems)
                .HasForeignKey(d => d.OrdersId)
                .HasConstraintName("fk_order_iet");

            entity.HasOne(d => d.Product).WithMany(p => p.OredersItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_prod_iet");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PRIMARY");

            entity.ToTable("person");

            entity.HasIndex(e => e.TypeRoleId, "fk_role_idx");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Pass)
                .HasMaxLength(45)
                .HasColumnName("pass");
            entity.Property(e => e.Patronamic)
                .HasMaxLength(45)
                .HasColumnName("patronamic");
            entity.Property(e => e.TypeRoleId).HasColumnName("type_role_id");

            entity.HasOne(d => d.TypeRole).WithMany(p => p.People)
                .HasForeignKey(d => d.TypeRoleId)
                .HasConstraintName("fk_role");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.TypeBootsId, "fk_boot_idx");

            entity.HasIndex(e => e.ManufacturerId, "fk_mun_idx");

            entity.HasIndex(e => e.TypeProductId, "fk_product_idx");

            entity.HasIndex(e => e.SupplierId, "fk_sup_idx");

            entity.Property(e => e.ProductId)
                .HasMaxLength(6)
                .HasColumnName("product_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Photo)
                .HasColumnType("mediumblob")
                .HasColumnName("photo");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.Title)
                .HasColumnType("text")
                .HasColumnName("title");
            entity.Property(e => e.TypeBootsId).HasColumnName("type_boots_id");
            entity.Property(e => e.TypeProductId).HasColumnName("type_product_id");
            entity.Property(e => e.Unit)
                .HasMaxLength(7)
                .HasColumnName("unit");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("fk_mun");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("fk_sup");

            entity.HasOne(d => d.TypeBoots).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeBootsId)
                .HasConstraintName("fk_boot");

            entity.HasOne(d => d.TypeProduct).WithMany(p => p.Products)
                .HasForeignKey(d => d.TypeProductId)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SuppliersId).HasName("PRIMARY");

            entity.ToTable("suppliers");

            entity.Property(e => e.SuppliersId).HasColumnName("suppliers_id");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TypeBoot>(entity =>
        {
            entity.HasKey(e => e.TypeBootsId).HasName("PRIMARY");

            entity.ToTable("type_boots");

            entity.Property(e => e.TypeBootsId).HasColumnName("type_boots_id");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TypeProduct>(entity =>
        {
            entity.HasKey(e => e.TypeProductId).HasName("PRIMARY");

            entity.ToTable("type_product");

            entity.Property(e => e.TypeProductId)
                .ValueGeneratedNever()
                .HasColumnName("type_product_id");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        modelBuilder.Entity<TypeRole>(entity =>
        {
            entity.HasKey(e => e.TypeRoleId).HasName("PRIMARY");

            entity.ToTable("type_role");

            entity.Property(e => e.TypeRoleId).HasColumnName("type_role_id");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
