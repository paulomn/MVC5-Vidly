using System;
using System.Collections.Generic;
//To implement eager load (membership types)
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DTOs;
using Vidly.Models;
using AutoMapper;

namespace Vidly.Controllers.Api
{
    public class CustomerController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new Models.ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //GET /api/customer
        [HttpGet]
        public IHttpActionResult GetCustomers()
        {
            var customerDTO = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDTO>);

            return Ok(customerDTO);
        }

        //GET /api/customer/1
        [HttpGet]
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                return NotFound();

            //Using IHttpActionResult instead CustomerDTO
            //return Mapper.Map<Customer, CustomerDTO>(customer);
            return Ok(Mapper.Map<Customer, CustomerDTO>(customer));
        }

        //POST /api/customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDTO customerDTO)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                return BadRequest();

            var customer = Mapper.Map<CustomerDTO, Customer>(customerDTO);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDTO.Id = customer.Id;

            //return customerDTO;
            return Created(new Uri(Request.RequestUri + "/" + customerDTO.Id), customerDTO);
        }

        //PUT /api/customer/1
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDTO customerDTO)
        {
            if (!ModelState.IsValid)
                //throw new HttpResponseException(HttpStatusCode.BadRequest);
                //Using IHttpActionResult
                return BadRequest();

            var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDB == null)
                //throw new HttpResponseException(HttpStatusCode.NotFound);
                //Using IHttpActionResult
                return NotFound();

            Mapper.Map(customerDTO, customerInDB);

            _context.SaveChanges();

            return Ok();
        }

        //DELETE /api/customer/1
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customerInDB = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDB == null)
                return NotFound();

            _context.Customers.Remove(customerInDB);
            _context.SaveChanges();

            return Ok();
        }
    }
}
