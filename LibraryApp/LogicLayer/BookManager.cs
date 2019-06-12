using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataObjects;

namespace LogicLayer
{
    public class BookManager
    {
        public List<VMBook> RetrieveAllBooks()
        {
            List<VMBook> bookList = null;

            try
            {
                bookList = BookAccessor.SelectAllBooks();
            }
            catch (Exception)
            {

                throw;
            }

            return bookList;
        }

        public List<VMBook> RetrieveBorrowedBooksByCustomer(int customerID)
        {
            List<VMBook> bookList = null;

            try
            {
                bookList = BookAccessor.SelectBorrowedBooksByCustomer(customerID);
            }
            catch (Exception)
            {

                throw;
            }

            return bookList;
        }

        public bool BorrowBook(int customerID, DateTime borrowDate, string bookID, string status, string oldStatus)
        {
            bool result = false;

            try
            {
                result = BookAccessor.BorrowBook(customerID, borrowDate, bookID, status, oldStatus);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public bool ReturnBook(int customerID, DateTime returnDate, string bookID, string status, string oldStatus)
        {
            bool result = false;

            try
            {
                result = BookAccessor.ReturnBook(customerID, returnDate, bookID, status, oldStatus);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public bool AddBook(VMBook book)
        {
            bool result = false;

            try
            {
                result = (1 == BookAccessor.InsertBook(book));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public bool UpdateBook(VMBook book, VMBook oldBook)
        {
            bool result = false;

            try
            {
                result = (1 == BookAccessor.UpdateBook(book, oldBook));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public bool DeleteBook(string bookID, string title)
        {
            bool result = false;

            try
            {
                result = (1 == BookAccessor.DeleteBook(bookID, title));
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public VMBook RetrieveBookByID(string id)
        {
            VMBook book = null;

            try
            {
                book = BookAccessor.SelectBookByID(id);
            }
            catch (Exception)
            {

                throw;
            }

            return book;
        }
    }
}