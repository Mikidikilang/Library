using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Library
{
    /// <summary>
    /// A Library osztály csak üzleti logikát tartalmaz, nem végez közvetlen felhasználói interakciót.
    /// Minden művelet eredményét OperationResult vagy adatszerkezet formájában adja vissza.
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }
        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    class Library : IDisposable
    {
        private readonly BookRepository _bookRepository;

        #region Konstruktorok
        public Library()
        {
            _bookRepository = new BookRepository();
        }
        #endregion

        #region Könyv műveletek

        public OperationResult AddBook(Book book)
        {
            try
            {
                bool isNew = _bookRepository.AddBook(book);
                if (isNew)
                {
                    return new OperationResult(true, "Sikeresen hozzáadtuk a könyvet!");
                }
                else
                {
                    var existingBook = _bookRepository.GetBookByIsbn(book.Isbn);
                    return new OperationResult(true, $"Ez az ISBN már létezik. Példányszám növelve: {existingBook?.NumberOfCopies}");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new OperationResult(false, "Hiba történt a könyv hozzáadásakor.");
            }
        }

        public OperationResult AddBook(string title, string author, string isbn, int numberOfCopies)
        {
            var book = new Book(title, author, isbn, numberOfCopies);
            return AddBook(book);
        }

        public OperationResult LendBook(string isbn)
        {
            try
            {
                var book = _bookRepository.GetBookByIsbn(isbn);
                if (book == null)
                {
                    return new OperationResult(false, "Sajnos a könyv nem található!");
                }
                else if (book.NumberOfCopies <= 0)
                {
                    return new OperationResult(false, "Sajnos a könyvből nincs bent szabad példány!");
                }
                else
                {
                    bool success = _bookRepository.DecrementBookCopy(book);
                    if (success)
                    {
                        return new OperationResult(true, $"Sikeres kölcsönzés! Maradt példány: {book.NumberOfCopies}");
                    }
                    else
                    {
                        return new OperationResult(false, "Hiba történt a példányszám csökkentésekor.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new OperationResult(false, "Hiba történt a kölcsönzés során.");
            }
        }

        public OperationResult ReturnBook(string isbn)
        {
            try
            {
                var book = _bookRepository.GetBookByIsbn(isbn);
                if (book == null)
                {
                    return new OperationResult(false, "Sajnos a könyv nem található!");
                }
                else
                {
                    bool success = _bookRepository.IncrementBookCopy(book);
                    if (success)
                    {
                        return new OperationResult(true, $"Sikeres visszavétel! Jelenlegi példányszám: {book.NumberOfCopies}");
                    }
                    else
                    {
                        return new OperationResult(false, "Hiba történt a példányszám növelésekor.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                return new OperationResult(false, "Hiba történt a visszavétel során.");
            }
        }

        #endregion

        #region Listázás és keresés

        public (bool Success, string Message, List<Book> Books) ListAllBooks()
        {
            try
            {
                var books = _bookRepository.GetAllBooks().ToList();
                if (!books.Any())
                {
                    return (true, "Nincsenek könyvek az adatbázisban.", new List<Book>());
                }
                else
                {
                    return (true, "A könyvek listája:", books);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Hiba történt a könyvek listázásakor: {ex.Message}", new List<Book>());
            }
        }

        public (bool Success, string Message, List<Book> Books) SearchBook(string searchItem)
        {
            try
            {
                var matches = _bookRepository.SearchBooks(searchItem).ToList();
                if (!matches.Any())
                {
                    return (true, $"Nincs sajnos egyezés ezzel: {searchItem}", new List<Book>());
                }
                else
                {
                    return (true, $"Találatok erre: {searchItem}", matches);
                }
            }
            catch (Exception ex)
            {
                return (false, $"Hiba történt a keresés során: {ex.Message}", new List<Book>());
            }
        }

        #endregion

        #region Segédfüggvények és IDisposable

        private Book? FindBookByIsbn(string isbn) => _bookRepository.GetBookByIsbn(isbn);

        private void LogError(Exception ex)
        {
            try
            {
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex}\n");
            }
            catch
            {
                // Ha a logolás is hibázik, ne csináljon semmit.
            }
        }

        public void Dispose()
        {
            _bookRepository?.Dispose();
        }

        #endregion
    }
}