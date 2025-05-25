using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library
{
    public class Book
    {
        [Key]
        [Required]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(13)")]
        public string Isbn { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue)]
        public int NumberOfCopies { get; set; } = 1;

        public bool IsAvailable { get; set; } = true;

        //Konstruktor
        public Book(string title, string author, string isbn, int numberOfCopies)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            NumberOfCopies = numberOfCopies;
            IsAvailable = numberOfCopies > 0;
        }

        private string AvailableStatus => IsAvailable ? "Avaiable" : "Not avaiable";



        public string GetDetails()
        {
            return $"{"Title: " + Title,-25}{"Author: " + Author,-25}{"ISBN: " + Isbn,-25}{"Copies: " + NumberOfCopies,-10}{"Status: " + AvailableStatus,-25}";
        }
    }
}
