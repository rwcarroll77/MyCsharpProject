using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class AuthorManager
    {
        public List<string> RetrieveAllAuthorIDs()
        {
            List<string> idList = new List<string>();

            try
            {
                idList = AuthorAccessor.SelectAllAuthorIds();
            }
            catch (Exception)
            {

                throw;
            }

            return idList;
        }

        public Author RetrieveAuthorByID(string authorID)
        {
            Author author = null;

            try
            {
                author = AuthorAccessor.SelectAuthorByID(authorID);
            }
            catch (Exception)
            {

                throw;
            }

            return author;
        }

        public List<Author> RetrieveAllAuthors()
        {
            List<Author> authors = new List<Author>();

            try
            {
                authors = AuthorAccessor.SelectAllAuthors();
            }
            catch (Exception)
            {

                throw;
            }

            return authors;
        }
    }
}
