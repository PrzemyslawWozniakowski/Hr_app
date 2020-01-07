using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using hr_app.EntityFramework;
using hr_app.Models;
using System.Security.Claims;
using Microsoft.Identity.Client;
using hr_app.BlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;


namespace hr_app.Controllers
{
    public class HRController : Controller
    {
        private readonly DataContext _context;
        public HRController(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Action that shows all job applications for job offers managed by HR user
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            
            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? id = _context.Users.Where(x => x.Email == email).First().Id;
            List<JobOffer> jos =_context.JobOffers.Where(x => x.UserId == id).ToList();
            List<JobApplication> jas = new List<JobApplication>();
            foreach(var el in jos)
            {
                jas.AddRange(_context.JobApplications.Where(x => x.JobOfferId == el.Id));
            }
            List<JobApplicationIndexView> jaiv = new List<JobApplicationIndexView>();
            foreach(var el in jas)
            {
                jaiv.Add(new JobApplicationIndexView {
                    Id = el.Id,
                    JobOfferId = el.JobOfferId,
                    Offer = _context.JobOffers.Where(x => x.Id == el.JobOfferId).First(),
                    FirstName = el.FirstName,
                    LastName = el.LastName,
                    PhoneNumber = el.PhoneNumber,
                    EmailAddress = el.EmailAddress,
                    ContactAgreement = el.ContactAgreement,
                    CvUrl = el.CvUrl,
                    State = el.State
                });
            }
            if (string.IsNullOrEmpty(searchString))
                return View(jaiv);
            else

                return View(jaiv.Where(x=> x.FirstName.Contains(searchString) || x.LastName.Contains(searchString)).ToList());
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Deny(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            jobApplication.State = Enums.State.Denied;

            _context.Update(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Accept(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            jobApplication.State = Enums.State.Accepted;

            _context.Update(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}