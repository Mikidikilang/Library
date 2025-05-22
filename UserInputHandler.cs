using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Library
{
    class UserInputHandler
    {
        public static int UserChoice()
        {
            while(true)
            {
                string inputStr = Console.ReadLine();
                int input;
                if (int.TryParse(inputStr, out input))
                {
                    return input;
                }
            }
        }
        public static Book GetBookFromUser()
        {
            Console.WriteLine("Cím: ");
            string title = Console.ReadLine();
            Console.WriteLine("Szerző: ");
            string author = Console.ReadLine();
            Console.WriteLine("ISBN: ");
            string isbn = Console.ReadLine();
            Book book = new Book(title, author, isbn);
            return book;
        }

        public static string GetIsbn()
        {
            Console.WriteLine("ISBN: ");
            string isbn = Console.ReadLine();
            return isbn;
        }

        public static string GetAuthorOrTitle()
        {
            Console.WriteLine("Kérlek adj meg egy szerzőt vagy címet: ");
            string search_object = Console.ReadLine();
            return search_object;
        }

    }
}
