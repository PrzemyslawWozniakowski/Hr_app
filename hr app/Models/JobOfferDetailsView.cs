using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_app.Models
{
    public class JobOfferDetailsView
    {
        public JobOfferDetailsView(JobOffer offer)
        {
            Id = offer.Id;
            JobTitle = offer.JobTitle;
            Company = offer.Company;
            SalaryFrom = offer.SalaryFrom;
            SalaryTo = offer.SalaryTo;
            Created = offer.Created;
            Location = offer.Location;
            Description = offer.Description;
            ValidUntil = offer.ValidUntil;
        }

        public int Id { get; set; }
        public string JobTitle { get; set; }
        public Company Company { get; set; }
        public decimal? SalaryFrom { get; set; }
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
