using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_app.Models
{
    public class JobApplicationCreateView
    {
      
        public int Id { get; set; }
        public int JobOfferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool ContactAgreement { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
