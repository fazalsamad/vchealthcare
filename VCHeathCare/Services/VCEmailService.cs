using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using VCHeathCare.App_Start;
using VCHeathCare.Models;

namespace VCHeathCare.Services
{
    public class VCEmailService
    {
        private readonly EmailCredential _emailCredential;
        public VCEmailService()
        {
            _emailCredential = new EmailCredential();
        }
        public void SendEmail(EmailModel emailModel)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(emailModel.To);
            mail.From = new MailAddress(emailModel.From);
            mail.Subject = emailModel.Subject;
            mail.Body  = emailModel.Body; 
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false; 


            smtp.Credentials = new System.Net.NetworkCredential(_emailCredential.Username, _emailCredential.Secret);
            
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}