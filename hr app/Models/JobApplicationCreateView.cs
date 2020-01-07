using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace hr_app.Models
{
    public class JobApplicationCreateView
    {
      
        public int Id { get; set; }
        public int JobOfferId { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Contact Agreement")]
        public bool ContactAgreement { get; set; }
        [Display(Name = "Attach your CV")]
        [Required(ErrorMessage = "Please submit your CV.")]
        public IFormFile FormFile { get; set; }
    }
}
