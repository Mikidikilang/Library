using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Library
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;

        // Konstruktor dependency injection-höz
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // Paraméter nélküli konstruktor a meglévő kód kompatibilitásához
        public LibraryContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Konfiguráció betöltése
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                // Connection string lekérése
                string connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=localhost\\SQLEXPRESS;Database=LibraryDB;Trusted_Connection=true;TrustServerCertificate=true;";

                optionsBuilder.UseSqlServer(connectionString);

                // Development módban részletes logolás
#if DEBUG
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.LogTo(Console.WriteLine);
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Book entitás konfigurálása
            modelBuilder.Entity<Book>(entity =>
            {
                // Primary key
                entity.HasKey(b => b.Isbn);

                // Unique index az ISBN-re
                entity.HasIndex(b => b.Isbn)
                    .IsUnique()
                    .HasDatabaseName("IX_Books_Isbn_Unique");

                // Mezők konfigurálása
                entity.Property(b => b.Isbn)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(b => b.Title)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(b => b.Author)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(b => b.NumberOfCopies)
                    .IsRequired()
                    .HasDefaultValue(1);

                entity.Property(b => b.IsAvailable)
                    .IsRequired()
                    .HasDefaultValue(true);

                // Index a kereséshez
                entity.HasIndex(b => b.Title)
                    .HasDatabaseName("IX_Books_Title");

                entity.HasIndex(b => b.Author)
                    .HasDatabaseName("IX_Books_Author");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}