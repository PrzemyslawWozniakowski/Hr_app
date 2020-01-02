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

namespace hr_app.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _storage;

        public JobOfferController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _storage = new BlobStorageService(context, configuration);
            
        }
        [Authorize]
        public IActionResult CreateJobApplication(int ?id)
        {
            if(id==null)
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
        public async Task<ActionResult> CreateJobApplication(JobApplicationCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            JobApplication ja = new JobApplication
            {
                JobOfferId = model.JobOfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,

            };
            //// correct to use view model
            _storage.AddToStorage(ja,model.FormFile);

            await _context.JobApplications.AddAsync(ja);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Create_Company()
        {

            var model = new CompanyCreateView {  };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create_Company(CompanyCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Company ca = new Company
            {
                Name=model.Name

            };

            await _context.Companies.AddAsync(ca);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            List<JobOffer> searchResult;
            if (string.IsNullOrEmpty(searchString))
               searchResult = await _context.JobOffers.Include(x => x.Company).ToListAsync();
            else
               searchResult = await _context.JobOffers.Include(x => x.Company).Where(o => o.JobTitle.Contains(searchString)).ToListAsync();

            List<JobOfferIndexView> finalResult = new List<JobOfferIndexView>();
            foreach(var el in searchResult)
            {
                JobOfferIndexView jo = new JobOfferIndexView(el);
                finalResult.Add(jo);
            }
            return View(finalResult);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }
            JobOfferEditView jo = new JobOfferEditView(offer);
            // nie edytować już edytowanych
            return View(jo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOfferEditView model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            _context.JobOffers.Remove(new JobOffer() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };

            return View(model);
        }


        //[Route("JobOffer/Add")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add(int mCompanyId/*, string mDescription, string mJobTitle, string mLocation, decimal? mSalaryFrom, decimal? mSalaryTo, DateTime mValidUntil*/)
        //{

        //    JobOffer jo = new JobOffer
        //    {
        //        CompanyId = mCompanyId,
        //        Description = "Lorem ipsum",
        //        JobTitle = "Some job",
        //        Location = "Somewhere",
        //        SalaryFrom = (decimal)1000,
        //        SalaryTo = (decimal)1001,
        //        ValidUntil = new DateTime(2020, 1, 1),
        //        Created = DateTime.Now
        //    };

        //    await _context.JobOffers.AddAsync(jo).ConfigureAwait(false);
        //    await _context.SaveChangesAsync().ConfigureAwait(false);

        //    return RedirectToAction("Index", "JobOffer");
        //}

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            JobOfferDetailsView returnoffer = new JobOfferDetailsView(offer);
            return View(returnoffer);
        }
    }
}
