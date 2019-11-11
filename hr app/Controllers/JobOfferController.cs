using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hr_app.Models;
using hr_app.EntityFramework;

namespace hr_app.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        

        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }

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
        //
        // GET: api/Users
        //[HttpGet]
        //public PagingViewModel GetUsers(int pageNo = 1, int pageSize = 4)
        //{
        //    int totalPage, totalRecord;

        //    totalRecord = _context.JobOffers.Count();
        //    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
        //    var record = (from u in _context.JobOffers
        //                  orderby u.JobTitle, u.Created
        //                  select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

        //    PagingViewModel empData = new PagingViewModel
        //    {
        //        JobOffers = record,
        //        TotalPage = totalPage
        //    };

        //    return empData;
        //}
        ////
        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUser([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _context.JobOffers.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}
        //

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
                FirstName=model.FirstName,
                LastName=model.LastName,
                PhoneNumber=model.PhoneNumber,
                EmailAddress=model.EmailAddress,
                ContactAgreement=model.ContactAgreement,
                CvUrl=model.CvUrl,
            
            };
            // correct to use view model

            await _context.JobApplications.AddAsync(ja);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


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

        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = await _context.Companies.ToListAsync();
                return View(model);
            }

            JobOffer jo = new JobOffer
            {
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            await _context.JobOffers.AddAsync(jo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.Include(x => x.Company).FirstOrDefaultAsync(x => x.Id == id);
            JobOfferDetailsView returnoffer = new JobOfferDetailsView(offer);
            return View(returnoffer);
        }
    }
}
