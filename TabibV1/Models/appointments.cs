using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TabibV1.Models
{
    public class appointments
    {
        public int appointmentsId { get; set; }
        public virtual doctors doctor { get; set; }
        public virtual patients patient { get; set; }
        public DateTime appointmentsTime { get; set; }
    }
}