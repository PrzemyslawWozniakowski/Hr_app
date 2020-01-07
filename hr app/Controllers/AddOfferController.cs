using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hr_app.BlobStorage;
using hr_app.EntityFramework;
using hr_app.Models;
using System.Security.Claims;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Action for creating offers
        /// </summary>
        /// <param name="mCompanyId"></param>
        /// <param name="mDescription"></param>
        /// <param name="mJobTitle"></param>
        /// <param name="mLocation"></param>
        /// <param name="mSalaryFrom"></param>
        /// <param name="mSalaryTo"></param>
        /// <param name="mValidUntil"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize (Roles ="HR")]
        public async Task<IActionResult> Add([FromForm] int mCompanyId, [FromForm] string mDescription, [FromForm] string mJobTitle,
            [FromForm] string mLocation, [FromForm] decimal? mSalaryFrom, [FromForm] decimal? mSalaryTo,[FromForm] DateTime mValidUntil)
        {
            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? uid = _context.Users.Where(x => x.Email == email).First().Id;
            JobOffer jo = new JobOffer
            {
                UserId = uid.Value,
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