﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Web.Security;
using TabibV1.Models;

[assembly: OwinStartupAttribute(typeof(TabibV1.Startup))]
namespace TabibV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
             createRoles();
        }

         public void createRoles() {
           
        ApplicationDbContext context = new ApplicationDbContext();    
        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));    
       
  
        if (!roleManager.RoleExists("Admin"))   {   
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();    
            role.Name = "Admin";    
            roleManager.Create(role);    
  
            var user = new ApplicationUser();    
            user.UserName = "admin";    
            user.Email = "admin@gmail.com";
            user.DateBirthday = DateTime.Now;
            string userPWD = "adminadmin";

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));    
            var x=UserManager.Create(user, userPWD);
            if (x.Succeeded)    
            {    UserManager.AddToRole(user.Id, "Admin");      }


            var patientRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            patientRole.Name = "patient";
            roleManager.Create(patientRole);    

            var DoctorRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            DoctorRole.Name = "Doctor";
            roleManager.Create(DoctorRole);
        } 
         }
    }
}


//if (Roles.GetAllRoles().Length== 0)
//     {
//         Roles.CreateRole("adminstration");
//         Roles.CreateRole("patient");
//         Roles.CreateRole("Doctor");

//    var user = new ApplicationUser() { Email ="", UserName = "Admin", LastName = "adminstration", DateBirthday = new DateTime(1996,27,8)};
//     var result =  UserManager.CreateAsync(user, "Admin");
//     if (result.Succeeded)
//     {
//          Roles.AddUserToRole("admin", "adminstration");
//           }
//}