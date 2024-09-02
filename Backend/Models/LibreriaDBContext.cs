using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public partial class LibreriaDbContext : DbContext
{

    private readonly IConfiguration _configuration;
    public LibreriaDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LibreriaDbContext(DbContextOptions<LibreriaDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Maestro> Maestros { get; set; }

    public virtual DbSet<Detalle> Detalles { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Formato> Formatos { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Tarjetum> Tarjeta { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("LibreriaDB");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Autor__3214EC2754B1440E");

            entity.ToTable("Autor");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.ToTable("Maestro");

            entity.HasKey(e => e.MaestroId);

            entity.Property(e => e.MaestroId).HasColumnName("MaestroId").ValueGeneratedOnAdd();
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioId");
            entity.Property(e => e.TotalFactura).HasColumnName("TotalFactura").HasColumnType("DECIMAL(10, 2)");
            entity.Property(e => e.FechaCompra).HasColumnName("FechaCompra").HasColumnType("DATE");

            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Detalle>(entity =>
        {
            entity.ToTable("Detalle");

            entity.HasKey(e => e.DetalleId);

            entity.Property(e => e.DetalleId).HasColumnName("DetalleId").ValueGeneratedOnAdd();
            entity.Property(e => e.MaestroId).HasColumnName("MaestroId");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoId");
            entity.Property(e => e.CantidadCompra).HasColumnName("CantidadCompra");
            entity.Property(e => e.PrecioCompra).HasColumnName("PrecioCompra").HasColumnType("DECIMAL(10, 2)");
            entity.Property(e => e.Impuesto).HasColumnName("Impuesto").HasColumnType("DECIMAL(10, 2)");

        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.ToTable("Carrito");

            entity.HasKey(e => e.CarritoId);

            entity.Property(e => e.CarritoId).HasColumnName("CarritoId").ValueGeneratedOnAdd();
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioId");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoId");
            entity.Property(e => e.Cantidad).HasColumnName("Cantidad");
            entity.Property(e => e.FechaCarrito).HasColumnName("FechaCarrito").HasColumnType("DATE");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Compra__3214EC271196A7B9");

            entity.ToTable("Compra");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FechaCompra).HasColumnType("datetime");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Compras)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Compra_Usuario");
        });

        modelBuilder.Entity<Formato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Formato__3214EC270B657CBC");

            entity.ToTable("Formato");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genero__3214EC27A504320E");

            entity.ToTable("Genero");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Libro__3214EC27CA88560F");

            entity.ToTable("Libro");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AutorId).HasColumnName("AutorID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.FormatoId).HasColumnName("FormatoID");
            entity.Property(e => e.GeneroId).HasColumnName("GeneroID");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RutaFoto).HasMaxLength(255);

            entity.HasOne(d => d.Autor).WithMany(p => p.Libros)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("FK_Libro_Autor");

            entity.HasOne(d => d.Formato).WithMany(p => p.Libros)
                .HasForeignKey(d => d.FormatoId)
                .HasConstraintName("FK_Libro_Formato");

            entity.HasOne(d => d.Genero).WithMany(p => p.Libros)
                .HasForeignKey(d => d.GeneroId)
                .HasConstraintName("FK_Libro_Genero");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC27E94B9CA0");

            entity.ToTable("Rol");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Tarjetum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tarjeta__3214EC279D7AD610");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cvv)
                .HasMaxLength(4)
                .HasColumnName("CVV");
            entity.Property(e => e.NombreTitular).HasMaxLength(50);
            entity.Property(e => e.Numero).HasMaxLength(20);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Tarjeta)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Tarjeta_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC27BC564FD1");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apellido).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.Telefono).HasMaxLength(15);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
