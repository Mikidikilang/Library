using System;
using System.Threading;

namespace Library
{
    /// <summary>
    /// Kezeli a könyvtár alkalmazás menüjét és a felhasználói interakciókat.
    /// </summary>
    public class MenuHandler
    {
        private readonly Library _library;

        /// <summary>
        /// Konstruktor a MenuHandler osztályhoz.
        /// Inicializálja a Library osztály egy példányát.
        /// </summary>
        public MenuHandler()
        {
            _library = new Library();
        }

        /// <summary>
        /// Elindítja a könyvtár alkalmazás fő menüjét és a felhasználói interakciót.
        /// </summary>
        public void Run()
        {
            using (_library)
            {
                while (true)
                {
                    DisplayMenu();
                    int choice = UserInputHandler.UserChoice(1, 6);

                    switch (choice)
                    {
                        case 1:
                            AddBook();
                            break;
                        case 2:
                            LendBook();
                            break;
                        case 3:
                            ReturnBook();
                            break;
                        case 4:
                            ListAllBooks();
                            break;
                        case 5:
                            SearchBook();
                            break;
                        case 6:
                            Console.WriteLine("Viszlát!");
                            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
                            return;
                        default:
                            Console.WriteLine("Érvénytelen választás. Kérjük, próbálja újra.");
                            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Megjeleníti a fő menü opcióit a konzolon.
        /// </summary>
        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("             KÖNYVTÁR KEZELŐ              ");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("1. Könyv hozzáadása");
            Console.WriteLine("2. Könyv kölcsönzése");
            Console.WriteLine("3. Könyv visszahozatala");
            Console.WriteLine("4. Összes könyv listázása");
            Console.WriteLine("5. Könyv keresése");
            Console.WriteLine("6. Kilépés");
            Console.WriteLine("------------------------------------------");
            Console.Write("Válasszon egy opciót: ");
        }

        private void ClearConsoleAndDisplayHeader(string header)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"             {header.ToUpper()}              ");
            Console.WriteLine("------------------------------------------");
        }

        private void PauseForUserInput()
        {
            Console.WriteLine("\nNyomjon meg egy gombot a folytatáshoz...");
            Console.ReadKey();
        }

        private void AddBook()
        {
            ClearConsoleAndDisplayHeader("Könyv hozzáadása");
            Book newBook = UserInputHandler.GetBookFromUser();
            var result = _library.AddBook(newBook);
            Console.WriteLine(result.Message);
            PauseForUserInput();
        }

        private void LendBook()
        {
            ClearConsoleAndDisplayHeader("Könyv kölcsönzése");
            string isbn = UserInputHandler.GetIsbn();
            var result = _library.LendBook(isbn);
            Console.WriteLine(result.Message);
            PauseForUserInput();
        }

        private void ReturnBook()
        {
            ClearConsoleAndDisplayHeader("Könyv visszahozatala");
            string isbn = UserInputHandler.GetIsbn();
            var result = _library.ReturnBook(isbn);
            Console.WriteLine(result.Message);
            PauseForUserInput();
        }

        private void ListAllBooks()
        {
            ClearConsoleAndDisplayHeader("Összes könyv listázása");
            var (success, message, books) = _library.ListAllBooks();
            Console.WriteLine(message);
            if (success && books.Any())
            {
                foreach (var book in books)
                {
                    Console.WriteLine(book.GetDetails());
                }
            }
            PauseForUserInput();
        }

        private void SearchBook()
        {
            ClearConsoleAndDisplayHeader("Könyv keresése");
            string searchItem = UserInputHandler.GetAuthorOrTitle();
            var (success, message, books) = _library.SearchBook(searchItem);
            Console.WriteLine(message);
            if (success && books.Any())
            {
                foreach (var book in books)
                {
                    Console.WriteLine(book.GetDetails());
                }
            }
            PauseForUserInput();
        }
    }
}