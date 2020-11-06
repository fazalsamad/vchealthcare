using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using VCHeathCare.App_Start;
using VCHeathCare.Models;
using VCHeathCare.Services;

namespace VCHeathCare.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var list = new List<string>() { "Gp Appointment", "Consultant", "Clinins" };
            ViewBag.list = list;

            var user = User.Identity.User();

            var defaultAppointment = new Appointment()
            {
                Name = user.Name,
                Contact = user.PhoneNumber,
                Email = user.Email
            };
            return View(defaultAppointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new ApplicationDbContext())
                {
                    ctx.Appointments.Add(appointment);
                    ctx.SaveChanges();
                };
            }

            var user = User.Identity.User();

            var relayService = new EmailRelayService(); 

            var result = relayService.SendOnSendGrid(appointment,user);

            if (!result)
            {
                result = relayService.SendOnSendGridSmtp(appointment);

            }

            if (!result)
            {
                result = relayService.SendOnGmailSmtp(appointment);

            }
            var status = result ? "sent" : "failed"; 

            return RedirectToAction("FeedBack", new { status });

        }

        public ActionResult FeedBack(string status)
        {
            ViewBag.Status = status;

            return View();
        } 
    }
}