using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace hr_app.Models
{
    public class Job_Offer
    {
        public Job_Offer(int id, string jobtitle, decimal salaryfrom, decimal salaryto, DateTime created, string location,string description, DateTime validuntil, Company company, int companyid)
        {
            Id = id;
            JobTitle = jobtitle;
            SalaryFrom = salaryfrom;
            SalaryTo = salaryto;
            Created = created;
            Location = location;
            Description = description;
            ValidUntil = validuntil;
            Company = company;
            CompanyId = companyid;
        }

        public int Id { get; set; }
        [Display(Name = "Job title")]
        [Required]
        public string JobTitle { get; set; }
        public virtual Company Company { get; set; }
        public virtual int CompanyId { get; set; }
        [Display(Name = "Salary from")]
        public decimal? SalaryFrom { get; set; }
        [Display(Name = "Salary to")]
        public decimal? SalaryTo { get; set; }
        public DateTime Created { get; set; }
        public string Location { get; set; }
        [Required]
        [MinLength(5)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        [Display(Name = "Valid until")]
        public DateTime? ValidUntil { get; set; }
        public List<Job_Application> JobApplications { get; set; } = new List<Job_Application>();



    }
}