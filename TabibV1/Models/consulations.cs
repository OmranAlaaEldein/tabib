using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TabibV1.Models
{
    public class consulations
    {
        [Key]
        public int consulationID { get; set; }

        [Required]
        [Display(Name = "title")]
        [MaxLength(50)]
        public string title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 20)]
        public string Question { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfconsulation { get; set; }

        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string reply { get; set; }

        public bool Isreplaied { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateOfreply { get; set; }

        [DataType(DataType.ImageUrl)]
        public string PathImage { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string KeyWords { get; set; }

        public virtual doctors doctor { get; set; }
        public virtual patients patient { get; set; }
    }
}