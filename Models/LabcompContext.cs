using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SSP2.Models;

public partial class LabcompContext : DbContext
{
    public LabcompContext()
    {
    }

    public LabcompContext(DbContextOptions<LabcompContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Folio> Folios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-JESUS\\SQLEXPRESS; Initial Catalog=LABCOMP; Integrated Security=True; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Folio>(entity =>
        {
            entity.HasKey(e => e.FolId).HasName("PK__Folios__00F59AEA34EAF1CE");

            entity.Property(e => e.FolId).HasColumnName("fol_id");
            entity.Property(e => e.FolFecha)
                .HasColumnType("date")
                .HasColumnName("fol_fecha");
            entity.Property(e => e.FolNu).HasColumnName("fol_nu");
            entity.Property(e => e.FolNumero)
                .HasMaxLength(100)
                .HasColumnName("fol_numero");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__63C76BE23A09615F");

            entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
