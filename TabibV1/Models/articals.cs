using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TabibV1.Models
{
    public class articals
    {

        [Key]
        public int articalsID { get; set; }

        [Required]
        [Display(Name = "title")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 20)]
        public string titleOfarticls { get; set; }

        [Display(Name = "type of articls")]
        public string typeOfarticls { get; set; }

        [Display(Name = "articl Writer")]
        public string articlWriter { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 100)]
        public string TextArticles { get; set; }

        [DataType(DataType.ImageUrl)]
        public String PathOfImage { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfarticals { get; set; } 
    }
}