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
            Console.Clear(); // Törli a konzol tartalmát a menü megjelenítése előtt
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
            Console.Write("Válasszon egy opciót: "); // Kiegészített prompt
        }

        /// <summary>
        /// Törli a konzolt és megjeleníti a megadott fejlécet.
        /// </summary>
        /// <param name="header">A megjelenítendő fejléc szövege.</param>
        private void ClearConsoleAndDisplayHeader(string header)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"             {header.ToUpper()}              ");
            Console.WriteLine("------------------------------------------");
        }

        /// <summary>
        /// Megállítja a program futását, amíg a felhasználó meg nem nyom egy gombot.
        /// </summary>
        private void PauseForUserInput()
        {
            Console.WriteLine("\nNyomjon meg egy gombot a folytatáshoz...");
            Console.ReadKey();
        }

        /// <summary>
        /// Bekéri a könyv adatait a felhasználótól és hozzáadja a könyvtárhoz.
        /// </summary>
        private void AddBook()
        {
            ClearConsoleAndDisplayHeader("Könyv hozzáadása");
            Book newBook = UserInputHandler.GetBookFromUser();
            _library.AddBook(newBook);
            PauseForUserInput(); // Vár a felhasználóra a művelet után
        }

        /// <summary>
        /// Bekéri a kölcsönözni kívánt könyv ISBN számát és kölcsönzi azt.
        /// </summary>
        private void LendBook()
        {
            ClearConsoleAndDisplayHeader("Könyv kölcsönzése");
            string isbn = UserInputHandler.GetIsbn();
            _library.LendBook(isbn);
            PauseForUserInput(); // Vár a felhasználóra a művelet után
        }

        /// <summary>
        /// Bekéri a visszahozni kívánt könyv ISBN számát és visszahozza azt.
        /// </summary>
        private void ReturnBook()
        {
            ClearConsoleAndDisplayHeader("Könyv visszahozatala");
            string isbn = UserInputHandler.GetIsbn();
            _library.ReturnBook(isbn);
            PauseForUserInput(); // Vár a felhasználóra a művelet után
        }

        /// <summary>
        /// Listázza az összes könyvet a könyvtárban.
        /// </summary>
        private void ListAllBooks()
        {
            ClearConsoleAndDisplayHeader("Összes könyv listázása");
            _library.ListAllBooks();
            PauseForUserInput(); // Vár a felhasználóra a művelet után
        }

        /// <summary>
        /// Bekéri a keresési kifejezést és megkeresi a megfelelő könyveket.
        /// </summary>
        private void SearchBook()
        {
            ClearConsoleAndDisplayHeader("Könyv keresése");
            string searchItem = UserInputHandler.GetAuthorOrTitle();
            _library.SearchBook(searchItem);
            PauseForUserInput(); // Vár a felhasználóra a művelet után
        }
    }
}
