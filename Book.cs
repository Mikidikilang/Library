using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class Book
    {
        //Field->Mezők
        private readonly string _title;
        private readonly string _author;
        private readonly string _isbn;  //egyedi azonosító
        private bool _isAvailable;


        //Konstruktor
        public Book(string title, string author, string isbn)
        {
            _title = title;
            _author = author;
            _isbn = isbn;
            _isAvailable = true;
        }

        //getterek
        public string Title => _title;
        public string Author => _author;
        public string Isbn => _isbn;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = !_isAvailable;
        } 
        private string AvailableStatus => IsAvailable ? "Available" : "Not avaiable";



        public string GetDetails()
        {
            return $"{($"Title: {Title}"), -25}{($"Author: {Author}"), -25}{($"ISBN: {Isbn}"), -25}{($"Status: {AvailableStatus}"), -25}";
        }
    }
}
