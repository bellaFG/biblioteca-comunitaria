using BibliotecaComunitaria.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BibliotecaComunitaria.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options) { }

        // -----------------------------
        // Tabelas do banco
        // -----------------------------
        public DbSet<User> User { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Borrow> Borrow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -----------------------------
            // Relacionamento User → Books (1:N)
            // -----------------------------
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // Relacionamento User → Borrow (1:N)
            // -----------------------------
            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.User)
                .WithMany(u => u.Borrow)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // Relacionamento Book → Borrow (1:N)
            // -----------------------------
            modelBuilder.Entity<Borrow>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.Borrow)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // Configurações padrão
            // -----------------------------
            modelBuilder.Entity<User>()
                .Property(u => u.Ative)
                .HasDefaultValue(true);

            modelBuilder.Entity<Book>()
                .Property(b => b.Status)
                .HasConversion<int>() // Enum → int
                .HasDefaultValue(BookStatus.Available);
        }
    }
}
