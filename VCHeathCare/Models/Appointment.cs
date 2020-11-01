using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCHeathCare.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Name")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        [Display(Name = "Date")]
        public DateTime? Date { get; set; } = DateTime.Now;

        [Display(Name = "Appointment")]
        public string AppointmentType { get; set; }


    }
}