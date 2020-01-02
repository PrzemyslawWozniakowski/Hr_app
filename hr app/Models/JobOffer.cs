using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using hr_app.Utilities;

namespace hr_app.Models
{
    public class JobOffer
    {

        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public virtual Company Company { get; set; }
        public virtual int CompanyId { get; set; }
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
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
        public int UserId { get; set; }
        public User User { get; set; }

    }
}