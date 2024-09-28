using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HebrewBooks
{
    public class BookEntry
    {
        public string Title { get; set; }
        public string ID_Book { get; set; }
        public string Author { get; set; }
        public string Printing_Place { get; set; }
        public string Printing_Year { get; set; }
        public string Pub_Place { get; set; }
        public string Pub_Date { get; set; }
        public string Pages { get; set; }
        public string Catalog_Information { get; set; }
        public string Content { get; set; }
        public string Tags { get; set; }

        public BookEntry(string title, string iD_Book, string author, string printing_Place, string printing_Year, string pub_Place, string pub_Date, string pages, string catalog_Information, string content, string tags)
        {
            ID_Book = iD_Book;
            Title = title;
            Author = author;
            if (!string.IsNullOrEmpty(printing_Place)) Printing_Place = "- דפוס " + printing_Place;
            Printing_Year = printing_Year;
            Pub_Place = pub_Place;
            Pub_Date = " - " + pub_Date;
            Pages = pages;
            Catalog_Information = catalog_Information;
            Content = content;
            Tags = tags.Replace(";", ", ").Trim('"');
        }
    }
}
