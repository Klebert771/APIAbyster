using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APIAbyster.Models;

public partial class BudgetContext : DbContext
{
    public BudgetContext()
    {
    }

    public BudgetContext(DbContextOptions<BudgetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\;Initial Catalog=Budget; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.IdCategorie)
                .HasName("PK_CATEGORIE")
                .IsClustered(false);

            entity.ToTable("Categorie");

            entity.Property(e => e.IdCategorie)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.CreateCategorie).HasColumnType("datetime");
            entity.Property(e => e.LibelleCategorie)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.IdOperation)
                .HasName("PK_OPERATION")
                .IsClustered(false);

            entity.ToTable("Operation");

            entity.HasIndex(e => e.IdUser, "EFFECTUER_FK");

            entity.HasIndex(e => e.IdCategorie, "UTILISER_FK");

            entity.Property(e => e.IdOperation)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.DateOperation).HasColumnType("datetime");
            entity.Property(e => e.IdCategorie).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.IdUser).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.LibelleOperation)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MontantOperation)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdCategorieNavigation).WithMany(p => p.Operations)
                .HasForeignKey(d => d.IdCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OPERATIO_UTILISER_CATEGORI");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Operations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OPERATIO_EFFECTUER_USER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser)
                .HasName("PK_USER")
                .IsClustered(false);

            entity.ToTable("User");

            entity.Property(e => e.IdUser)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.CreatedUser).HasColumnType("datetime");
            entity.Property(e => e.EmailUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MdpUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NomUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PrenomUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.RoleUser)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
