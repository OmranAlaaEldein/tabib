using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace tabib.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("TabibConnection")
        {
        }
        
        public DbSet<consulation> CusConsulation { set; get; }
        public DbSet<artical> DocArticals { set; get; }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Role> Roles { set; get; }
        public DbSet<Visiting> Visiting { set; get; }
        
    }


    [Table("Visiting")]
    public class Visiting
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string IpAddressVisitor { get; set; }        
        public int countVisit { get; set; }
    }
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        { Roles = new HashSet<Role>(); 
        countVisit=0;}
        public UserProfile (DateTime  dateBirthday ,string email ,string lastName,string username)
        {
            UserName = username; LastName = lastName; Email = email; DateBirthday = dateBirthday;
            Roles = new HashSet<Role>();
            countVisit=0;
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        public string LastName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateBirthday { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        
        
        public int countVisit { get; set; } 
        public ICollection<consulation> consulation { get; set; }
    }

    [Table("webpages_Roles")]
    public class Role
    {
       public Role(string name)
        {
            RoleName = name;
            UserProfiles = new HashSet<UserProfile>();
        }
       public Role()
       {
           UserProfiles = new HashSet<UserProfile>();
       } 
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateBirthday { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        
        //    [Required]
          //  [Display(Name = "captcha")]
        //public string captcha { get; set; }

    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
