using System;
using System.Collections.Generic;
//Add to change Entity Framework defaults
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public DateTime? DateAdded { get; set; }
        [Required]
        [Range(1, 20)]
        public int NumberInStock { get; set; }

        public int NumberAvailable { get; set; }
    }
}