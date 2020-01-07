using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using hr_app.EntityFramework;
using hr_app.Models;

namespace hr_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly DataContext _context;

        public OffersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/offers
        [HttpGet]
        [Authorize]
        public PagingViewModel GetJobOffers(int pageNo = 1, int pageSize = 4)
        {
            int totalPage, totalRecord;

            totalRecord = _context.JobOffers.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = (from u in _context.JobOffers
                          orderby u.JobTitle, u.Created
                          select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            List<JobOfferIndexView> rec = new List<JobOfferIndexView>();
            foreach(var el in record)
            {
                rec.Add(new JobOfferIndexView(el));
            }
            PagingViewModel data = new PagingViewModel
            {
                JobOffers = rec,
                TotalPage = totalPage
            };

            return data;
        }

        // GET: api/offers/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetJobOffer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.JobOffers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }

}