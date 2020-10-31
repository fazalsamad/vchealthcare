using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

            
            var toUseremail = new EmailModel(appointment.Email, "Appointment Booking", "<h2>Your appointment is booked.</h2>");

            var credentials = new EmailCredential();

            var message = $"<p>An appointment is booked by {appointment.Name} <br/>" +
              $"Email: {appointment.Email} <br/>" +
              $"Date &amp; Time: {appointment.Date.Value.ToLongDateString()}-{appointment.Date.Value.ToLongTimeString()}<br/>" +
              $"Contact: {appointment.Contact}<br/>" +
              $"Appointment Type: {appointment.AppointmentType}</p>";
            var toAdminEmail = new EmailModel(credentials.AdminEmail, "Appointment Booking", message);

            var emailService = new VCEmailService();

            emailService.SendEmail(toUseremail);
            emailService.SendEmail(toAdminEmail);

            return RedirectToAction("SuccessFeedBack");
        }

        public ActionResult SuccessFeedBack()
        {
            return View();
        }
    }
}