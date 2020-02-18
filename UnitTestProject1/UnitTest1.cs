using hr_app.Controllers;
using hr_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Enums;
namespace UnitTestProject1
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_WhenItsCalled_ShouldReturnObject()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.IndexHR() as ViewResult;

            Assert.IsNotNull(result);
        }
    }

    [TestClass]
    public class JobOfferTest
    {
        [TestMethod]
        public void JobOffer_Validated_WhenSalaryFromBiggerThanSalaryTo()
        {
            var jo = new JobOffer
            {
                Id = 1,
                JobTitle = "SomeTitle",
                CompanyId = 1,
                SalaryFrom = (decimal)1000,
                SalaryTo = (decimal)100,
                Created = new System.DateTime(2010, 10, 10),
                Location = "Warszawa",
                Description = "Lorem ipsum",
                ValidUntil = System.DateTime.Now,
                UserId = 1
            };

            Assert.IsTrue(ValidateModel(jo).Any(
          v => v.ErrorMessage.Contains("Salary to shouldn't be smaller than Salary from")));

        }
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

    }

    [TestClass]
    public class JobApplicationTest
    {
        [TestMethod]
        public void JobApplication_Validated_WhenNoEmail()
        {
            var jo = new JobApplication
            {
                Id=1,
                JobOfferId=1,
                UserId=1,
                FirstName="Bob",
                LastName="Boboewski",
                PhoneNumber ="112322111",
                EmailAddress=null,
                ContactAgreement=true,
                CvUrl="www.wp.pl",
                State=Enums.State.Accepted
            };

            Assert.IsTrue(ValidateModel(jo).Any(
          v => v.ErrorMessage.Contains("required")));

        }
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

    }


}
