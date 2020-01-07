using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Identity.Client;
using hr_app.Models;
using hr_app.EntityFramework;

namespace hr_app.Controllers
{
    public class AdminController : Controller
    {
        private readonly DataContext _context;
        public AdminController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {

            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? id = _context.Users.Where(x => x.Email == email).First().Id;
            List<JobOffer> jos = _context.JobOffers.Where(x => x.UserId == id).ToList();
            List<JobApplication> jas = new List<JobApplication>();
            foreach (var el in jos)
            {
                jas.AddRange(_context.JobApplications.Where(x => x.JobOfferId == el.Id));
            }
            List<JobApplicationAdminView> jaiv = new List<JobApplicationAdminView>();
            foreach (var el in jas)
            {
                jaiv.Add(new JobApplicationAdminView
                {
                    Id = el.Id,
                    JobOfferId = el.JobOfferId,
                    Offer = _context.JobOffers.Where(x => x.Id == el.JobOfferId).First(),
                    FirstName = el.FirstName,
                    LastName = el.LastName,
                    PhoneNumber = el.PhoneNumber,
                    EmailAddress = el.EmailAddress,
                });
            }
            if (string.IsNullOrEmpty(searchString))
                return View(jaiv);
            else

                return View(jaiv.Where(x => x.FirstName.Contains(searchString) || x.LastName.Contains(searchString)).ToList());
        }
    }
}