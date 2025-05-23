using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// Repository osztály a könyv adatbázis műveletek megvalósításához.
    /// Implementálja az IBookRepository interfészt és kezeli az összes adatelérési logikát.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        private bool _disposed = false;

        /// <summary>
        /// Konstruktor a BookRepository osztályhoz.
        /// </summary>
        /// <param name="context">A LibraryContext példány az adatbázis kapcsolathoz</param>
        /// <exception cref="ArgumentNullException">Ha a context paraméter null</exception>
        public BookRepository(LibraryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Alternatív konstruktor, amely létrehozza a saját context példányát.
        /// </summary>
        public BookRepository()
        {
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
        }

        #region Async Methods

        /// <inheritdoc />
        public async Task<bool> AddBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                var existingBook = await _context.Books
                    .FirstOrDefaultAsync(b => b.Isbn == book.Isbn);

                if (existingBook != null)
                {
                    existingBook.NumberOfCopies += book.NumberOfCopies;
                    if (existingBook.NumberOfCopies > 0)
                    {
                        existingBook.IsAvailable = true;
                    }
                    _context.Books.Update(existingBook);
                    await _context.SaveChangesAsync();
                    return false; 
                }
                else
                {
                    await _context.Books.AddAsync(book);
                    await _context.SaveChangesAsync();
                    return true; 
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Hiba történt a könyv hozzáadása során: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<Book?> GetBookByIsbnAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("Az ISBN nem lehet üres vagy null.", nameof(isbn));

            return await _context.Books
                .FirstOrDefaultAsync(b => b.Isbn == isbn);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> UpdateBookAsync(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                var existingBook = await _context.Books
                    .FirstOrDefaultAsync(b => b.Isbn == book.Isbn);

                if (existingBook == null)
                    throw new InvalidOperationException($"A könyv nem található az ISBN alapján: {book.Isbn}");

                
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.NumberOfCopies = book.NumberOfCopies;
                existingBook.IsAvailable = book.NumberOfCopies > 0;

                _context.Books.Update(existingBook);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Hiba történt a könyv frissítése során: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteBookAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("Az ISBN nem lehet üres vagy null.", nameof(isbn));

            try
            {
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.Isbn == isbn);

                if (book == null)
                    return false;

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Hiba történt a könyv törlése során: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("A keresési kifejezés nem lehet üres vagy null.", nameof(searchTerm));

            return await _context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await _context.Books
                .Where(b => b.IsAvailable && b.NumberOfCopies > 0)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Book>> GetUnavailableBooksAsync()
        {
            return await _context.Books
                .Where(b => !b.IsAvailable || b.NumberOfCopies <= 0)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> BookExistsAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("Az ISBN nem lehet üres vagy null.", nameof(isbn));

            return await _context.Books
                .AnyAsync(b => b.Isbn == isbn);
        }

        /// <inheritdoc />
        public async Task<int> GetBookCountAsync()
        {
            return await _context.Books.CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetBookCopyCountAsync(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("Az ISBN nem lehet üres vagy null.", nameof(isbn));

            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Isbn == isbn);

            return book?.NumberOfCopies ?? 0;
        }

        #endregion

        #region Sync Methods

        /// <inheritdoc />
        public bool AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                var existingBook = _context.Books
                    .FirstOrDefault(b => b.Isbn == book.Isbn);

                if (existingBook != null)
                {
                    existingBook.NumberOfCopies += book.NumberOfCopies;
                    if (existingBook.NumberOfCopies > 0)
                    {
                        existingBook.IsAvailable = true;
                    }
                    _context.Books.Update(existingBook);
                    _context.SaveChanges();
                    return false; 
                }
                else
                {
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    return true; 
                }
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Hiba történt a könyv hozzáadása során: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public Book? GetBookByIsbn(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("Az ISBN nem lehet üres vagy null.", nameof(isbn));

            return _context.Books
                .FirstOrDefault(b => b.Isbn == isbn);
        }

        /// <inheritdoc />
        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books
                .OrderBy(b => b.Title)
                .ToList();
        }

        /// <inheritdoc />
        public bool UpdateBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            try
            {
                var existingBook = _context.Books
                    .FirstOrDefault(b => b.Isbn == book.Isbn);

                if (existingBook == null)
                    throw new InvalidOperationException($"A könyv nem található az ISBN alapján: {book.Isbn}");

                // Frissítjük az adatokat
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.NumberOfCopies = book.NumberOfCopies;
                existingBook.IsAvailable = book.NumberOfCopies > 0;

                _context.Books.Update(existingBook);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Hiba történt a könyv frissítése során: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public IEnumerable<Book> SearchBooks(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                throw new ArgumentException("A keresési kifejezés nem lehet üres vagy null.", nameof(searchTerm));

            return _context.Books
                .Where(b => b.Title.Contains(searchTerm) || b.Author.Contains(searchTerm))
                .OrderBy(b => b.Title)
                .ToList();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Ellenőrzi, hogy az objektum el lett-e már szabadítva.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Ha az objektum már el lett szabadítva</exception>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BookRepository));
        }

        /// <summary>
        /// Biztonságosan csökkenti egy könyv példányszámát és frissíti az elérhetőségi státuszát.
        /// </summary>
        /// <param name="book">A módosítandó könyv</param>
        /// <returns>True, ha a művelet sikeres volt</returns>
        public bool DecrementBookCopy(Book book)
        {
            if (book == null || book.NumberOfCopies <= 0)
                return false;

            book.NumberOfCopies--;
            if (book.NumberOfCopies == 0)
            {
                book.IsAvailable = false;
            }

            return UpdateBook(book);
        }

        /// <summary>
        /// Biztonságosan növeli egy könyv példányszámát és frissíti az elérhetőségi státuszát.
        /// </summary>
        /// <param name="book">A módosítandó könyv</param>
        /// <returns>True, ha a művelet sikeres volt</returns>
        public bool IncrementBookCopy(Book book)
        {
            if (book == null)
                return false;

            book.NumberOfCopies++;
            if (book.NumberOfCopies > 0)
            {
                book.IsAvailable = true;
            }

            return UpdateBook(book);
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Felszabadítja a repository által használt erőforrásokat.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected dispose pattern implementáció.
        /// </summary>
        /// <param name="disposing">True, ha a Dispose metódusból hívják</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizer - biztonsági háló az erőforrások felszabadítására.
        /// </summary>
        ~BookRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}