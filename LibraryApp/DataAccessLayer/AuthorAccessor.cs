using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class AuthorAccessor
    {
        public static List<string> SelectAllAuthorIds()
        {
            List<string> idList = new List<string>();

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_select_all_author_ids";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        idList.Add(r.GetString(0));
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return idList;
        }

        public static List<Author> SelectAllAuthors()
        {
            List<Author> authors = new List<Author>();

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_select_all_authors";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            string authorID;
            string firstName;
            string lastName;
            string primaryGenre;
            string description;

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        authorID = r.GetString(0);
                        firstName = r.GetString(1);
                        lastName = r.GetString(2);
                        primaryGenre = r.GetString(3);
                        description = r.GetString(4);
                        Author author = new Author(authorID, firstName, lastName, primaryGenre, description);
                        authors.Add(author);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return authors;
        }

        public static Author SelectAuthorByID(string authorID)
        {
            Author author = new Author();

            var cmdText = @"sp_select_author_by_id";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AuthorID", authorID);

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    author.AuthorID = r.GetString(0);
                    author.FirstName = r.GetString(1);
                    author.LastName = r.GetString(2);
                    author.PrimaryGenre = r.GetString(3);
                    author.Description = r.GetString(4);

                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return author;
        }
    }
}
