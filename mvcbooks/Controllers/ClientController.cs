using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;
using mvcbooks.Data;
using mvcbooks.Models;
using Newtonsoft.Json;

namespace mvcbooks.Controllers
{

    /* TODO
     * 
     *   SOLVED: Admin Confirmation links NOT WORKING!!!!
     *   MAYBE?: Client Borrowing Book: Each Book to have its own borrowing dates 
     *   Client SendConfirmation - default values into web config or somewhere static? and bring it from there.
     *   Whole HTML generated with Tiny MCE to save into DB, and when sending a confirmation link, get that html from DB, input Values {Title} from db values and send it like that
     *   Jquery Calendar. After StartDate is selected, EndDate Calendar can't be assigned less value
     *   Home Table to sort by Title and Date (and other values too). If sorting by Author/Genre, from multiple values (for one Book) take only first author's first letter to compare row to others,
     *   Client to have its own View and for old clients, only P_ID textbox is editable. after P_ID is assigned, show all other client fields with labels. To Edit other values (including P_ID), popup window is used.
     *   Any Error to be logged in a log file. Log4Net.
     *   Instead of ALERT when in ADD BOOK / ADD CLIENT, Use Jquery and onclick return 
     * */



    public class ClientController : BaseController
    {
        private readonly LibraryEntities db = new LibraryEntities();

        //To Populate ViewModel Classes from DB. To be run from Index()
        public List<BookViewModel> GenerateAvailableBooks()
        {
            var db_books = db.Books.ToList();

            // Populate Books Table that includes ONLY Available Books in Library
            var booklist = new List<BookViewModel>(
                db_books.Select(x => new BookViewModel
                {
                    BookId = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    TotalQuantity = x.Total_Quantity,
                    AvailableQuantity = x.Total_Quantity - x.Borrowed_Quantity,
                    Description = x.Description,
                    Authors = new List<AuthorViewModel>(
                        x.Author_Book.Select(y => new AuthorViewModel
                        {
                            AuthorId = y.Author.Id,
                            Name = y.Author.Name
                        })),
                    Genres = new List<GenreViewModel>(
                        x.Book_Genre.Select(y => new GenreViewModel
                        {
                            GenreId = y.Genre.Id,
                            GenreName = y.Genre.Genre_Name
                        })),
                }).Where(x => x.AvailableQuantity != 0).OrderBy(x => x.Title)
            );
            return booklist;
        }


        //GET: Generate Client View To Borrow Book(s) with all available books dropdownlist populated
        //public ActionResult Index()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        List<BookViewModel> availableBookListModel = GenerateAvailableBooks();
        //        var clientViewModelM = new ClientViewModel()
        //        {
        //            AvailableBooks = availableBookListModel
        //        };
        //        return View("Index", clientViewModelM);
        //    }
        //}
        public ActionResult Index()
        {
            var clientViewModel = new ClientViewModel()
            {
                AvailableBooks = GenerateAvailableBooks()
            };
            return View(clientViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(ClientViewModel clientViewModel)
        {

            if (ModelState.IsValid)
            {
                // Generate a unique token key (Byte Data) depending on request time and a unique Guid key to later use for confirmation
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                clientViewModel.AvailableBooks = GenerateAvailableBooks();
                // If any of non-required fields of ClientViewModel wasn't filled in, their value should not be null or empty.
                //if (clientViewModel.StartDate == null)
                //{
                //    clientViewModel.StartDate = DateTime.Now;
                //    clientViewModel.EndDate = DateTime.Now.AddDays(28);
                //}
                //if( clientViewModel.EndDate == null)
                //{
                //    clientViewModel.EndDate = clientViewModel.StartDate.AddDays(28);
                //}
                if (String.IsNullOrEmpty(clientViewModel.PhoneNumber))
                {
                    clientViewModel.PhoneNumber = "";
                }
                if (String.IsNullOrEmpty(clientViewModel.Address))
                {
                    clientViewModel.Address = "";
                }
                // Assign this key to ClientViewModel field to later specify which client made a request
                clientViewModel.Token = token;
                // Use SQL Procedure to store client info into DB
                SqlProceduresToInputDataIntoDB(clientViewModel);
                // Encrypt the Token to send a confirmation link to admin
                clientViewModel.Token = (Encrypt(token)).Replace('+', '-').Replace('/', '_');
                // Send Confirmation Email to Admin
                SendConfirmationEmail(clientViewModel);

                ViewBag.Message = "Please Wait for Confirmation";
                return View("BorrowingConfirmation");
            }
            clientViewModel.AvailableBooks = GenerateAvailableBooks();
            return View(clientViewModel);
        }

        // Generate and Send Confirmation Email to Admin To Confirm or Decline Requested data from a client
        public void SendConfirmationEmail(ClientViewModel clientViewModel)
        {
            SmtpClient smtp = new SmtpClient();
            MailAddress to = new MailAddress("nodsaatec@gmail.com");
            MailAddress from = new MailAddress(clientViewModel.Email);//todo
            MailMessage mailMessage = new MailMessage(from, to)
            {
                Subject = "Client Confirmation"
            };
            var body = new StringBuilder();

            body.AppendLine("Please respond to the following request.<br/><br/>"
                + clientViewModel.FirstName + " " + clientViewModel.LastName + " (ID: " + clientViewModel.PersonalId
                + ") is requesting to borrow the book(s):");
            // Get Requested books from Database
            foreach (var eachBookId in clientViewModel.BookIds.Split(','))
            {
                var bookId = Convert.ToInt32(eachBookId);
                var book = db.Books.FirstOrDefault(x => x.Id == bookId);
                body.AppendLine("<br/>&emsp;Title: " + book.Title + " (Year " + book.Year + "), Available Quantity: " + (book.Total_Quantity - book.Borrowed_Quantity));
            }
            body.AppendLine("<br/><br/>Click here to Confirm<br/>");
            // Generate Accept and Decline hyperlinks for admin to response to confirmation request
            body.AppendFormat("<a href=\"{0}://{1}/Client/BorrowingConfirmation/{2}?accepted=true\">Accept</a>&emsp;", Request.Url.Scheme, Request.Url.Authority, clientViewModel.Token);
            body.AppendFormat("<a href=\"{0}://{1}/Client/BorrowingConfirmation/{2}?accepted=false\">Decline</a>", Request.Url.Scheme, Request.Url.Authority, clientViewModel.Token);
            mailMessage.IsBodyHtml = true;

            mailMessage.Body = body.ToString();

            smtp.Send(mailMessage);
        }

        // Call two procedures: One to write Client information in Client table and another to write which book(s) is this client requesting
        public void SqlProceduresToInputDataIntoDB(ClientViewModel clientViewModel)
        {
            //A reader to store the values
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
                SqlCommand sqlCommand = new SqlCommand("ClientRegistration", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Adding client parameters to the procedure, to make a new client row
                sqlCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@Personal_Id", clientViewModel.PersonalId),
                        new SqlParameter("@First_Name", clientViewModel.FirstName),
                        new SqlParameter("@Last_Name", clientViewModel.LastName),
                        new SqlParameter("@Email", clientViewModel.Email),
                        new SqlParameter("@Phone_Number", clientViewModel.PhoneNumber),
                        new SqlParameter("@Address", clientViewModel.Address),

                    }
                );

                //Open your connection
                conn.Open();

                //Execute your Stored Procedure
                sqlDataReader = sqlCommand.ExecuteReader();

                //Close the connection
                conn.Close();

                sqlCommand = new SqlCommand("ClientBorrowingBook", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                Client client = db.Clients.FirstOrDefault(x => x.Personal_Id == clientViewModel.PersonalId);

                // Insert Hidden values of (Id, BorrowFrom, BorrowTo) into multidimensional Array[][]
                var booksCount = clientViewModel.BookIds.Count(c => c == ',');
                string[,] datesArrayForEachBorrowingBook = new string[booksCount + 1, 3];
                int i = 0;
                foreach (var datesForEachBookArray in clientViewModel.BorrowingDates.Split(','))
                {
                    if (string.IsNullOrEmpty(datesForEachBookArray))
                        continue;
                    int j = 0;
                    foreach (var eachBookRow in datesForEachBookArray.Split('_'))
                    {
                        datesArrayForEachBorrowingBook[i, j] = eachBookRow;
                        j++;
                    }
                    i++;
                }

                //foreach (var eachBookId in clientViewModel.BookIds.Split(','))
                for (int j = 0; j <= booksCount; j++)
                {
                    int bookId = Convert.ToInt32(datesArrayForEachBorrowingBook[j, 0]);
                    // Clear or parameters from previous loop values
                    sqlCommand.Parameters.Clear();
                    // add new parameters to make a new row in Book-Client joined table. For each Book (to-be) reserved, there will be a new row
                    // and all these rows will have the same Token, since those were generated by one client at one particular time.
                    sqlCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@Client_Id", client.Id),
                        new SqlParameter("@Book_Id", bookId),
                        new SqlParameter("@Start_Date", datesArrayForEachBorrowingBook[j,1]),
                        new SqlParameter("@End_Date", datesArrayForEachBorrowingBook[j,2]),
                        new SqlParameter("@Token", clientViewModel.Token)
                    });

                    //Open your connection
                    conn.Open();
                    //Execute your Stored Procedure
                    sqlDataReader = sqlCommand.ExecuteReader();
                    //Close the connection
                    conn.Close();
                }


                //foreach (var eachBookId in clientViewModel.BookIds.Split(','))
                //{
                //    int bookId = Convert.ToInt32(eachBookId);
                //    // Clear or parameters from previous loop values
                //    sqlCommand.Parameters.Clear();
                //    // add new parameters to make a new row in Book-Client joined table. For each Book (to-be) reserved, there will be a new row
                //    // and all these rows will have the same Token, since those were generated by one client at one particular time.
                //    sqlCommand.Parameters.AddRange(new[]
                //    {
                //        new SqlParameter("@Client_Id", client.Id),
                //        new SqlParameter("@Book_Id", bookId),
                //        //new SqlParameter("@Start_Date", clientViewModel.StartDate),
                //        //new SqlParameter("@End_Date", clientViewModel.EndDate),
                //        new SqlParameter("@Token", clientViewModel.Token)
                //    });

                //    //Open your connection
                //    conn.Open();
                //    //Execute your Stored Procedure
                //    sqlDataReader = sqlCommand.ExecuteReader();
                //    //Close the connection
                //    conn.Close();
                //}
            }
        }

        // After admin accepts or declines the request, this method will run 
        public ActionResult BorrowingConfirmation(string id)
        {
            if (ModelState.IsValid)
            {
                // decrypt the id value, that is encrypted token
                var decryptedId = Decrypt(id.Replace('-', '+').Replace('_', '/'));
                string adminConfirmationResponse = Request.QueryString["accepted"];
                bool status = bool.Parse(adminConfirmationResponse);
                if (status == true)
                {
                    var borrowingToBeConfirmed = db.Borrowed_Book_By_Client.FirstOrDefault(x => x.Token == decryptedId);
                    // if time of generating token is more than 1 day, then the confirmation link is too old to get confirmed


                    byte[] data = Convert.FromBase64String(decryptedId);
                    DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                    if (when < DateTime.UtcNow.AddHours(-24))
                    {
                        ViewBag.Message = "Confirmation link is outdated!";
                    }
                    else
                    {   // if admin accepted the request, the borrow status will be true for every borrowed book and book quantity(ies) will change in the database
                        foreach (var item in db.Borrowed_Book_By_Client.Where(x => x.Token == decryptedId))
                        {
                            item.ConfirmationStatus = true;
                            Book book = db.Books.SingleOrDefault(x => x.Id == item.Book_Id);
                            book.Borrowed_Quantity++;
                        }
                        db.SaveChanges();

                        ViewBag.Message = "Confirmation is Successful";
                        return View();
                    }
                }
            }
            ViewBag.Message = "Request is declined successfully";
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        // Encrypt the Token received when Client Requested a new book. Encrypted text value will be send to admin for confirmation
        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            // Using AES standard Encryption algorithm to get a new key
            using (Aes encryptor = Aes.Create())
            {
                // Rfc2898DeriveBytes class creates two identical keys. It then encrypts and decrypts some data using the keys.
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                // Creates a stream whose backing store is memory
                using (MemoryStream ms = new MemoryStream())
                {
                    // Defines a stream that links data streams to cryptographic transformations
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        // Decrypt the Token after Admin confirms or declines the request for client. Decrypted Value will be used to find time when it was created.
        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        //public void SaveClient(ClientViewModel clientViewModel)
        //{
        //    Client client = new Client();
        //    if (clientViewModel.Client.Id == 0)
        //    {
        //        client.First_Name = clientViewModel.FirstName;
        //        client.Last_Name = clientViewModel.LastName;
        //        client.Personal_Id = clientViewModel.PersonalId;
        //        client.Email = clientViewModel.Email;
        //        client.Address = clientViewModel.Address;
        //        client.Phone_Number = clientViewModel.PhoneNumber;
        //        db.Clients.Add(client);
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        var clientInDb = db.Clients.SingleOrDefault(c => c.Id == clientViewModel.Client.Id);

        //        clientInDb.First_Name = clientViewModel.FirstName;
        //        clientInDb.Last_Name = clientViewModel.LastName;
        //        clientInDb.Personal_Id = clientViewModel.PersonalId;
        //        clientInDb.Email = clientViewModel.Email;
        //        clientInDb.Address = clientViewModel.Address;
        //        clientInDb.Phone_Number = clientViewModel.PhoneNumber;
        //        db.SaveChanges();
        //    }
        //}
    }
}
