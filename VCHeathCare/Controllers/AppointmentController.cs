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

            var result =  SendOnSendGrid(appointment);

            if (!result)
            {
                result = SendOnSmtp(appointment);

            }

            var status = result ? "sent" : "failed"; 

            return RedirectToAction("FeedBack", new { status });

        }

        public ActionResult FeedBack(string status)
        {
            ViewBag.Status = status;

            return View();
        }


        private bool SendOnSendGrid(Appointment appointment)
        {
            var date = appointment.Date.HasValue ? appointment.Date.Value : DateTime.Now;
            var markUp = $@" 
<table>
        <tbody>
            <tr>
                <th>Name</th>  <td>{appointment.Name}</td>
            </tr>
            <tr>
                <th>Email</th> <td>{appointment.Email}</td>
            </tr>
            <tr>
                <th>Date &amp; Time</th> <td>{date.ToLongDateString()} {date.ToLongTimeString()}</td>
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
            var adminPreMarkUp = $"<h2>An appointment is booked by </h2> ";
            var adminMarkUp = $"{adminPreMarkUp}{markUp}";
            var name = User.Identity.Name();

            var _emailCredentials = new EmailCredential();
            var emailService = new SendGridMailService();

            var adminEmail = emailService.Email(adminMarkUp, _emailCredentials.AdminEmail, _emailCredentials.Name);
            var userEmail = emailService.Email(userMarkup, appointment.Email, name);

            var result = true;
            try
            {


                var adminResponse = emailService.SendEmail(adminEmail);
                var userResponse = emailService.SendEmail(userEmail);

                var adminCode = (int)adminResponse.StatusCode;
                var userCode = (int)userResponse.StatusCode;

                result = (adminCode >= 200 && adminCode <= 299) && (userCode >= 200 && userCode <= 299);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private bool SendOnSmtp(Appointment appointment)
        {
            var date = appointment.Date.HasValue ? appointment.Date.Value : DateTime.Now;
            var markUp = $@" 
<table>
        <tbody>
            <tr>
                <th>Name</th>  <td>{appointment.Name}</td>
            </tr>
            <tr>
                <th>Email</th> <td>{appointment.Email}</td>
            </tr>
            <tr>
                <th>Date &amp; Time</th> <td>{date.ToLongDateString()} {date.ToLongTimeString()}</td>
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
            var result = true;
            try
            {
                emailService.SendEmail(toUseremail);
                emailService.SendEmail(toAdminEmail);
            }
            catch
            {
                result = false;
            }

            return result;
        }
            
    }
}