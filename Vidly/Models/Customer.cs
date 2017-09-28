using System;
using System.Collections.Generic;
//Add to change Entity Framework defaults
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }

        //Overriding validation messages
        [Required(ErrorMessage = "Please enter customer's name.")]
        [StringLength(255)]
        //Other data annotations:
        //[Range(1, 10)]
        //[Compare("Other Property")]
        //[Phone]
        //[EmailAddress]
        //[URL]
        //[RegularExpression("...")]

        public string Name { get; set; }
        public bool IsSubscribedToNewsletter { get; set; }
        public MembershipType MembershipType { get; set; }

        [Display(Name = "Membership Type")]
        public byte MemberShipTypeId { get; set; }

        //Every time you change the form label, you must recompile
        //[Display(Name = "Date of Birth")]
        [ValidationMin18Years]
        public DateTime? BirthDate { get; set; }
    }
}