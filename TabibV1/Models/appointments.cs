using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TabibV1.Models
{
    public class appointments
    {
        public int appointmentsId { get; set; }
        
        [Display(Name = "Doctor Name")]
        [MaxLength(50)]
        public string DoctorName { get; set; }
        public string Question { get; set; }
        public string Note { get; set; }
        public int evaluation { get; set; }
        
        public virtual doctors doctor { get; set; }
        public virtual patients patient { get; set; }
        public DateTime appointmentsTime { get; set; }
    }
}