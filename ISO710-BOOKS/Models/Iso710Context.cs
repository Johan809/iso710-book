using Microsoft.EntityFrameworkCore;

namespace ISO710_BOOKS.Models;

public partial class Iso710Context : DbContext
{
    public Iso710Context() { }

    public Iso710Context(DbContextOptions<Iso710Context> options)
        : base(options) { }

    public virtual DbSet<Miembro> Miembros { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Miembro>(entity =>
        {
            entity.HasKey(e => e.MiembroId).HasName("PK__Miembro__CC213E597D75A02B");

            entity.ToTable("Miembro");

            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.PrestamoId).HasName("PK__Prestamo__AA58A0A08133B651");

            entity.ToTable("Prestamo");

            entity.Property(e => e.Devuelto).HasDefaultValue(false);
            entity.Property(e => e.FechaDevolucion).HasColumnType("datetime");
            entity.Property(e => e.FechaPrestamo)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN");
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Miembro).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.MiembroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prestamo_Miembro");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
