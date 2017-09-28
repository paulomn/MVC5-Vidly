using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DTOs;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class RentalController : ApiController
    {
        private ApplicationDbContext _context;
        public RentalController()
        {
            _context = new Models.ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //POST /api/rental
        [HttpPost]
        public IHttpActionResult CreateRental(RentalDTO newRental)
        {
            //DEFENSIVE APROACH
            //if (newRental.MovieIds.Count == 0)
            //    return BadRequest("No movies selected.");

            var customer = _context.Customers.SingleOrDefault(c => c.Id == newRental.CustomerId);

            //DEFENSIVE APROACH
            //if (customer == null)
            //    return BadRequest("Invalid customer ID.");

            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            //DEFENSIVE APROACH
            //if (movies.Count != newRental.MovieIds.Count)
            //    return BadRequest("One or more movies are invalid.");

            foreach (Movie movie in movies)
            {
                if (movie.NumberAvailable == 0 )
                    return BadRequest("Movie is not available.");

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                movie.NumberAvailable--;
                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
