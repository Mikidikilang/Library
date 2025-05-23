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
        public static int UserChoice(int minValue, int maxValue)
        {
            while (true)
            {
                Console.Write($"Kérjük adjon meg egy számot {minValue} és {maxValue} között: ");
                string? inputStr = Console.ReadLine();

                if (int.TryParse(inputStr, out int input) && input >= minValue && input <= maxValue)
                {
                    return input;
                }

                Console.WriteLine($"Érvénytelen bemenet. Kérjük adjon meg egy számot {minValue} és {maxValue} között.");
            }
        }

        /// <summary>
        /// Bekéri egy könyv adatait a felhasználótól és létrehoz egy Book objektumot
        /// </summary>
        /// <returns>A felhasználó által megadott adatokkal létrehozott Book objektum.</returns>
        public static Book GetBookFromUser()
        {
            string title = GetNonEmptyInput("Cím: ");
            string author = GetNonEmptyInput("Szerző: ");
            string isbn = GetNonEmptyInput("ISBN: ");
            int numberOfCopies = GetPositiveIntegerInput("Példányszám: ");

            return new Book(title, author, isbn, numberOfCopies);
        }

        /// <summary>
        /// Bekéri a könyv ISBN számát a felhasználótól.
        /// </summary>
        /// <returns>A felhasználó által megadott ISBN szám</returns>
        public static string GetIsbn()
        {
            return GetNonEmptyInput("ISBN: ");
        }

        /// <summary>
        /// Bekéri a keresési kifejezést (szerző vagy cím) a felhasználótól.
        /// </summary>
        /// <returns>A felhasználó által megadott keresési kifejezés.</returns>
        public static string GetAuthorOrTitle()
        {
            return GetNonEmptyInput("Kérlek adj meg egy szerzőt vagy címet: ");
        }

        /// <summary>
        /// Segéd metódus, ami biztosítja, hogy a felhasználó ne adjon meg üres vagy csak szóközt tartalmazó inputot.
        /// </summary>
        /// <param name="prompt">A felhasználónak megjelenő üzenet.</param>
        /// <returns>A felhasználó által megadott, nem üres és trimelt string.</returns>
        private static string GetNonEmptyInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("Ez a mező nem lehet üres! Próbálja újra.");
            }
        }

        /// <summary>
        /// Segéd metódus, ami pozitív egész számot kér be a felhasználótól.
        /// </summary>
        /// <param name="prompt">A felhasználónak megjelenő üzenet.</param>
        /// <returns>A felhasználó által megadott pozitív egész szám.</returns>
        private static int GetPositiveIntegerInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? inputStr = Console.ReadLine();
                if (int.TryParse(inputStr, out int input) && input >= 0)
                {
                    return input;
                }
                Console.WriteLine("Kérjük adjon meg egy érvényes, nem negatív egész számot: ");
            }
        }

    }
}
