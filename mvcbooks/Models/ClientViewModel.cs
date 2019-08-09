using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mvcbooks.Data;

namespace mvcbooks.Models
{
    public class ClientViewModel
    {
        public int Id { get; set; }

        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Invalid Personal Number")]
        [Required]
        [Display(Name = "ID Number")]
        public string PersonalId { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        [Display(Name = "Available Books")]
        public List<BookViewModel> AvailableBooks { get; set; }

        public string BookIds { get; set; }

        public string Token { get; set; }

        public string BorrowingDates { get; set; }

        public Client Client { get; set; }
        //[Display(Name = "Borrowing From")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime StartDate { get; set; }
        //[Display(Name = "Borrowing To")]
        //[DataType(DataType.Date)]
        //public DateTime? EndDate { get; set; }
    }
}