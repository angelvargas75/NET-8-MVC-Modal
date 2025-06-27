using Microsoft.EntityFrameworkCore;
using WebAppModal.Models;

namespace WebAppModal.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamento { get; set; }

    public virtual DbSet<Distrito> Distrito { get; set; }

    public virtual DbSet<Provincia> Provincia { get; set; }

    public virtual DbSet<Trabajadores> Trabajadores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ANGEL-PC\\SQLEXPRESS;Database=TrabajadoresPrueba;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departam__3214EC074728C8F0");
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Distrito__3214EC07B12FFD59");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Distrito).HasConstraintName("FK__Distrito__IdProv__3D5E1FD2");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Provinci__3214EC071D7CF3A6");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Provincia).HasConstraintName("FK__Provincia__IdDep__3E52440B");
        });

        modelBuilder.Entity<Trabajadores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Trabajad__3214EC077C3387DD");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Trabajadores).HasConstraintName("FK__Trabajado__IdDep__3F466844");

            entity.HasOne(d => d.IdDistritoNavigation).WithMany(p => p.Trabajadores).HasConstraintName("FK__Trabajado__IdDis__403A8C7D");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Trabajadores).HasConstraintName("FK__Trabajado__IdPro__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
