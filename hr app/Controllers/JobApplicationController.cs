using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace hr_app.Controllers
{
    public class JobApplicationController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}