using DataObjects;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataAccessLayer
{
    public class BookAccessor
    {
        public static List<VMBook> SelectAllBooks()
        {
            List<VMBook> bookList = new List<VMBook>();
            VMBook book;

            string bookID;
            string title;
            string authorID;
            string authorName;
            string publisher;
            string genre;
            string description;
            string status;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_show_all_books";
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
                        bookID = r.GetString(0);
                        title = r.GetString(1);
                        authorID = r.GetString(2);
                        publisher = r.GetString(3);
                        genre = r.GetString(4);
                        description = r.GetString(5);
                        status = r.GetString(6);
                        authorName = r.GetString(7) + " " + r.GetString(8);
                        book = new VMBook(bookID, title, authorID, publisher, genre, description,
                            status, authorName);
                        bookList.Add(book);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return bookList;
        }

        public static bool BorrowBook(int customerID, DateTime borrowDate, string bookID, string status, string oldStatus)  
        {
            bool result = false;

            var cmdText1 = @"sp_insert_borrow_record";
            var cmdText2 = @"sp_update_book_status";
            


            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = Connection.GetDbConnection())
                    {
                        conn.Open();

                        var cmd1 = new SqlCommand(cmdText1, conn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd1.Parameters.AddWithValue("@DateBorrowed", borrowDate);
                        cmd1.Parameters.AddWithValue("@BookID", bookID);
                        cmd1.ExecuteNonQuery();

                        var cmd2 = new SqlCommand(cmdText2, conn);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@BookID", bookID);
                        cmd2.Parameters.AddWithValue("@Status", status);
                        cmd2.Parameters.AddWithValue("@OldStatus", oldStatus);
                        cmd2.ExecuteNonQuery();


                    }
                    scope.Complete();
                    result = true;
                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static bool ReturnBook(int customerID, DateTime returnDate, string bookID, string status, string oldStatus)
        {
            bool result = false;

            var cmdText1 = @"sp_return_borrowed_book";
            var cmdText2 = @"sp_update_book_status";

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = Connection.GetDbConnection())
                    {
                        conn.Open();
                        var cmd1 = new SqlCommand(cmdText1, conn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@DateReturned", returnDate);
                        cmd1.Parameters.AddWithValue("@BookID", bookID);
                        cmd1.Parameters.AddWithValue("@CustomerID", customerID);
                        cmd1.ExecuteNonQuery();

                        var cmd2 = new SqlCommand(cmdText2, conn);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@BookID", bookID);
                        cmd2.Parameters.AddWithValue("@Status", status);
                        cmd2.Parameters.AddWithValue("@OldStatus", oldStatus);
                        cmd2.ExecuteNonQuery();
                    }
                    scope.Complete();
                    result = true;
                }
            }
            catch (Exception)
            {throw;}
            return result;
        }

        public static List<VMBook> SelectBorrowedBooksByCustomer(int customerID)
        {
            List<VMBook> bookList = new List<VMBook>();
            VMBook book;

            string bookID;
            string title;
            string authorID;
            string authorName;
            string publisher;
            string genre;
            string description;
            string status;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_select_borrowed_books_by_customer";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        bookID = r.GetString(0);
                        title = r.GetString(1);
                        authorID = r.GetString(2);
                        publisher = r.GetString(3);
                        genre = r.GetString(4);
                        description = r.GetString(5);
                        status = r.GetString(6);
                        authorName = r.GetString(7) + " " + r.GetString(8);
                        book = new VMBook(bookID, title, authorID, publisher, genre, description,
                            status, authorName);
                        bookList.Add(book);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return bookList;
        }

        public static int InsertBook(VMBook book)
        {
            int result = 0;

            var cmdText = @"sp_insert_book";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@BookID", book.BookID);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
            cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Description", book.Description);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }



            return result;
        }
        
        public static int UpdateBook(VMBook book, VMBook oldBook)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_update_book";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", book.BookID);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
            cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Description", book.Description);

            cmd.Parameters.AddWithValue("@OldBookID", oldBook.BookID);
            cmd.Parameters.AddWithValue("@OldTitle", oldBook.Title);
            cmd.Parameters.AddWithValue("@OldAuthorID", oldBook.AuthorID);
            cmd.Parameters.AddWithValue("@OldPublisher", oldBook.Publisher);
            cmd.Parameters.AddWithValue("@OldGenre", oldBook.Genre);
            cmd.Parameters.AddWithValue("@OldDescription", oldBook.Description);

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public static int DeleteBook(string bookID, string title)
        {
            int result = 0;

            var conn = Connection.GetDbConnection();
            var cmdText = @"sp_deactivate_book";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@BookID", bookID);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static VMBook SelectBookByID(string id)
        {
            VMBook book = null;

            var cmdText = "sp_select_book_by_id";
            var conn = Connection.GetDbConnection();
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", id);

            try
            {
                conn.Open();
                var r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        book = new VMBook
                        {
                            BookID = r.GetString(0),
                            Title = r.GetString(1),
                            AuthorID = r.GetString(2),
                            AuthorName = r.GetString(3),
                            Publisher = r.GetString(4),
                            Genre = r.GetString(5),
                            Description = r.GetString(6),
                            Status = r.GetString(7)
                        };
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return book;
        }
        

    }
}
//Create Procedure[dbo].[sp_show_all_books]
//AS
//   Begin
//        Select[VMBook].[BookID], [VMBook].[Title], [VMBook].[AuthorID], [VMBook].[Publisher], [VMBook].[Genre], 
//        [VMBook].[Description], [VMBook].[Status], [Author].[FirstName], [Author].[LastName]
//From VMBook Join Author on Author.AuthorID = VMBook.AuthorID

//public string BookID { get; set; }
//public string Title { get; set; }
//public string AuthorID { get; set; }
//public string AuthorName { get; set; }
//public string Publisher { get; set; }
//public string Genre { get; set; }
//public string Description { get; set; }
//public string Status { get; set; }
                                    