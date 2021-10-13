using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TabibV1.Models;

namespace TabibV1.Models
{
    public class patients
    {
        public patients()
        {
            countVisit = 0;
        }
        [Key, ForeignKey("user")]
        public string patientsId { get; set; }
        public int countVisit { get; set; }
        
        public  virtual ApplicationUser user { get; set; }
        public virtual ICollection<consulations> consulation { get; set; }
        public virtual ICollection<appointments> appointment { get; set; }
    }
}