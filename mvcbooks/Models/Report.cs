using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace mvcbooks.Models
{
    public class ReportViewModel
    {
        [Required]
        [Display(Name = "Date From")]
        public DateTime BorrowedAndReternedBooksDateFrom { get; set; }
        [Display(Name = "Date To")]
        [Required]
        public DateTime BorrowedAndReternedBooksDateTo { get; set; }
        [Required]
        [Display(Name = "Date From")]
        public DateTime BorrowedBookBetweenDatesDateFrom { get; set; }

        [Display(Name = "Date To")]
        [Required]
        public DateTime BorrowedBookBetweenDatesDateTo { get; set; }
    }
}