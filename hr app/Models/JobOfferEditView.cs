using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using hr_app.Utilities;

namespace hr_app.Models
{
    public class JobOfferEditView
    {
        public JobOfferEditView(JobOffer offer)
        {
            Id = offer.Id;
            JobTitle = offer.JobTitle;
            SalaryFrom = offer.SalaryFrom;
            SalaryTo = offer.SalaryTo;
            Created = offer.Created;
            Location = offer.Location;
            Description = offer.Description;
            ValidUntil = offer.ValidUntil;
        }

        public JobOfferEditView(int _Id, string _JobTitle, decimal _SalaryFrom, decimal _SalaryTo, DateTime _Created, string _Location, string _Description, DateTime _ValidUntil)
        {
            Id = _Id;
            JobTitle = _JobTitle;
            SalaryFrom = _SalaryFrom;
            SalaryTo = _SalaryTo;
            Created = _Created;
            Location = _Location;
            Description = _Description;
            ValidUntil = _ValidUntil;
        }

        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        [Display(Name = "Salary from")]
        [SalaryCorrectRangeAttribute(ValidateSalaryFrom = true, ErrorMessage = "Salary from shouldn't be larger than Salary to")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        [SalaryCorrectRangeAttribute(ValidateSalaryTo = true, ErrorMessage = "Salary to shouldn't be smaller than Salary from")]
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Valid until")]
        [DateCorrectRangeAttribute(ValidateValidUntil = true, ErrorMessage = "Valid until date should be after current date")]
        public DateTime? ValidUntil { get; set; }
       

    }
}