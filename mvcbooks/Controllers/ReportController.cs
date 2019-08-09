using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using mvcbooks.Data;
using mvcbooks.Models;
using Rotativa;

namespace mvcbooks.Controllers
{
    public class ReportController : BaseController
    {
        #region DbContext
        private readonly LibraryEntities _context;
        public ReportController()
        {
            _context = new LibraryEntities();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion

        #region Borrowed Book For Now
        // GET: Report for BorrowedBookForNow
        public ActionResult BorrowedBookForNow(string sortBy)
        {
            //for sorting
            ViewBag.SortByTitleParameter = string.IsNullOrEmpty(sortBy) ? "Book desc" : "";
            ViewBag.SortByFirst_NameParameter = sortBy == "First Name" ? "First Name desc" : "First Name";
            ViewBag.SortByLast_NameParameter = sortBy == "Last Name" ? "Last Name desc" : "Last Name";
            ViewBag.SortByYearParameter = sortBy == "Year" ? "Year desc" : "Year";
            ViewBag.SortByLeft_QuantityParameter = sortBy == "Left Quantity" ? "Left Quantity desc" : "Left Quantity";

            var result = _context.BorrowedBookForNow().ToList();
            if (result == null)
                return HttpNotFound();
            //return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            // for sorting
            switch (sortBy)
            {
                case "Book desc":
                    result = _context.BorrowedBookForNow().OrderByDescending(b => b.Title).ToList();
                    break;
                case "First Name desc":
                    result = _context.BorrowedBookForNow().OrderByDescending(b => b.First_Name).ToList();
                    break;
                case "First Name":
                    result = _context.BorrowedBookForNow().OrderBy(b => b.First_Name).ToList();
                    break;
                case "Last Name desc":
                    result = _context.BorrowedBookForNow().OrderByDescending(b => b.Last_Name).ToList();
                    break;
                case "Last Name":
                    result = _context.BorrowedBookForNow().OrderBy(b => b.Last_Name).ToList();
                    break;
                case "Year desc":
                    result = _context.BorrowedBookForNow().OrderByDescending(b => b.Year).ToList();
                    break;
                case "Year":
                    result = _context.BorrowedBookForNow().OrderBy(b => b.Year).ToList();
                    break;
                case "Left Quantity desc":
                    result = _context.BorrowedBookForNow().OrderByDescending(b => b.Left_Quantity).ToList();
                    break;
                case "Left Quantity":
                    result = _context.BorrowedBookForNow().OrderBy(b => b.Left_Quantity).ToList();
                    break;
                default:
                    result = _context.BorrowedBookForNow().OrderBy(b => b.Title).ToList();
                    break;
            }
            return View(result);
        }

        // BorrowedBookForNow To Excel
        public ActionResult BorrowedBookForNowToExcel()
        {
            var result = _context.BorrowedBookForNow().ToList();
            if (result == null)
                return HttpNotFound();
            return ExportDataToExcel(result);
        }

        // BorrowedBookForNow To PDF View
        public ActionResult BorrowedBookForNowToPDFView()
        {
            var result = _context.BorrowedBookForNow().ToList();
            if (result == null)
                return HttpNotFound();
            return View(result);
        }

        // BorrowedBookForNow To PDF
        public ActionResult BorrowedBookForNowToPDF()
        {
            return new ActionAsPdf("BorrowedBookForNowToPDFView")
            {
                FileName = "Report.pdf"
            };
        }
        #endregion

        #region BorrowedAndReternedBooks

        // GET: Report for BorrowedAndReternedBooks
        public ActionResult BorrowedAndReternedBooks(ReportViewModel report, string borrowedAndReternedBooksDateFrom, string borrowedAndReternedBooksDateTo, string sortBy)
        {
            ViewBag.SortTitleParameter = sortBy == "Book" ? "Book_desc" : "Book";
            ViewBag.SortByFirst_NameParameter = sortBy == "First Name" ? "First Name desc" : "First Name";
            ViewBag.SortByLast_NameParameter = sortBy == "Last Name" ? "Last Name desc" : "Last Name";
            ViewBag.SortByYearParameter = sortBy == "Year" ? "Year desc" : "Year";
            ViewBag.SortByLeft_QuantityParameter = sortBy == "Left Quantity" ? "Left Quantity desc" : "Left Quantity";
            if (!ModelState.IsValid)
                return View("Report");
            var result = _context.BorrowedAndReternedBooks(report.BorrowedAndReternedBooksDateFrom, report.BorrowedAndReternedBooksDateTo).ToList();
            if (result == null)
                return HttpNotFound();
            switch (sortBy)
            {
                case "Book_desc":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderByDescending(b => b.Title).ToList();
                    break;
                case "Book":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderBy(b => b.Title).ToList();
                    break;
                case "First Name desc":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderByDescending(b => b.First_Name).ToList();
                    break;
                case "First Name":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderBy(b => b.First_Name).ToList();
                    break;
                case "Last Name desc":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderByDescending(b => b.Last_Name).ToList();
                    break;
                case "Last Name":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderBy(b => b.Last_Name).ToList();
                    break;
                case "Year desc":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderByDescending(b => b.Year).ToList();
                    break;
                case "Year":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderBy(b => b.Year).ToList();
                    break;
                case "Left Quantity desc":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderByDescending(b => b.Left_Quantity).ToList();
                    break;
                case "Left Quantity":
                    result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).OrderBy(b => b.Left_Quantity).ToList();
                    break;
            }
            return View(result);
        }

        //BorrowedAndReternedBooks to excel
        public ActionResult BorrowedAndReternedBooksToExcel(string borrowedAndReternedBooksDateFrom, string borrowedAndReternedBooksDateTo)
        {
            var result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).ToList();
            if (result == null)
                return HttpNotFound();
            return ExportDataToExcel(result);
        }

        public ActionResult BorrowedAndReternedBooksToPdfView(string borrowedAndReternedBooksDateFrom, string borrowedAndReternedBooksDateTo)
        {
            var result = _context.BorrowedAndReternedBooks(Convert.ToDateTime(borrowedAndReternedBooksDateFrom), Convert.ToDateTime(borrowedAndReternedBooksDateTo)).ToList();
            if (result == null)
                return HttpNotFound();
            return View(result);
        }

        // BorrowedBookBetweenDates To PDF
        public ActionResult BorrowedAndReternedBooksToPDF(string borrowedAndReternedBooksDateFrom, string borrowedAndReternedBooksDateTo)
        {
            return new ActionAsPdf("BorrowedAndReternedBooksToPdfView", new { borrowedAndReternedBooksDateFrom, borrowedAndReternedBooksDateTo })
            {
                FileName = "Report.pdf"
            };
        }

        #endregion

        #region BorrowedBookBetweenDates

        // GET: Report for BorrowedBookBetweenDates
        public ActionResult BorrowedBookBetweenDates(ReportViewModel report, string sortBy, string BorrowedBookBetweenDatesDateFrom, string BorrowedBookBetweenDatesDateTo)
        {
            ViewBag.SortTitleParameter = sortBy == "Book" ? "Book_desc" : "Book";
            ViewBag.SortByFirst_NameParameter = sortBy == "First Name" ? "First Name desc" : "First Name";
            ViewBag.SortByLast_NameParameter = sortBy == "Last Name" ? "Last Name desc" : "Last Name";
            ViewBag.SortByYearParameter = sortBy == "Year" ? "Year desc" : "Year";
            ViewBag.SortByLeft_QuantityParameter = sortBy == "Left Quantity" ? "Left Quantity desc" : "Left Quantity";
            if (!ModelState.IsValid)
                return View("Report");
            var result = _context.BorrowedBookBetweenDates(report.BorrowedBookBetweenDatesDateFrom, report.BorrowedBookBetweenDatesDateTo).ToList();
            if (result == null)
                return HttpNotFound();
            //Session["BorrowedBookBetweenDatesTmp"] = result;
            //Session["BorrowedBookBetweenDatesTmpForPdf"] = result;
            switch (sortBy)
            {
                case "Book_desc":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderByDescending(b => b.Title).ToList();
                    break;
                case "Book":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderBy(b => b.Title).ToList();
                    break;
                case "First Name desc":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderByDescending(b => b.First_Name).ToList();
                    break;
                case "First Name":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderBy(b => b.First_Name).ToList();
                    break;
                case "Last Name desc":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderByDescending(b => b.Last_Name).ToList();
                    break;
                case "Last Name":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderBy(b => b.Last_Name).ToList();
                    break;
                case "Year desc":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderByDescending(b => b.Year).ToList();
                    break;
                case "Year":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderBy(b => b.Year).ToList();
                    break;
                case "Left Quantity desc":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderByDescending(b => b.Left_Quantity).ToList();
                    break;
                case "Left Quantity":
                    result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).OrderBy(b => b.Left_Quantity).ToList();
                    break;
            }
            return View(result);
        }

        //BorrowedBookBetweenDates to excel
        public ActionResult BorrowedBookBetweenDatesToExcel(string BorrowedBookBetweenDatesDateFrom, string BorrowedBookBetweenDatesDateTo)
        {
            var result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).ToList();
            if (result == null)
                return HttpNotFound();
            return ExportDataToExcel(result);
        }

        // BorrowedBookBetweenDates To PDF View
        public ActionResult BorrowedBookBetweenDatesToPDFView(string BorrowedBookBetweenDatesDateFrom, string BorrowedBookBetweenDatesDateTo)
        {
            var result = _context.BorrowedBookBetweenDates(Convert.ToDateTime(BorrowedBookBetweenDatesDateFrom), Convert.ToDateTime(BorrowedBookBetweenDatesDateTo)).ToList();
            if (result == null)
                return HttpNotFound();
            return View(result);
        }

        // BorrowedBookBetweenDates To PDF
        public ActionResult BorrowedBookBetweenDatesToPDF(string BorrowedBookBetweenDatesDateFrom, string BorrowedBookBetweenDatesDateTo)
        {
            return new ActionAsPdf("BorrowedBookBetweenDatesToPDFView", new {BorrowedBookBetweenDatesDateFrom, BorrowedBookBetweenDatesDateTo })
            {
                FileName = "Report.pdf"
            };
        }
        #endregion

        #region Generic Methods
        public ActionResult Report()
        {
            ReportViewModel report = new ReportViewModel();
            return View(report);
        }
        public ActionResult ExportDataToExcel<T>(List<T> report)
        {
            var datasource = report;

            GridView gv = new GridView
            {
                DataSource = datasource
            };
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Report.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return Json("Success");
        }
        #endregion
    }
}