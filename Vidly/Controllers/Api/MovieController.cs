using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DTOs;
using Vidly.Models;
using AutoMapper;


namespace Vidly.Controllers.Api
{
    public class MovieController : ApiController
    {
        private ApplicationDbContext _context;
        public MovieController()
        {
            _context = new Models.ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //GET /api/movie
        [HttpGet]
        public IEnumerable<MovieDTO> GetMovies()
        {
            return _context.Movies.ToList().Select(Mapper.Map<Movie, MovieDTO>);
        }

        //GET /api/movie/1
        [HttpGet]
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();

            //Using IHttpActionResult instead CustomerDTO
            //return Mapper.Map<Customer, CustomerDTO>(customer);
            return Ok(Mapper.Map<Movie, MovieDTO>(movie));
        }

        //POST /api/movie
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDTO movieDTO)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest();

            var movie = Mapper.Map<MovieDTO, Movie>(movieDTO);
            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDTO.Id = movie.Id;

            //return customerDTO;
            return Created(new Uri(Request.RequestUri + "/" + movieDTO.Id), movieDTO);
        }

        //PUT /api/movie/1
        [HttpPut]
        public IHttpActionResult UpdateMovie(int id, CustomerDTO movieDTO)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                //Using IHttpActionResult
                return BadRequest();

            var movieInDB = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movieInDB == null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                //Using IHttpActionResult
                return NotFound();

            Mapper.Map(movieDTO, movieInDB);

            _context.SaveChanges();

            return Ok();
        }

        //DELETE /api/movie/1
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            var movieInDB = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movieInDB == null)
                return NotFound();

            _context.Movies.Remove(movieInDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
