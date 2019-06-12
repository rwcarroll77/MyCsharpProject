using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Book
    {
        public Book(string bookID, string title, string authorID, string publisher,
                    string genre, string description, string status)
        {
            BookID = bookID;
            Title = title;
            AuthorID = authorID;
            Publisher = publisher;
            Genre = genre;
            Description = description;
            Status = status;
        }

        public string BookID { get; set; }
        public string Title { get; set; }
        public string AuthorID { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        // BookID
        //Title
        //AuthorID
        //Publisher
        //Genre
        //Description
        //Status

    }
}
