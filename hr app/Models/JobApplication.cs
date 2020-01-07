using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace hr_app.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobOfferId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
        public Enums.State State { get; set; }

    }
}
