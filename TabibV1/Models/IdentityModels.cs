using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TabibV1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public ApplicationUser()
        { }
       
        public string LastName { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateBirthday { get; set; }

        [DataType(DataType.ImageUrl)]
        public String PathOfImage { get; set; }

        public virtual doctors doctor { get; set; }
        public virtual patients patient { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>  //DbContext
    {
        public ApplicationDbContext()
            : base("TabibConnection")
        { }


        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception exception = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);

                        exception = new InvalidOperationException(message, exception);
                    }
                }
                throw exception;
            }

        }
            public DbSet<patients> myPatients { set; get; }
            public DbSet<doctors> myDoctors { set; get; }
            public DbSet<appointments> myAppointments { set; get; }
            public DbSet<consulations> myConsulations { get; set; }
            public DbSet<searches> mySearches { set; get; }
            public DbSet<articals> myArticals { set; get; }
            public DbSet<myModel> myModels { set; get; }
            public DbSet<DoctorAppointment> myDoctorAppointment { set; get; }
            public DbSet<Address> myAddress { set; get; }
    }

}