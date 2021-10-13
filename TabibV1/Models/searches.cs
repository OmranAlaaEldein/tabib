using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TabibV1.Models
{
    public class searches
    {

        [Key]
        public int searchesID { get; set; }

        [Required]
        [Display(Name = "title")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 20)]
        public string titleOfsearches { get; set; }

        [Display(Name = "type of searches")]
        public string typeOfsearches { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 100)]
        public string Textsearches { get; set; }

        [DataType(DataType.ImageUrl)]
        public String PathOfImage { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfsearches { get; set; }

        public virtual ICollection<doctors> Writings { get; set; }

    }
}