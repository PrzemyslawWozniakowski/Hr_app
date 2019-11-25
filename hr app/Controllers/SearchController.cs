using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using hr_app.EntityFramework;
using hr_app.Models;

namespace hr_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly DataContext _context;

        public SearchController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public SearchViewModel GetJobTitles()
        {
            var offers = _context.JobOffers.ToList();
            //var a = offers.GroupBy(o => o.JobTitle);
            //var b = offers.GroupBy(o => o.JobTitle).Select(g => g.First());
            var offer_dist = offers.GroupBy(o => o.JobTitle).Select(g => g.First()).Select(o=>o.JobTitle);
            var ss = offer_dist.ToArray();

            SearchViewModel data = new SearchViewModel
            {
                JobTitles = ss
            };
            return data;
        }

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetJobOffer([FromRoute] int id)
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
    }
}