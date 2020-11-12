using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VCHeathCare.App_Start;
using VCHeathCare.Models;

namespace VCHeathCare.Services
{
    public class EmailRelayService
    {
        public bool SendOnSendGrid(Appointment appointment,ApplicationUser user)
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
            var name = user.Name;

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
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool SendOnGmailSmtp(Appointment appointment)
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
                emailService.SendGmailSmtpEmail(toUseremail);
                emailService.SendGmailSmtpEmail(toAdminEmail);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool SendOnSendGridSmtp(Appointment appointment)
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
                emailService.SendSendGridSmtpEmail(toUseremail);
                emailService.SendSendGridSmtpEmail(toAdminEmail);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}