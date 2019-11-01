using System;
using System.ComponentModel.DataAnnotations;

namespace hr_app.Models
{
    public class Job_OfferViewModel
    {
        public Job_OfferViewModel(Job_OfferModel jo)
        {
            JobTitle = jo.JobTitle;
            SalaryFrom = jo.SalaryFrom;
            SalaryTo = jo.SalaryTo;
            Created = jo.Created;
            Location = jo.Location;
            ValidUntil = jo.ValidUntil;
        }

        public string JobTitle { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
