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

            var markUp = $@"<table>
        <tbody>
            <tr>
                <th>Name</th>  <td>{appointment.Name}</td>
            </tr>
            <tr>
                <th>Email</th> <td>{appointment.Email}</td>
            </tr>
            <tr>
                <th>Date &amp; Time</th> <td>{appointment.Date.Value.ToLongDateString()} {appointment.Date.Value.ToLongTimeString()}</td>
            </tr>
            <tr>
                <th>Contact Number</th> <td>{appointment.Contact}</td>
            </tr>
            <tr>
                <th>Appointment Type</th>  <td>{appointment.AppointmentType}</td>
            </tr> 
        </tbody>
    </table>";

            var userPreMarkUp = @"<h2> We have received the following details for appointments.</h2>";
            var userPostMarkUp = "<h3>We will contact you soon!</h3>";

            
    
                var userMarkup = $"{userPreMarkUp}{markUp}{userPostMarkUp}";
            var toUseremail = new EmailModel(appointment.Email, "Appointment Booking", userMarkup);

            var credentials = new EmailCredential();

            var adminPreMarkUp = $"<h2>An appointment is booked by </h2> ";
            var adminMarkUp = $"{adminPreMarkUp}{markUp}";

            var toAdminEmail = new EmailModel(credentials.AdminEmail, "Appointment Booking", adminMarkUp);

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