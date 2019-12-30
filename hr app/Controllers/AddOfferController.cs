using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hr_app.BlobStorage;
using hr_app.EntityFramework;
using hr_app.Models;
namespace hr_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddOfferController : ControllerBase
    {
        private readonly DataContext _context;

        public AddOfferController(DataContext context)
        {
            _context = context;

        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] int mCompanyId, [FromForm] string mDescription, [FromForm] string mJobTitle,
            [FromForm] string mLocation, [FromForm] decimal? mSalaryFrom, [FromForm] decimal? mSalaryTo,[FromForm] DateTime mValidUntil)
        {

            JobOffer jo = new JobOffer
            {
                CompanyId = mCompanyId,
                Description = mDescription,
                JobTitle = mJobTitle,
                Location = mLocation,
                SalaryFrom = mSalaryFrom,
                SalaryTo = mSalaryTo,
                ValidUntil = mValidUntil,
                Created = DateTime.Now
            };

            await _context.JobOffers.AddAsync(jo).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Ok();
        }
    }
}