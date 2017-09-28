using System;
using System.Collections.Generic;
//To include method
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomerController : Controller
    {
        //Add to give access to EF DBContext
        private ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new Models.ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customer

        //You can add it, for each Action
        //You can add it, for the whole class
        //You can add it, at FilterConfig    
        //[Authorize]

        //To add as cache - performance optimization
        //[OutputCache(Duration = 50, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "Genre")]
        public ActionResult Index()
        {
            //Replaced by jQuery in Index view
            //var customers = _context.Customers.Include(c => c.MembershipType).ToList();

            //if (MemoryCache.Default["Genres"] == null)
            //MemoryCache.Default["Genres"] = _context...

            //Remove <compilation debug=true>... from web.config

            return View();
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes,
                // To correct the bug on validation, setting Customer Id to 0 on creation
                Customer = new Customer()
            };

            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes
                };

                return View("CustomerForm", viewModel);
            }

            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerInDB = _context.Customers.Single(c => c.Id == customer.Id);

                //1) Microsoft MVC Official method
                //TryUpdateModel(customerInDB);

                //2) Mapper.Map
                //Data Transfer Object (later)

                customerInDB.Name = customer.Name;
                customerInDB.BirthDate = customer.BirthDate;
                customerInDB.MemberShipTypeId = customer.MemberShipTypeId;
                customerInDB.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            CustomerFormViewModel viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            //Specify the view name, using the same form for to actions: Edit and New
            return View("CustomerForm", viewModel);
        }

        //Rest API
        //GET /api/customer
        //GET /api/customr/1
        //POST /api/customer
        //PUT /api/customer/1
        //DELETE /api/customer/1
    }
}