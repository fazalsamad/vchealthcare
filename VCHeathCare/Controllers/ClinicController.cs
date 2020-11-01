using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VCHeathCare.Models;

namespace VCHeathCare.Controllers
{
    public class ClinicController : Controller
    {
        // GET: Clinic
        public ActionResult Categories()
        {
            return View();
        }

       
    }
}