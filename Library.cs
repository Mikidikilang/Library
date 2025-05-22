using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class Library
    {
        //mező
        private List<Book> _books;


        //konstruktor
        public Library()
        {
            _books = new List<Book>();
        }

        //metódusok

        public void AddBook(Book book)
        {
            _books.Add(book);
            Console.WriteLine("Sikeresen hozzá adtuk a könyvet!");
            Thread.Sleep(2000);
        }

        public void AddBook(string title, string author, string isbn)
        {
            var book = new Book(title, author, isbn);
            _books.Add(book);
            Console.WriteLine("Sikeresen hozzá adtuk a könyvet!");
            Thread.Sleep(2000);
        }

        public void LendBook(string isbn)
        {
            var book = FindBookByIsbn(isbn);
            if(book == null)
            {
                Console.WriteLine("Sajnos a könyv nem található!");
            } else if (!book.IsAvailable){
                Console.WriteLine("Sajnos a könvyből nincs bent szabad példány!");
            } else {
                book.IsAvailable = book.IsAvailable;
                Console.WriteLine("Sikeres kölcsönzés!");
            }
            Thread.Sleep(2000);
        }

        public void ReturnBook(string isbn)
        {
            var book = FindBookByIsbn(isbn);
            if (book == null)
            {
                Console.WriteLine("Sajnos a könyv nem található!");
            }
            else
            {
                book.IsAvailable = book.IsAvailable;
                Console.WriteLine("Sikeres visszahozatal!");
            }
            Thread.Sleep(2000);
        }
        public void ListAllBooks()
        {
            foreach(var book in _books)
            {
                Console.WriteLine(book.GetDetails());
            }
            Thread.Sleep(2000);
        }
        public void SearchBook(string searchItem)
        {
            var matches = _books.Where(book => book.Title.Contains(searchItem) || book.Author.Contains(searchItem));
            if(!matches.Any())
            {
                Console.WriteLine("Nincs sajnos egy egyezés sem!");
                return;
            }

            foreach(var book in matches)
            {
                Console.WriteLine(book.GetDetails());
            }
            Thread.Sleep(2000);
        }

        //segéd függvények
        private Book? FindBookByIsbn(string isbn) => _books.FirstOrDefault(book => book.Isbn == isbn);
        


    }
}
