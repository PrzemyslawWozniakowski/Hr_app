using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hr_app.Models;
using hr_app.EntityFramework;
using Microsoft.Extensions.Configuration;
using hr_app.BlobStorage;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Identity.Client;
using hr_app.Content;

namespace hr_app.Controllers
{
    [Route("[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AddApplicationController : Controller
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _storage;
        private readonly SendGridService _sendgrid;

        public AddApplicationController(DataContext context , IConfiguration configuration)
        {
            _context = context;
            _storage = new BlobStorageService(context, configuration);
            _sendgrid = new SendGridService(context, configuration);

        }


        [Authorize(Roles = "User")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult CreateJobApplication(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var model = new JobApplicationCreateView
            {
                Id = 0,
                JobOfferId = id.Value
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateJobApplication(JobApplicationCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? id = _context.Users.Where(x => x.Email == email).First().Id;
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            JobApplication ja = new JobApplication
            {
                JobOfferId = model.JobOfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                UserId = (int)id
            };
            _storage.AddToStorage(ja, model.FormFile);
            await _sendgrid.SendMailNotification(ja);
            await _context.JobApplications.AddAsync(ja);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexUser", "Home");
        }

    }
}