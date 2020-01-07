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
    public class JobApplicationsController : Controller
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _storage;

        public JobApplicationsController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _storage = new BlobStorageService(context, configuration);

        }

        // GET: JobApplications
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {

            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? id = _context.Users.Where(x => x.Email == email).First().Id;
            if (id != null) return View(await _context.JobApplications.Where(x => x.UserId == id).ToListAsync());
            return View(await _context.JobApplications.ToListAsync());
        }

        // GET: JobApplications/Details/5
        [Authorize(Roles = "User,HR")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? uid = _context.Users.Where(x => x.Email == email).First().Id;
            List<JobApplication> ja = await _context.JobApplications.Where(x => x.UserId == uid && x.JobOfferId == id).ToListAsync();


            return View(ja.First());
        }


        // GET: JobApplications/Edit/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? uid = _context.Users.Where(x => x.Email == email).First().Id;
            if (uid == null)
            {
                return NotFound();
            }
            List<JobApplication> ja = await _context.JobApplications.Where(x => x.UserId == uid && x.JobOfferId == id).ToListAsync();
            if (ja == null)
            {
                return NotFound();
            }
            JobApplicationEditView jaev = new JobApplicationEditView
            {
                Id = ja.First().Id,
                UserId = uid.Value,
                JobOfferId = id.Value,
                FirstName = ja.First().FirstName,
                LastName = ja.First().LastName,
                PhoneNumber = ja.First().PhoneNumber,
                EmailAddress = ja.First().EmailAddress,
                ContactAgreement = ja.First().ContactAgreement,
          
            };
            return View(jaev);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobApplicationEditView model)
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
            JobApplication ja = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == model.Id);
            ja.FirstName = model.FirstName;
            ja.LastName = model.LastName;
            ja.PhoneNumber = model.PhoneNumber;
            ja.EmailAddress = model.EmailAddress;
            ja.ContactAgreement = model.ContactAgreement;
            _storage.deleteFromStorage(ja);
            _storage.AddToStorage(ja, model.FormFile);

            _context.Update(ja);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
