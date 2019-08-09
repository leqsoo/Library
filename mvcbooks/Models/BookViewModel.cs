using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvcbooks.Models
{
    public class BookViewModel
    {

        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
        public List<GenreViewModel> Genres { get; set; }
        [Required]
        public int Year { get; set; }
        [StringLength(100, ErrorMessage = "Description is too long")]
        public string Description { get; set; }
        public string AuthorIds { get; set; }
        public string DeletedAuthorIds { get; set; }
        public string GenreIds { get; set; }
        public string DeletedGenreIds { get; set; }
        [Display(Name = "Quantity")]
        [Range(0, 1000, ErrorMessage = "Please enter a correct quantity")]
        public int TotalQuantity { get; set; }
        [Display(Name = "Available")]
        public int AvailableQuantity { get; set; }
        public List<AuthorViewModel> FullAuthorsList { get; set; }
        public List<GenreViewModel> FullGenresList { get; set; }
        public int? Availability { get; set; }

    }
}