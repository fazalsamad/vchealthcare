using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCHeathCare.Controllers
{
    public class DateController : Controller
    {
        // GET: Date
        public ActionResult dateTime()
        {

            var list = new List<string>() { "Gp Appointment", "Consultant", "Clinins" };
            ViewBag.list = list;
            return View();
        }
    }
}