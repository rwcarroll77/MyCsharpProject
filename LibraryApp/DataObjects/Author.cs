using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Author
    {
        public Author()
        {
        }

        public Author(string authorID, string firstName, string lastName, string primaryGenre, string description)
        {
            AuthorID = authorID;
            FirstName = firstName;
            LastName = lastName;
            PrimaryGenre = primaryGenre;
            Description = description;
        }

        public string AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryGenre { get; set; }
        public string Description { get; set; }

        // AuthorID
        //FirstName
        //LastName
        //Primary Genre
        //Description

    }
}
