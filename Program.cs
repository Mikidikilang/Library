using System.Diagnostics;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            bool fut = true;

            while(fut)
            {
                PrintMenu();
                UserChoice(library);
            }

        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Könyv hozzáadása!");
            Console.WriteLine("2. Könyv kölcsönzése!");
            Console.WriteLine("3. Könyv visszavétele!");
            Console.WriteLine("4. Összes könyv listázása!");
            Console.WriteLine("5. Könvy keresése!");
            Console.WriteLine("6. Kilépés!");
        }

        static void UserChoice(Library library)
        {
            int input = UserInputHandler.UserChoice();
            switch(input)
            {
                case 1:
                    Console.Clear();
                    Book book = UserInputHandler.GetBookFromUser();
                    library.AddBook(book);
                    break;
                case 2:
                    Console.Clear();
                    string isbn_out = UserInputHandler.GetIsbn();
                    library.LendBook(isbn_out);
                    break;
                case 3:
                    Console.Clear();
                    string isbn_in = UserInputHandler.GetIsbn();
                    library.ReturnBook(isbn_in);
                    break;
                case 4:
                    Console.Clear();
                    library.ListAllBooks();
                    break;
                case 5:
                    Console.Clear();
                    string search_object = UserInputHandler.GetAuthorOrTitle();
                    library.SearchBook(search_object);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("Köszi, hogy nálnuk voltál!");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nem értettem pontosan!");
                    break;
            }
        }
    }
}
