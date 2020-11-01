using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCHeathCare.Models
{
    public class Date
    {
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Display(Name = "Date")]
        public DateTime Dt { get; set; }

        [Display(Name = "Appointment")]
        public string appointmenttype { get; set; }


    }
}