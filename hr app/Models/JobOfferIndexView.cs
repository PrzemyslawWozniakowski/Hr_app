using System;
using System.ComponentModel.DataAnnotations;

namespace hr_app.Models
{
    public class JobOfferIndexView 
    {
        public JobOfferIndexView(JobOffer offer)
        {
            Id = offer.Id;
            JobTitle = offer.JobTitle;
            Company = offer.Company;
            Location = offer.Location;
            Created = offer.Created;
        }
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public Company Company{ get; set; }
        public string Location { get; set; }
        public DateTime Created { get; set; }
    }
}
