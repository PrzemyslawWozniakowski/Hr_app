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
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;
        private readonly BlobStorageService _storage;
        private readonly SendGridService _sendgrid;
        public JobOfferController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _storage = new BlobStorageService(context, configuration);
            _sendgrid = new SendGridService(context, configuration);
        }
       

        
        [Authorize(Roles = "Admin")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Create_Company()
        {

            var model = new CompanyCreateView {  };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize (Roles ="HR")]
        [ApiExplorerSettings(IgnoreApi = true)]
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

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR")]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.SalaryFrom = model.SalaryFrom;
            offer.SalaryTo = model.SalaryTo;
            offer.Location = model.Location;
            offer.Description = model.Description;
            offer.ValidUntil = model.ValidUntil;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }
        [HttpPost]
        [Authorize(Roles = "HR")]
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


        [Authorize(Roles = "HR")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };

            return View(model);
        }


        [Authorize(Roles = "HR,User")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            JobOfferDetailsView returnoffer = new JobOfferDetailsView(offer);
            return View(returnoffer);
        }
    }
}
