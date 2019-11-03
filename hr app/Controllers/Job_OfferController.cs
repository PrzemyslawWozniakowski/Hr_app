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
            List<Job_Offer> list = _context.job_Offers;
            List<Job_OfferIndexViewModel> listvm = new List<Job_OfferIndexViewModel>();
            foreach (var el in list)
            {
                listvm.Add(new Job_OfferIndexViewModel(el));
            }
            return View(listvm);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        public IActionResult Details()
        {

            return View(_context.job_Offers.First());
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}