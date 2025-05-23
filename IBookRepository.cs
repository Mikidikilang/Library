using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// Interfész a könyv adatbázis műveletek definiálásához.
    /// Meghatározza az összes szükséges CRUD műveletet és keresési funkciókat.
    /// </summary>
    public interface IBookRepository : IDisposable
    {
        /// <summary>
        /// Hozzáad egy új könyvet az adatbázishoz, vagy növeli a meglévő könyv példányszámát.
        /// </summary>
        /// <param name="book">A hozzáadandó könyv objektum</param>
        /// <returns>True, ha új könyv lett hozzáadva; False, ha meglévő könyv példányszáma lett növelve</returns>
        /// <exception cref="ArgumentNullException">Ha a book paraméter null</exception>
        /// <exception cref="InvalidOperationException">Adatbázis hiba esetén</exception>
        Task<bool> AddBookAsync(Book book);

        /// <summary>
        /// Megkeresi a könyvet ISBN alapján.
        /// </summary>
        /// <param name="isbn">A keresett könyv ISBN száma</param>
        /// <returns>A megtalált könyv objektum, vagy null ha nem található</returns>
        /// <exception cref="ArgumentException">Ha az ISBN üres vagy null</exception>
        Task<Book?> GetBookByIsbnAsync(string isbn);

        /// <summary>
        /// Visszaadja az összes könyvet az adatbázisból.
        /// </summary>
        /// <returns>Az összes könyv listája</returns>
        Task<IEnumerable<Book>> GetAllBooksAsync();

        /// <summary>
        /// Frissíti egy meglévő könyv adatait az adatbázisban.
        /// </summary>
        /// <param name="book">A frissítendő könyv objektum</param>
        /// <returns>True, ha a frissítés sikeres volt</returns>
        /// <exception cref="ArgumentNullException">Ha a book paraméter null</exception>
        /// <exception cref="InvalidOperationException">Ha a könyv nem található az adatbázisban</exception>
        Task<bool> UpdateBookAsync(Book book);

        /// <summary>
        /// Törli a könyvet az adatbázisból ISBN alapján.
        /// </summary>
        /// <param name="isbn">A törlendő könyv ISBN száma</param>
        /// <returns>True, ha a törlés sikeres volt</returns>
        /// <exception cref="ArgumentException">Ha az ISBN üres vagy null</exception>
        Task<bool> DeleteBookAsync(string isbn);

        /// <summary>
        /// Megkeresi a könyveket cím vagy szerző alapján.
        /// </summary>
        /// <param name="searchTerm">A keresési kifejezés</param>
        /// <returns>A keresési feltételnek megfelelő könyvek listája</returns>
        /// <exception cref="ArgumentException">Ha a searchTerm üres vagy null</exception>
        Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm);

        /// <summary>
        /// Megkeresi az elérhető könyveket (amelyekből van készleten).
        /// </summary>
        /// <returns>Az elérhető könyvek listája</returns>
        Task<IEnumerable<Book>> GetAvailableBooksAsync();

        /// <summary>
        /// Megkeresi a nem elérhető könyveket (amelyekből nincs készleten).
        /// </summary>
        /// <returns>A nem elérhető könyvek listája</returns>
        Task<IEnumerable<Book>> GetUnavailableBooksAsync();

        /// <summary>
        /// Ellenőrzi, hogy létezik-e könyv a megadott ISBN-nel.
        /// </summary>
        /// <param name="isbn">Az ellenőrizendő ISBN szám</param>
        /// <returns>True, ha létezik a könyv</returns>
        /// <exception cref="ArgumentException">Ha az ISBN üres vagy null</exception>
        Task<bool> BookExistsAsync(string isbn);

        /// <summary>
        /// Visszaadja a könyvek teljes számát az adatbázisban.
        /// </summary>
        /// <returns>A könyvek száma</returns>
        Task<int> GetBookCountAsync();

        /// <summary>
        /// Visszaadja egy adott könyv összes példányának számát.
        /// </summary>
        /// <param name="isbn">A könyv ISBN száma</param>
        /// <returns>A példányok száma, vagy 0 ha a könyv nem található</returns>
        /// <exception cref="ArgumentException">Ha az ISBN üres vagy null</exception>
        Task<int> GetBookCopyCountAsync(string isbn);

        /// <summary>
        /// Szinkron verzió: Hozzáad egy új könyvet az adatbázishoz, vagy növeli a meglévő könyv példányszámát.
        /// </summary>
        /// <param name="book">A hozzáadandó könyv objektum</param>
        /// <returns>True, ha új könyv lett hozzáadva; False, ha meglévő könyv példányszáma lett növelve</returns>
        bool AddBook(Book book);

        /// <summary>
        /// Szinkron verzió: Megkeresi a könyvet ISBN alapján.
        /// </summary>
        /// <param name="isbn">A keresett könyv ISBN száma</param>
        /// <returns>A megtalált könyv objektum, vagy null ha nem található</returns>
        Book? GetBookByIsbn(string isbn);

        /// <summary>
        /// Szinkron verzió: Visszaadja az összes könyvet az adatbázisból.
        /// </summary>
        /// <returns>Az összes könyv listája</returns>
        IEnumerable<Book> GetAllBooks();

        /// <summary>
        /// Szinkron verzió: Frissíti egy meglévő könyv adatait az adatbázisban.
        /// </summary>
        /// <param name="book">A frissítendő könyv objektum</param>
        /// <returns>True, ha a frissítés sikeres volt</returns>
        bool UpdateBook(Book book);

        /// <summary>
        /// Szinkron verzió: Megkeresi a könyveket cím vagy szerző alapján.
        /// </summary>
        /// <param name="searchTerm">A keresési kifejezés</param>
        /// <returns>A keresési feltételnek megfelelő könyvek listája</returns>
        IEnumerable<Book> SearchBooks(string searchTerm);
    }
}