using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
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
        public void SendGmailSmtpEmail(EmailModel emailModel)
        {
            MailMessage mail = new MailMessage();

            mail.To.Add(emailModel.To);
            mail.From = new MailAddress(_emailCredential.AdminEmail);
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

        public void SendSendGridSmtpEmail(EmailModel emailModel)
        { 
            var mailMsg = new MailMessage();
             
            mailMsg.To.Add(new MailAddress(emailModel.To));
             
            mailMsg.From = new MailAddress(_emailCredential.AdminEmail, _emailCredential.Name);
             
            mailMsg.Subject = "Appointment Booking";
            string text = emailModel.Body;
            string html = emailModel.Body;

            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));

            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html)); 

            var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

            var credentials = new System.Net.NetworkCredential("apikey", _emailCredential.Key);

            smtpClient.Credentials = credentials; 

            smtpClient.Send(mailMsg);
        }
    }
}