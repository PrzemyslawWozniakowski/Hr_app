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
using Microsoft.AspNetCore.Authorization;

namespace hr_app.Controllers
{
    public class JobOffersController : Controller
    {
        private readonly DataContext _context;

        public JobOffersController(DataContext context)
        {
            _context = context;
        }

        // GET: JobOffers
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            string email = User.FindFirst(x => x.Type.Equals(ClaimTypes.Email)).Value;
            int? id = _context.Users.Where(x => x.Email == email).First().Id;
            List<JobApplication> ja = await _context.JobApplications.Where(x => x.UserId == id).ToListAsync();
            List<JobOffer> jo = new List<JobOffer>();
            foreach (var el in ja)
            {
                jo.AddRange(_context.JobOffers.Include(x => x.Company).Where(x => x.Id == el.JobOfferId).ToList());
            }
            return View(jo);
        }
    }
}
