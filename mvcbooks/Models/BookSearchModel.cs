using mvcbooks.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvcbooks.Models
{
    public class BookSearchModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; } 
        public List<Author> Authors { get; set; }
        [Display(Name = "Release Date")]
        public int? DateFrom { get; set; }
        [Display(Name = "Release Year")]
        public int? DateTo { get; set; }
        public string Genre { get; set; }
        public List<Genre> Genres { get; set; }
        public string Description { get; set; }
        public List<BookViewModel> Books { get; set; }
        
    }
}