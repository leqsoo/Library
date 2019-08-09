using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using mvcbooks;
using mvcbooks.Data;
using mvcbooks.Models;

namespace mvcbooks.Controllers
{
    public class BooksController : BaseController
    {
        private readonly LibraryEntities db = new LibraryEntities();

        public static List<BookViewModel> myBookList;

        //To Populate ViewModel Classes from DB. To be run from Index()
        public List<BookViewModel> GenerateBookList()
        {          
            var db_books = db.Books.ToList();

            var db_bookgenre = db.Book_Genre.ToList();

            // Create new BookViewModel List to show on the main page. Inserts Data in Books, Authors and Genres from DataBase
            var booklist = new List<BookViewModel>(
                db_books.Select(x => new BookViewModel
                {
                    BookId = x.Id,
                    Title = x.Title.ToLower(),
                    Year = x.Year,
                    TotalQuantity = x.Total_Quantity,
                    AvailableQuantity = x.Total_Quantity - x.Borrowed_Quantity,
                    Description = x.Description != null ? x.Description.ToLower() : "",
                    Authors = new List<AuthorViewModel>(
                        x.Author_Book.Select(y => new AuthorViewModel
                        {
                            AuthorId = y.Author.Id,
                            Name = y.Author.Name
                        })),
                    Genres = new List<GenreViewModel>(
                        db_bookgenre.Where(y => y.Book_Id == x.Id)
                                    .Select(z => new GenreViewModel
                                    {
                                        GenreId = z.Genre.Id,
                                        GenreName = z.Genre.Genre_Name
                                    }))
                }).OrderByDescending(x => x.Year)  // Sort populated data by Date in Descending order
            );
            return booklist;
        }
        


        // GET: Books
        public ActionResult Index()
        {
            string searchedTitle = Request.QueryString["Title"];
            string searchedAuthor = Request.QueryString["Author"];
            string searchedDateFrom = Request.QueryString["DateFrom"];
            string searchedDateTo = Request.QueryString["DateTo"];
            string searchedGenre = Request.QueryString["Genre"];
            string searchedDescription = Request.QueryString["Description"];

            // Filter the received Author from QueryString. If it is a starting point and/or nothing is selected, we pass empty value
            if (String.IsNullOrEmpty(searchedAuthor) || searchedAuthor.Contains("Selected"))
            {
                searchedAuthor = "";
            } 
            // if by hidden field multiple values are accumulated (because we may have or may not have clicked on any option) we cut only last value from it
            else if(searchedAuthor.Contains(','))
            {
                searchedAuthor = searchedAuthor.Substring(searchedAuthor.LastIndexOf(',') + 1);
            }
            // Same goes to Genre Dropdown
            if (String.IsNullOrEmpty(searchedGenre) || searchedGenre.Contains("Selected"))
            {
                searchedGenre = "";
            }
            else if (searchedGenre.Contains(','))
            {
                searchedGenre = searchedGenre.Substring(searchedGenre.LastIndexOf(',') + 1);
            }

            
            // Populate Books from Database
            myBookList = new List<BookViewModel>();
            foreach(var item in db.Books)
            {
                UpdateBorrowedBookQuantity(item.Id);
            }
            myBookList = GenerateBookList();

            // Filter Authors and Genres with searched Data
            var authors = myBookList.SelectMany(x => x.Authors).Where(x => x.Name.Contains(searchedAuthor));
            var genres = myBookList.SelectMany(x => x.Genres).Where(x => x.GenreName.Contains(searchedGenre));

            // Filter myBookList (BookViewModel Object) based on input data from Search boxes
            myBookList = myBookList.Where(x => 
                (!string.IsNullOrWhiteSpace(searchedTitle) ? x.Title.Contains(searchedTitle.ToLower()) : x.Title.Contains(""))
                && (!string.IsNullOrWhiteSpace(searchedAuthor) ? x.Authors.Intersect(authors).Any() : x.Authors.Any())
                && (!string.IsNullOrWhiteSpace(searchedDateFrom) ? x.Year >= int.Parse(searchedDateFrom) : x.Year >= db.Books.Min(y => y.Year))
                && (!string.IsNullOrWhiteSpace(searchedDateTo) ? x.Year <= int.Parse(searchedDateTo) : x.Year <= db.Books.Max(y => y.Year))
                && (!string.IsNullOrWhiteSpace(searchedGenre) ? x.Genres.Intersect(genres).Any() : x.Genres.Any())
                && (!string.IsNullOrWhiteSpace(searchedDescription) ? x.Description.Contains(searchedDescription.ToLower()) : x.Description.Contains("") )               
                ).ToList();

            // Check how many per cent of Total Quantity is actually available for each book and assign colours to rows respectively
            foreach (var item in myBookList)
            {  
                var availability = item.TotalQuantity > 0 ? ((Convert.ToDouble(item.AvailableQuantity) / Convert.ToDouble(item.TotalQuantity))) : 0;
                if (availability == 0)
                { // Those Books of which none is available or none is in the Library at all
                    item.Availability = 0;
                }
                else if(availability <= 0.25)
                { // Those books of which maximum 25% is available to Borrow
                    item.Availability = 1;
                }
                else if(availability <= 0.75)
                { // Those books of which more than 25% but equal or less than 75% is available to Borrow
                    item.Availability = 2;
                }
                else
                { // Those books of which more than 75% is available to Borrow
                    item.Availability = 3;
                }
            }


            // Generate BookSearchModel object to contain recently searched data and those Books that correspond the Search results
            var myBookSearchModel = new BookSearchModel()
            {
                Id = 1,
                Title = !string.IsNullOrWhiteSpace(searchedTitle) ? searchedTitle : "",
                Authors = db.Authors.OrderBy(x => x.Name).ToList(),
                Author = !string.IsNullOrWhiteSpace(searchedAuthor) ? searchedAuthor : "",
                DateFrom = !string.IsNullOrWhiteSpace(searchedDateFrom) ? int.Parse(searchedDateFrom) : default(int?),
                DateTo = !string.IsNullOrWhiteSpace(searchedDateTo) ? int.Parse(searchedDateTo) : default(int?),
                Genres = db.Genres.OrderBy(x => x.Genre_Name).ToList(),
                Genre = !string.IsNullOrWhiteSpace(searchedGenre) ? searchedGenre : "",
                Description = !string.IsNullOrWhiteSpace(searchedDescription) ? searchedDescription : "",
                Books = myBookList
            };
            return View(myBookSearchModel);
        }
        

        // GET: Books/Create
        // Prepare Create View by Populating Authors and Genres List for Dropdown Menu
        public ActionResult Create()
        {
            BookViewModel model = new BookViewModel();
            
            // Fill AuthorViewModel List by Authors from DataBase
            model.Authors = new List<AuthorViewModel>(
                db.Authors.Select(y => new AuthorViewModel
                {
                    AuthorId = y.Id,
                    Name = y.Name
                }).OrderBy(a => a.Name));

            // Fill GenreViewModel List by Genres from DataBase
            model.Genres = new List<GenreViewModel>(
                db.Genres.Select(x => new GenreViewModel
                {
                    GenreId = x.Id,
                    GenreName = x.Genre_Name
                }).OrderBy(a => a.GenreName));

            return View(model);
        }

        
        // POST: Books/Create
        // Posting new Data to Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                // Add Book Date To Database
                db.Books.Add(new Book
                {
                    Id = book.BookId,
                    Title = book.Title,
                    Year = book.Year,
                    Total_Quantity = book.TotalQuantity,
                    Borrowed_Quantity = 0,
                    Description = String.IsNullOrEmpty(book.Description) ? "" : book.Description
                });
                

                // Add values to joined table of Book and Author
                foreach (var eachAuthorId in book.AuthorIds.Split(','))
                {

                    db.Author_Book.Add(new Author_Book
                    {
                        Book_Id = book.BookId,
                        Author_Id = Convert.ToInt32(eachAuthorId)
                    });
                }

                // Add values to joined table of Genre and Book
                foreach (var eachGenreId in book.GenreIds.Split(','))
                {
                    db.Book_Genre.Add(new Book_Genre
                    {
                        Book_Id = book.BookId,
                        Genre_Id = Convert.ToInt32(eachGenreId)
                    });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BookViewModel book = myBookList.Find(x => x.BookId == id);
            if (book == null)
            {
                return HttpNotFound();
            }

            // Populate the value of Book Authors by its IDs to pass to Hidden field
            foreach (var item in book.Authors)
            {
                 book.AuthorIds += item.AuthorId.ToString() + ',';
            }
            book.AuthorIds = book.AuthorIds.Substring(0, book.AuthorIds.Length - 1);
            // Copy IDs to save its value to check the difference /edited value/ later
            book.DeletedAuthorIds = book.AuthorIds;

            foreach (var item in book.Genres)
            {
                book.GenreIds += item.GenreId.ToString() + ',';
            }
            book.GenreIds = book.GenreIds.Substring(0, book.GenreIds.Length - 1);
            book.DeletedGenreIds = book.GenreIds;


            book.FullAuthorsList = new List<AuthorViewModel>(
                db.Authors.Select(y => new AuthorViewModel
                {
                    AuthorId = y.Id,
                    Name = y.Name
                }).OrderBy(a => a.Name));

            book.FullGenresList = new List<GenreViewModel>(
                db.Genres.Select(x => new GenreViewModel
                {
                    GenreId = x.Id,
                    GenreName = x.Genre_Name
                }).OrderBy(a => a.GenreName));
            
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                Book mybook = db.Books.FirstOrDefault(x => x.Id == book.BookId);

                mybook.Title = book.Title;
                mybook.Year = book.Year;
                mybook.Total_Quantity = book.TotalQuantity;
                mybook.Description = String.IsNullOrEmpty(book.Description) ? "" : book.Description;

                // Split the IDs string returned from Hidden field. Check the difference with original data
                // and find what Authors were deleted or added to current Book sample.
                var deletedAuthorIds = book.DeletedAuthorIds.Split(',').ToList();
                foreach (var eachAuthorId in book.AuthorIds.Split(','))
                {
                    int thisAuthorId = Convert.ToInt32(eachAuthorId);

                    if(!db.Author_Book.Any(x => x.Book_Id == book.BookId && x.Author_Id == thisAuthorId))
                    {
                        db.Author_Book.Add(new Author_Book
                        {
                            Book_Id = book.BookId,
                            Author_Id = thisAuthorId
                        });
                    }
                    deletedAuthorIds.Remove(eachAuthorId);
                }
                foreach(var item in deletedAuthorIds)
                {
                    int thisAuthorId = Convert.ToInt32(item);
                    var authorsToBeDeleted = db.Author_Book.FirstOrDefault(x => x.Author_Id == thisAuthorId && x.Book_Id == book.BookId);
                    db.Author_Book.Remove(authorsToBeDeleted);
                }

                var deletedGenreIds = book.DeletedGenreIds.Split(',').ToList();
                foreach (var eachGenreId in book.GenreIds.Split(','))
                {
                    int thisGenreId = Convert.ToInt32(eachGenreId); 

                    if(!db.Book_Genre.Any(x => x.Book_Id == book.BookId && x.Genre_Id == thisGenreId))
                    {
                        db.Book_Genre.Add(new Book_Genre
                        {
                            Book_Id = book.BookId,
                            Genre_Id = thisGenreId
                        });
                    }
                    deletedGenreIds.Remove(eachGenreId);
                }
                foreach (var item in deletedGenreIds)
                {
                    int thisGenreId = Convert.ToInt32(item);
                    var genresToBeDeleted = db.Book_Genre.FirstOrDefault(x => x.Genre_Id == thisGenreId && x.Book_Id == book.BookId);
                    db.Book_Genre.Remove(genresToBeDeleted);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            return View(book);
        }

        // Delete Book item
        public ActionResult Delete(int id)
        {
            Book book = db.Books.Find(id);

            // Search and Delete from DB every item in Book-Genre connector table that contains this book
            foreach (var thisBook in db.Book_Genre.Where(x => x.Book_Id == id))
            {
                db.Book_Genre.Remove(thisBook);
            }

            // Search and Delete from DB every item in Book-Author connector table that contains this book
            foreach (var thisBook in db.Author_Book.Where(x => x.Book_Id == id))
            {
                db.Author_Book.Remove(thisBook);
            }

            // Delete this book item from Database
            db.Books.Remove(book);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Check (and update if wrong) that Borrowed Quantity value in DB is same as in total borrowed books for the time that have not been returned
        public void UpdateBorrowedBookQuantity(int bookId)
        {
            SqlDataReader sqlDataReader;

            //Setting SQL Connection
            using (SqlConnection conn = new SqlConnection(
                new SqlConnectionStringBuilder()
                {
                    DataSource = "192.168.10.15", // "ServerName"
                    InitialCatalog = "Library", // "DatabaseName"
                    UserID = "Test", // "UserName"
                    Password = "TE_5832n" // "UserPassword"
                }.ConnectionString
                ))
            {
                //Create a command to execute your Stored Procedure
                SqlCommand sqlCommand = new SqlCommand("UpdateBorrowedBookQuantity", conn);

                sqlCommand.CommandType = CommandType.StoredProcedure;
                
                //Add any necessary parameters to your Procedure
                sqlCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@Book_Id", bookId)
                });

                //Open your connection
                conn.Open();

                //Execute your Stored Procedure
                sqlDataReader = sqlCommand.ExecuteReader();

                //Close the connection
                conn.Close();
                
            }
        }
    }
}
