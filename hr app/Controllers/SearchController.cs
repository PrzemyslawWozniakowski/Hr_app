using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hr_app.Models;
using hr_app.EntityFramework;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Simple action that returns offers with job title matching some string
        /// </summary>
        /// <param name="sstr"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public PagingViewModel GetJobOffers(string sstr="", int pageNo = 1, int pageSize = 4)
        {
            List<JobOfferIndexView> list = new List<JobOfferIndexView>();
            int totalPage, totalRecord;

            totalRecord = _context.JobOffers.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = (from u in _context.JobOffers
                          where u.JobTitle.Contains(sstr)
                          orderby u.JobTitle, u.Created
                          select u).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

          foreach(var el in record)
            {
                list.Add(new JobOfferIndexView(el));
            }

          PagingViewModel ret = new PagingViewModel
          {
              JobOffers = list,
              TotalPage = totalPage
          };

            return ret;
        }
    }
}