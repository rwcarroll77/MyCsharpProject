using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;

namespace WebPresentation.Controllers
{
    public class BookController : Controller
    {
        private BookManager _bookManager = new BookManager();
        private CustomerManager _customerManager = new CustomerManager();
        private ApplicationUserManager userManager;
        private AuthorManager _authorManager = new AuthorManager();
        private List<Author> authors = new List<Author>();
        private List<string> authorIDs = new List<string>();

        public BookController()
        {
            try
            {
                authors = _authorManager.RetrieveAllAuthors();
                foreach (var author in authors)
                {
                    authorIDs.Add(author.AuthorID);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: VMBook
        public ActionResult Index()
        {
            List<VMBook> _books = _bookManager.RetrieveAllBooks();
            
            _books = _books.FindAll(b => b.Status == "Active");

            return View(_books);
        }

        // GET: VMBook/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var book = _bookManager.RetrieveBookByID(id);
            return View(book);
        }

        [HttpGet]
        [Authorize(Roles ="Basic")]
        // GET: VMBook/Create
        public ActionResult Create()
        {
            ViewBag.AuthorIDs = authorIDs;
            return View();

        }

        // POST: VMBook/Create
        [HttpPost]
        [Authorize(Roles = "Basic")]
        public ActionResult Create(VMBook book)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (_bookManager.AddBook(book))
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    ViewBag.AuthorIDs = authorIDs;
                    return View();
                }
            }
            
            ViewBag.AuthorIDs = authorIDs;
            return View(book);
        }
        
        [Authorize(Roles ="Guest")]
        public ActionResult Checkout(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = userManager.Users.First(u => u.UserName == User.Identity.Name);
            Customer customer = _customerManager.RetrieveCustomerByEmail(currentUser.Email);
            VMBook currentBook = _bookManager.RetrieveBookByID(id);
            try
            {
                _bookManager.BorrowBook(customer.CustomerID, DateTime.Now, currentBook.BookID, "Borrowed", currentBook.Status);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

        }

        // GET: VMBook/Edit/5
        [HttpGet]
        [Authorize(Roles ="Basic")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            VMBook book = _bookManager.RetrieveBookByID(id);
            ViewBag.AuthorIDs = authorIDs;
            return View(book);
        }

        // POST: VMBook/Edit/5
        [HttpPost]
        [Authorize(Roles = "Basic")]
        public ActionResult Edit(string id, VMBook book)
        {
            ViewBag.AuthorIDs = authorIDs;
            if (ModelState.IsValid)
            {
                try
                {
                    VMBook oldBook = _bookManager.RetrieveBookByID(id);
                    if(_bookManager.UpdateBook(book, oldBook))
                    {
                        return RedirectToAction("Index");
                    } else
                    {
                        return View(book);
                    }

                }
                catch (Exception)
                {
                    return View(book);
                }
            } else
            {
                return View(book);
            }
        }

        // GET: VMBook/Delete/5
        [Authorize(Roles = "Basic")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            VMBook book = _bookManager.RetrieveBookByID(id);

            return View(book);
        }

        // POST: VMBook/Delete/5
        [HttpPost]
        [Authorize(Roles = "Basic")]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                VMBook book = _bookManager.RetrieveBookByID(id);
                if (_bookManager.DeleteBook(book.BookID, book.Title))
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Guest")]
        public ActionResult BorrowedIndex()
        {

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = userManager.Users.First(u => u.UserName == User.Identity.Name);
            Customer customer;
            List<VMBook> borrowedBooks = new List<VMBook>();

            try
            {
                customer = _customerManager.RetrieveCustomerByEmail(currentUser.Email);
                borrowedBooks = _bookManager.RetrieveBorrowedBooksByCustomer(customer.CustomerID);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
            return View(borrowedBooks);

        }

        public ActionResult Return(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var currentUser = userManager.Users.First(u => u.UserName == User.Identity.Name);
            Customer customer = _customerManager.RetrieveCustomerByEmail(currentUser.Email);
            VMBook currentBook = _bookManager.RetrieveBookByID(id);
            try
            {
                _bookManager.ReturnBook(customer.CustomerID, DateTime.Now, currentBook.BookID, "Active", currentBook.Status);
                return RedirectToAction("BorrowedIndex");
            }
            catch (Exception)
            {
                return RedirectToAction("BorrowedIndex");
            }
        }
    }
}
