﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mvcbooks.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LibraryEntities : DbContext
    {
        public LibraryEntities()
            : base("name=LibraryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
                                                                                                                                                                                                      
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Author_Book> Author_Book { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Book_Genre> Book_Genre { get; set; }
        public virtual DbSet<Borrowed_Book_By_Client> Borrowed_Book_By_Client { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
    
        public virtual ObjectResult<BorowedBookList_Result> BorowedBookList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BorowedBookList_Result>("BorowedBookList");
        }
    
        public virtual ObjectResult<BorrowedAndReternedBooks_Result> BorrowedAndReternedBooks(Nullable<System.DateTime> dateFrom, Nullable<System.DateTime> dateTo)
        {
            var dateFromParameter = dateFrom.HasValue ?
                new ObjectParameter("dateFrom", dateFrom) :
                new ObjectParameter("dateFrom", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("dateTo", dateTo) :
                new ObjectParameter("dateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BorrowedAndReternedBooks_Result>("BorrowedAndReternedBooks", dateFromParameter, dateToParameter);
        }
    
        public virtual ObjectResult<BorrowedBookBetweenDates_Result> BorrowedBookBetweenDates(Nullable<System.DateTime> dateFrom, Nullable<System.DateTime> dateTo)
        {
            var dateFromParameter = dateFrom.HasValue ?
                new ObjectParameter("DateFrom", dateFrom) :
                new ObjectParameter("DateFrom", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("dateTo", dateTo) :
                new ObjectParameter("dateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BorrowedBookBetweenDates_Result>("BorrowedBookBetweenDates", dateFromParameter, dateToParameter);
        }
    
        public virtual ObjectResult<BorrowedBookForNow_Result> BorrowedBookForNow()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BorrowedBookForNow_Result>("BorrowedBookForNow");
        }
    
        public virtual int ClientBorrowingBook(Nullable<int> client_Id, Nullable<int> book_Id, Nullable<System.DateTime> start_Date, Nullable<System.DateTime> end_Date, string token)
        {
            var client_IdParameter = client_Id.HasValue ?
                new ObjectParameter("Client_Id", client_Id) :
                new ObjectParameter("Client_Id", typeof(int));
    
            var book_IdParameter = book_Id.HasValue ?
                new ObjectParameter("Book_Id", book_Id) :
                new ObjectParameter("Book_Id", typeof(int));
    
            var start_DateParameter = start_Date.HasValue ?
                new ObjectParameter("Start_Date", start_Date) :
                new ObjectParameter("Start_Date", typeof(System.DateTime));
    
            var end_DateParameter = end_Date.HasValue ?
                new ObjectParameter("End_Date", end_Date) :
                new ObjectParameter("End_Date", typeof(System.DateTime));
    
            var tokenParameter = token != null ?
                new ObjectParameter("Token", token) :
                new ObjectParameter("Token", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClientBorrowingBook", client_IdParameter, book_IdParameter, start_DateParameter, end_DateParameter, tokenParameter);
        }
    
        public virtual int ClientRegistration(string personal_Id, string first_Name, string last_Name, string email, string phone_Number, string address)
        {
            var personal_IdParameter = personal_Id != null ?
                new ObjectParameter("Personal_Id", personal_Id) :
                new ObjectParameter("Personal_Id", typeof(string));
    
            var first_NameParameter = first_Name != null ?
                new ObjectParameter("First_Name", first_Name) :
                new ObjectParameter("First_Name", typeof(string));
    
            var last_NameParameter = last_Name != null ?
                new ObjectParameter("Last_Name", last_Name) :
                new ObjectParameter("Last_Name", typeof(string));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var phone_NumberParameter = phone_Number != null ?
                new ObjectParameter("Phone_Number", phone_Number) :
                new ObjectParameter("Phone_Number", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ClientRegistration", personal_IdParameter, first_NameParameter, last_NameParameter, emailParameter, phone_NumberParameter, addressParameter);
        }
    
        public virtual int ReturnBook(Nullable<int> borrowed_Book_By_Client_BookId, Nullable<int> borrowed_Book_By_Client_ClientId, Nullable<System.DateTime> updateReternDAte)
        {
            var borrowed_Book_By_Client_BookIdParameter = borrowed_Book_By_Client_BookId.HasValue ?
                new ObjectParameter("Borrowed_Book_By_Client_BookId", borrowed_Book_By_Client_BookId) :
                new ObjectParameter("Borrowed_Book_By_Client_BookId", typeof(int));
    
            var borrowed_Book_By_Client_ClientIdParameter = borrowed_Book_By_Client_ClientId.HasValue ?
                new ObjectParameter("Borrowed_Book_By_Client_ClientId", borrowed_Book_By_Client_ClientId) :
                new ObjectParameter("Borrowed_Book_By_Client_ClientId", typeof(int));
    
            var updateReternDAteParameter = updateReternDAte.HasValue ?
                new ObjectParameter("UpdateReternDAte", updateReternDAte) :
                new ObjectParameter("UpdateReternDAte", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReturnBook", borrowed_Book_By_Client_BookIdParameter, borrowed_Book_By_Client_ClientIdParameter, updateReternDAteParameter);
        }
    
        public virtual int UpdateBorrowedBookQuantity(Nullable<int> book_Id)
        {
            var book_IdParameter = book_Id.HasValue ?
                new ObjectParameter("Book_Id", book_Id) :
                new ObjectParameter("Book_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateBorrowedBookQuantity", book_IdParameter);
        }
    }
}