using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VCHeathCare.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult dashboard()
        {
            return View();
        }
    }
}