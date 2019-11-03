using System;
using System.ComponentModel.DataAnnotations;

namespace hr_app.Models
{
    public class Job_OfferIndexViewModel
    {
        public Job_OfferIndexViewModel(Job_Offer jo)
        {
            Id = jo.Id;
            JobTitle = jo.JobTitle;
            Company = jo.Company;
            Created = jo.Created;
            Location = jo.Location;
        }

        public int Id;
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public virtual Company Company { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }


    }
}
