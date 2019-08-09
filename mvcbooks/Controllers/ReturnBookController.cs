using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvcbooks.Data;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace mvcbooks.Controllers
{
    public class ReturnBookController : BaseController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ReturnBookController));
        private readonly LibraryEntities _context;
        public ReturnBookController()
        {
            _context = new LibraryEntities();
        }
        // GET: ReturnBook
        public ActionResult ReturnBookList(string sortBy)
        {
            ViewBag.SortByTitleParameter = sortBy == "Book" ? "Book desc" : "Book";
            ViewBag.SortByFirst_NameParameter = sortBy == "First Name" ? "First Name desc" : "First Name";
            ViewBag.SortByLast_NameParameter = sortBy == "Last Name" ? "Last Name desc" : "Last Name";
            ViewBag.SortByYearParameter = sortBy == "Year" ? "Year desc" : "Year";
            ViewBag.SortByLeft_QuantityParameter = sortBy == "Left Quantity" ? "Left Quantity desc" : "Left Quantity";
            var books = _context.BorowedBookList().ToList();
            if (books == null)
                return HttpNotFound();
            switch (sortBy)
            {
                case "Book desc":
                    books = _context.BorowedBookList().OrderByDescending(b => b.Title).ToList();
                    break;
                case "Book":
                    books = _context.BorowedBookList().OrderBy(b => b.Title).ToList();
                    break;
                case "First Name desc":
                    books = _context.BorowedBookList().OrderByDescending(b => b.First_Name).ToList();
                    break;
                case "First Name":
                    books = _context.BorowedBookList().OrderBy(b => b.First_Name).ToList();
                    break;
                case "Last Name desc":
                    books = _context.BorowedBookList().OrderByDescending(b => b.Last_Name).ToList();
                    break;
                case "Last Name":
                    books = _context.BorowedBookList().OrderBy(b => b.Last_Name).ToList();
                    break;
                case "Year desc":
                    books = _context.BorowedBookList().OrderByDescending(b => b.Year).ToList();
                    break;
                case "Year":
                    books = _context.BorowedBookList().OrderBy(b => b.Year).ToList();
                    break;
                case "Left Quantity desc":
                    books = _context.BorowedBookList().OrderByDescending(b => b.Left_Quantity).ToList();
                    break;
                case "Left Quantity":
                    books = _context.BorowedBookList().OrderBy(b => b.Left_Quantity).ToList();
                    break;
            }
            return View(books);
        }

        public ActionResult ReturnBook(int id, int clientId, DateTime? returnDate)
        {
            if (returnDate == null)
            {
                ModelState.AddModelError("returnDate", "Return Date is Required!");
                return View("ReturnBookList", _context.BorowedBookList().ToList());
            }
            _context.ReturnBook(id, clientId, returnDate);
            _context.SaveChanges();
            return RedirectToAction("ReturnBookList");
        }
    }
}