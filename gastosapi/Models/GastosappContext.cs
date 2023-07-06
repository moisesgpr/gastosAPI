using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gastosapi.Models;

public partial class GastosappContext : DbContext
{
    public GastosappContext()
    {
    }

    public GastosappContext(DbContextOptions<GastosappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Operation> Operation { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:gastosapp.database.windows.net,1433;Initial Catalog=gastosapp;Persist Security Info=False;User ID=gastosapp;Password=yucafrita2023*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
       
       // => optionsBuilder.UseSqlServer("Server=PR\\SQLExpress;Database=gastosapp;Trusted_Connection=True;TrustServerCertificate=true");
        //=> optionsBuilder.UseSqlServer("Server=DESKTOP-SDKDVL8;Database=gastosapp;Trusted_Connection=True;TrustServerCertificate=true");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.IdOperation);

            entity.Property(e => e.IdOperation).HasColumnName("idOperation");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Created)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("created");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Operation)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operation_Category");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Operation)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operation_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
