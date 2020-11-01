using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using VCHeathCare.App_Start;

namespace VCHeathCare.Models
{
    public class EmailModel
    {
        private readonly EmailCredential _emailCredential;
        public EmailModel(string to, string subject, string body)
        {
             
            To = to;
            Subject = subject;
            Body = body;
            _emailCredential = new EmailCredential();
            From = _emailCredential.Username;
        }
        public EmailModel(string from, string to, string subject, string body)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            _emailCredential = new EmailCredential();

        }


        public string From { get; set; } 
        public string To { get; set; } 
        public string Subject { get; set; } 
        public string Body { get; set; }
    }
}