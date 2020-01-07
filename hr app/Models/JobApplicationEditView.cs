using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace hr_app.Models
{
    public class JobApplicationEditView
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobOfferId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Contact Agreement")]
        public bool ContactAgreement { get; set; }
        [Display(Name = "Attach your CV")]
        public IFormFile FormFile { get; set; }

    }
}
