using System;
using System.ComponentModel.DataAnnotations;


namespace hr_app.Models
{
    public class Job_OfferModel
    {
        public Job_OfferModel(int id, string jobtitle, decimal salaryfrom, decimal salaryto, DateTime created, string location,string description, DateTime validuntil)
        {
            Id = id;
            JobTitle = jobtitle;
            SalaryFrom = salaryfrom;
            SalaryTo = salaryto;
            Created = created;
            Location = location;
            Description = description;
            ValidUntil = validuntil;
        }
        public int Id { get; set; }

        public string JobTitle { get; set; }
        //public virtual Company Company { get; set; }
        //public virtual int CompanyId { get; set; }

        public decimal? SalaryFrom { get; set; }

        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        //[Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }


    }
}