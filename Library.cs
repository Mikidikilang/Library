using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Library
{
    class Library : IDisposable
    {
        private readonly LibraryContext _context;

        public Library()
        {
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
        }

        //metódusok

        public void AddBook(Book book)
        {
            try
            {
                var existingBook = _context.Books.FirstOrDefault(b => b.Isbn == book.Isbn);

                if(existingBook != null)
                {
                    existingBook.NumberOfCopies += book.NumberOfCopies;
                    Console.WriteLine($"Ez az ISBN már létezik. Példányszám növelve: {existingBook.NumberOfCopies}");
                    _context.Books.Update(existingBook);
                }
                else
                {
                    _context.Books.Add(book);
                    Console.WriteLine("Sikeresen hozzá adtuk a könyvet!");
                }
                _context.SaveChanges();
            } catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a könyv hozzáadásakor: {ex.Message}");
            }
            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
        }

        public void AddBook(string title, string author, string isbn, int numberOfCopies)
        {
            var book = new Book(title, author, isbn, numberOfCopies);
            AddBook(book);
        }

        public void LendBook(string isbn)
        {
            try
            {
                var book = FindBookByIsbn(isbn);
                if (book == null)
                {
                    Console.WriteLine("Sajnos a könyv nem található!");
                } else if (book.NumberOfCopies <= 0) {
                    Console.WriteLine("Sajnos a könvyből nincs bent szabad példány!");
                } else
                {
                    book.NumberOfCopies--;
                    if (book.NumberOfCopies == 0)
                    {
                        book.IsAvailable = false;
                    }
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    Console.WriteLine($"Sikeres kölcsönzés! Maradt példány: {book.NumberOfCopies}");
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a kölcsönzés során: {ex.Message}");
            }
            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
        }

        public void ReturnBook(string isbn)
        {
            try
            {
                var book = FindBookByIsbn(isbn);
                if (book == null)
                {
                    Console.WriteLine("Sajnos a könyv nem található!");
                } else
                {
                    book.NumberOfCopies++;
                    if (book.NumberOfCopies > 0)
                    {
                        book.IsAvailable = true;
                    }
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    Console.WriteLine($"Sikeres visszahozatal! Jelenlegi példányszám: {book.NumberOfCopies}");
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a  visszahozás során: {ex.Message}");
            }
            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
        }
        public void ListAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                if(!books.Any())
                {
                    Console.WriteLine("Nincsenek könyvek az adatbázisban.");
                } else
                {
                    Console.WriteLine("A könyvek tartalma:");
                    Console.WriteLine();
                    foreach(var book in books)
                    {
                        Console.WriteLine(book.GetDetails());
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a könyvek listázásakor: {ex.Message}");
            }
            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
        }
        public void SearchBook(string searchItem)
        {
            try
            {
                var matches = _context.Books
                    .Where(book => book.Title.Contains(searchItem) || book.Author.Contains(searchItem))
                    .ToList();
                if (!matches.Any())
                {
                    Console.WriteLine($"Nincs sajnos egyezés ezzel: {searchItem}");
                } else
                {
                    Console.WriteLine($"Találatok erre: {searchItem}");
                    Console.WriteLine();
                    foreach (var book in matches)
                    {
                        Console.WriteLine(book.GetDetails());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba történt a keresés során: {ex.Message}");
            }
            Thread.Sleep(Constants.UserFeedbackDelayMilliseconds);
        }

        //segéd függvények
        private Book? FindBookByIsbn(string isbn) => _context.Books.FirstOrDefault(book => book.Isbn == isbn);

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
