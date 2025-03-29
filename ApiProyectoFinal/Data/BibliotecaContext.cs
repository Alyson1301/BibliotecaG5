using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ApiProyectoFinal.Models; 

namespace ApiProyectoFinal.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }

        // Mapeo de las tablas
        public DbSet<UsuarioG5> FIDE_USUARIOSG5 { get; set; }
        public DbSet<CategoriaG5> FIDE_CATEGORIASG5 { get; set; }
        public DbSet<AutorG5> FIDE_AUTORESG5 { get; set; }
        public DbSet<LibroG5> FIDE_LIBROSG5 { get; set; }
        public DbSet<FavoritoG5> FIDE_FAVORITOSG5 { get; set; }
        public DbSet<DisponibilidadG5> FIDE_DISPONIBILIDADG5 { get; set; }
        public DbSet<PrestamoG5> FIDE_PRESTAMOSG5 { get; set; }
        public DbSet<FeedbackG5> FIDE_FEEDBACKG5 { get; set; }
        public DbSet<TransaccionG5> FIDE_TRANSACCIONESG5 { get; set; }
        public DbSet<ListaNegraG5> FIDE_LISTA_NEGRAG5 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones y restricciones

            // Relación entre Libros y Autores
            modelBuilder.Entity<LibroG5>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.ID_Autor)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Libros y Categorías
            modelBuilder.Entity<LibroG5>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(l => l.ID_Categoria)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación entre Favoritos y Usuarios
            modelBuilder.Entity<FavoritoG5>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Favoritos)
                .HasForeignKey(f => f.ID_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Favoritos y Libros
            modelBuilder.Entity<FavoritoG5>()
                .HasOne(f => f.Libro)
                .WithMany(l => l.Favoritos)
                .HasForeignKey(f => f.ID_Libro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Disponibilidad y Libros
            modelBuilder.Entity<DisponibilidadG5>()
                .HasOne(d => d.Libro)
                .WithMany(l => l.Disponibilidades)
                .HasForeignKey(d => d.ID_Libro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Préstamos y Usuarios
            modelBuilder.Entity<PrestamoG5>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Prestamos)
                .HasForeignKey(p => p.ID_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Préstamos y Libros
            modelBuilder.Entity<PrestamoG5>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.ID_Libro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Feedback y Usuarios
            modelBuilder.Entity<FeedbackG5>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.ID_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Feedback y Libros
            modelBuilder.Entity<FeedbackG5>()
                .HasOne(f => f.Libro)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(f => f.ID_Libro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Transacciones y Usuarios
            modelBuilder.Entity<TransaccionG5>()
                .HasOne(t => t.Usuario)
                .WithMany(u => u.Transacciones)
                .HasForeignKey(t => t.ID_Usuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Transacciones y Libros
            modelBuilder.Entity<TransaccionG5>()
                .HasOne(t => t.Libro)
                .WithMany(l => l.Transacciones)
                .HasForeignKey(t => t.ID_Libro)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación entre Lista Negra y Usuarios
            modelBuilder.Entity<ListaNegraG5>()
                .HasOne(ln => ln.Usuario)
                .WithMany(u => u.ListaNegra)
                .HasForeignKey(ln => ln.ID_Usuario)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}