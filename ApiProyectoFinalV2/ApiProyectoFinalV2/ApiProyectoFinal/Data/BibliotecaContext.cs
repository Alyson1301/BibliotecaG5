using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ApiProyectoFinal.Models;

namespace ApiProyectoFinal.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        // Mapeo de las tablas
        public DbSet<UsuarioG5> FIDE_USUARIOSG5 { get; set; }
        public DbSet<CategoriaG5> FIDE_CATEGORIASG5 { get; set; }
        public DbSet<AutorG5> FIDE_AUTORESG5 { get; set; }
        public DbSet<LibroG5> FIDE_LIBROSG5 { get; set; }
        
        public DbSet<DisponibilidadG5> FIDE_DISPONIBILIDADG5 { get; set; }
        public DbSet<PrestamoG5> FIDE_PRESTAMOSG5 { get; set; }
        
        public DbSet<ListaNegraG5> FIDE_LISTA_NEGRAG5 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Libros
            modelBuilder.Entity<LibroG5>(entity =>
            {
                entity.ToTable("FIDE_LIBROSG5");
                entity.HasKey(e => e.ID_Libro);

                entity.HasOne(l => l.Autor)
                      .WithMany(a => a.Libros)
                      .HasForeignKey(l => l.ID_Autor)
                      .OnDelete(DeleteBehavior.Cascade);

                
            });

            // Autores
            modelBuilder.Entity<AutorG5>(entity =>
            {
                entity.ToTable("FIDE_AUTORESG5");
                entity.HasKey(a => a.ID_Autor);
            });

            // Categorías
            modelBuilder.Entity<CategoriaG5>(entity =>
            {
                entity.ToTable("FIDE_CATEGORIASG5");
                entity.HasKey(c => c.ID_Categoria);
            });

            // Usuarios
            modelBuilder.Entity<UsuarioG5>(entity =>
            {
                entity.ToTable("FIDE_USUARIOSG5");
                entity.HasKey(u => u.ID_Usuario);
            });

            

            // Disponibilidad
            modelBuilder.Entity<DisponibilidadG5>(entity =>
            {
                entity.ToTable("FIDE_DISPONIBILIDADG5");
                entity.HasKey(d => d.ID_Disponibilidad);

                
            });

            // Préstamos
            modelBuilder.Entity<PrestamoG5>(entity =>
            {
                entity.ToTable("FIDE_PRESTAMOSG5");
                entity.HasKey(p => p.ID_Prestamo);

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Prestamos)
                      .HasForeignKey(p => p.ID_Usuario)
                      .OnDelete(DeleteBehavior.Cascade);

                
            });

            

           

            // Lista Negra
            modelBuilder.Entity<ListaNegraG5>(entity =>
            {
                entity.ToTable("FIDE_LISTA_NEGRAG5");
                entity.HasKey(ln => ln.ID_Lista_Negra);

                entity.HasOne(ln => ln.Usuario)
                      .WithMany(u => u.ListaNegra)
                      .HasForeignKey(ln => ln.ID_Usuario)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
