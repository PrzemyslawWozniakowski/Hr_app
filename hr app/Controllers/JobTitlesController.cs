using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using hr_app.EntityFramework;
using hr_app.Models;
using Microsoft.AspNetCore.Authorization;

namespace hr_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitlesController : ControllerBase
    {
        private readonly DataContext _context;

        public JobTitlesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public SearchViewModel GetJobTitles()
        {
            var offers = _context.JobOffers.ToList();
    
            var offer_dist = offers.GroupBy(o => o.JobTitle).Select(g => g.First()).Select(o=>o.JobTitle);
            var ss = offer_dist.ToArray();

            SearchViewModel data = new SearchViewModel
            {
                JobTitles = ss
            };
            return data;
        }


    }
}