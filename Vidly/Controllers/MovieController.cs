using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//To include method
using System.Data.Entity;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;


namespace Vidly.Controllers
{
    public class MovieController : Controller
    {
        //Add to give access to EF DBContext
        private ApplicationDbContext _context;

        public MovieController()
        {
            _context = new Models.ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movie/Random
        public ActionResult Random()
        {
            //var movie = new Movie() { Name = "Shrek!" };

            //Different returns: ActionResult is generic and must cast
            //return View(movie);
            //return new ViewResult();
            //return Content("Hello World");
            //return HttpNotFound();
            //return new EmptyResult();
            //return RedirectToAction("Index", "Home", new { page = 1, sortby = "name" });

            //TO PASS DATA BACK TO VIEW (movie)
            //First method: viewdata
            //ViewData["Movie"] = movie;

            //Second method: viewbag
            //ViewBag.Movie = movie;

            //Third and best method: return
            //Where is the object:
            //var viewResult = new ViewResult();
            //viewResult.ViewData.Model <-- here is the move object
            //return View(movie);

            //WHAT IF I HAVE MORE THAN ONE CLASS?
            //How to use ViewModel
            //Create a folder ViewModel, create the necessary atributes and lists

            var movie = new Movie() { Name = "Shrek!" };

            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" },
                new Customer { Name = "Customer 3" },
                new Customer { Name = "Customer 4" },
                new Customer { Name = "Customer 5" }
            };

            var viewModel = new RandomMovieViewModel();
            viewModel.Movie = movie;
            viewModel.Customers = customers;

            return View(viewModel);
        }

        [Route("movie/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
        //ASP.NET MVC Attribute Route Constraints
        //Other contraints: min, max, minlength, maxlength, int, float, guid
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(String.Format("year={0}&month={1}", year, month));
        }

        public ActionResult Index()
        {
            //Checking user in the role
            //if (User.IsInRole("CanManageMovies")

            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View("ReadOnlyList", movies);
        }

        //You could separate multiple roles with comma
        //[Authorize(Roles = "CanManageMovies")]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres,
                Movie = new Movie()
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            MovieFormViewModel viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres = _context.Genres.ToList()
            };

            //Specify the view name, using the same form for to actions: Edit and New
            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel
                {
                    Movie = movie,
                    Genres = _context.Genres
                };

                return View("MovieForm", viewModel);
            }


            if (movie.Id == 0)
            {
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDB = _context.Movies.Single(c => c.Id == movie.Id);

                //1) Microsoft MVC Official method
                //TryUpdateModel(customerInDB);

                //2) Mapper.Map
                //Data Transfer Object (later)

                movieInDB.Name = movie.Name;
                movieInDB.ReleaseDate = movie.ReleaseDate;
                movieInDB.GenreId = movie.GenreId;
                movieInDB.NumberInStock = movie.NumberInStock;
            }

            
            //How to deal with validation failed on save changes
            //try
            //{
                _context.SaveChanges();
            //}
            //catch (System.Data.Entity.Validation.DbEntityValidationException e)
            //{
            //    Console.WriteLine(e);
            //}

            return RedirectToAction("Index", "Movie");
        }
    }
}