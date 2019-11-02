using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hr_app.Models;

namespace hr_app.Controllers
{
    public class Job_OfferController : Controller
    {
        private readonly Job_OfferContext _context;

        public Job_OfferController()
        {
            _context = new Job_OfferContext();
        }

        public IActionResult Job_offers()
        {
            List<Job_OfferModel> list = _context.job_Offers;
            List<Job_OfferViewModel> listvm = new List<Job_OfferViewModel>();
            foreach (var el in list)
            {
                listvm.Add(new Job_OfferViewModel(el));
            }
            return View(listvm);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}