using Library;
using System;
using System.Threading.Tasks;

namespace LibraryApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("          KÖNYVTÁR KEZELŐ RENDSZER        ");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // Adatbázis inicializálás
            using var context = new LibraryContext();

            Console.WriteLine("Adatbázis inicializálása...");
            bool dbInitialized = await DatabaseInitializer.InitializeAsync(context);

            if (!dbInitialized)
            {
                Console.WriteLine("\n❌ Az alkalmazás nem indítható el adatbázis kapcsolat nélkül.");
                Console.WriteLine("\nNyomjon meg egy gombot a kilépéshez...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("✓ Adatbázis sikeresen inicializálva");
            Console.WriteLine("\nAlkalmazás indítása...");

            // Kis szünet a visszajelzés olvashatósága érdekében
            await Task.Delay(2000);

            // Menü indítása
            MenuHandler menuHandler = new MenuHandler();
            menuHandler.Run();
        }
    }
}