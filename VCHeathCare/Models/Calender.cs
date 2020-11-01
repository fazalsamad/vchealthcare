using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace VCHeathCare.Models
{
    [TableName("Calendar")]
    public class Calender
    {
        [Key]
        public int Id { get; set; }
        public int Day { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

       
    }
}