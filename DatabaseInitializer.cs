using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// Adatbázis inicializálásért felelős osztály
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Inicializálja az adatbázist és lefuttatja a migrációkat
        /// </summary>
        /// <param name="context">A LibraryContext példány</param>
        /// <returns>True, ha az inicializálás sikeres volt</returns>
        public static async Task<bool> InitializeAsync(LibraryContext context)
        {
            try
            {
                Console.WriteLine("Adatbázis kapcsolat ellenőrzése...");

                // Ellenőrizzük a kapcsolatot
                await context.Database.OpenConnectionAsync();
                await context.Database.CloseConnectionAsync();

                Console.WriteLine("✓ Adatbázis kapcsolat sikeres");

                // Migrációk futtatása
                Console.WriteLine("Migrációk ellenőrzése és futtatása...");
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    Console.WriteLine("Függőben lévő migrációk futtatása...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine("✓ Migrációk sikeresen lefutottak");
                }
                else
                {
                    Console.WriteLine("✓ Nincs függőben lévő migráció");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hiba az adatbázis inicializálása során: {ex.Message}");
                Console.WriteLine("\nLehetséges okok:");
                Console.WriteLine("1. SQL Server nem fut");
                Console.WriteLine("2. Hibás connection string az appsettings.json-ben");
                Console.WriteLine("3. Nincs megfelelő jogosultság az adatbázis létrehozására");
                Console.WriteLine("\nEllenőrizze a következőket:");
                Console.WriteLine("- SQL Server Developer Edition fut-e");
                Console.WriteLine("- A connection string helyes-e az appsettings.json-ben");
                Console.WriteLine("- Windows Authentication vagy SQL Authentication használata");

                return false;
            }
        }

        /// <summary>
        /// Szinkron verzió az adatbázis inicializálásához
        /// </summary>
        /// <param name="context">A LibraryContext példány</param>
        /// <returns>True, ha az inicializálás sikeres volt</returns>
        public static bool Initialize(LibraryContext context)
        {
            try
            {
                Console.WriteLine("Adatbázis kapcsolat ellenőrzése...");

                // Adatbázis létrehozása, ha nem létezik
                context.Database.EnsureCreated();
                Console.WriteLine("✓ Adatbázis elérhető");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hiba az adatbázis inicializálása során: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Teszteli az adatbázis kapcsolatot
        /// </summary>
        /// <param name="context">A LibraryContext példány</param>
        /// <returns>True, ha a kapcsolat működik</returns>
        public static async Task<bool> TestConnectionAsync(LibraryContext context)
        {
            try
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.CloseConnectionAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}