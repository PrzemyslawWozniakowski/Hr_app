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
        //public virtual Company Company { get; set; }
        //public virtual int CompanyId { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }

        //public string Description { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        //[Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }
    }
}
