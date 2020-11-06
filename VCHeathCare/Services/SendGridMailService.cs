using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VCHeathCare.App_Start;
using VCHeathCare.Models;

namespace VCHeathCare.Services
{
    public class SendGridMailService
    {
        private readonly EmailCredential _emailCredential;
        public SendGridMailService()
        {
            _emailCredential = new EmailCredential();
        }

        public SendGridMessage Email(string message,string toEmail,string toName=null)
        { 
            var from = new EmailAddress(_emailCredential.AdminEmail, _emailCredential.Name);
            var subject = "Appointment Booking";
            var to = new EmailAddress(toEmail,toName); 
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            return msg;
        }

        public Response SendEmail(SendGridMessage message)
        {
            string apiKey = _emailCredential.Key;

            var client = new SendGridClient(apiKey);
            
            var response = Task.Run(async ()=> await client.SendEmailAsync(message)).Result;

            return response;
        }
    }
}