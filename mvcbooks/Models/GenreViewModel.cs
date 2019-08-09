using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvcbooks.Models
{
    public class GenreViewModel

    {
        public int GenreId { get; set; }
        [Display(Name = "Genre")]
        public string GenreName { get; set; }
    }
}