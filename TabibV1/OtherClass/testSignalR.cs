using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data.Entity;
using TabibV1.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR.Hubs;

namespace TabibV1.OtherClass
{
    public class testSignalR:Hub
    {
        public void getAllNotifications(string Role, string name)
        {
            if (name == null || name.Equals(""))
                return;
            List<Notifications> allNotification = new List<Notifications>();

            ApplicationDbContext db = new ApplicationDbContext();
            IQueryable<consulations> myNotifications;
        
            if (Role.Equals("Admin"))
                myNotifications = db.myConsulations.Where(x => x.Isreplaied == false);
            else if (Role.Equals("Doctor"))
                myNotifications = db.myConsulations.Include(x => x.doctor.user).Where(x => x.doctor.user.UserName.Equals(name) && x.Isreplaied == false);
            else
            {
                myNotifications = db.myConsulations.Include(x => x.patient.user).Where(x => x.Isreplaied == true && x.IsReading == false && x.patient.user.UserName.Equals(name));
                myNotifications.ToList().ForEach(x => x.IsReading = true);
                db.SaveChanges();
            }

            foreach (var item in myNotifications)
            {
                string messag = "need Reply";
                if (item.Isreplaied == true)
                    messag = "replied";
                allNotification.Add(new Notifications(item.consulationID.ToString(), item.IsReading, messag + ":" + item.Question + "?", "info"));
            }
            Clients.User(Context.User.Identity.Name).sendAllNotification(allNotification);
        }

        public void addNotification(string UserName, string message, string id)
        {
            Clients.User(UserName).addNotifications("info", message, id);
        }

        public void removeNotification(string UserName, string id)
        {

            Clients.User(UserName).removeNotification(id);
        }
    }

    public class Notifications
    {
        public string id;
        public bool isRead;
        public string message;
        public string type;

        IHubContext tempContext = GlobalHost.ConnectionManager.GetHubContext<testSignalR>();
        
        public Notifications(string Id, string Message)
        {
            id = Id;
            message = Message;
        }

        public Notifications(string Id, bool IsRead, string Message, string Type)
        {
            id = Id;
            message = Message;
            isRead = IsRead;
            type = Type;
        }


        public void addNotifications(string user)
        {
            tempContext.Clients.User(user).addNotifications(type, message, id);
        }

        public void removeNotification(string user)
        {
            tempContext.Clients.User(user).addNotifications(type, message, id);
        }
    }
}