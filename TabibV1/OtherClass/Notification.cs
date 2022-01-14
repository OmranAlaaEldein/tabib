using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using TabibV1.Models;

namespace TabibV1.OtherClass
{

    public class Notification:Hub
    {
        //int id;
        //string message;
        //bool isRead = false;

        //public Notification(int Id,string Message,bool IsRead) {
        //    id = Id;
        //    message = Message;
        //    isRead = IsRead;
        //}

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }
        //public void addNotification(string userId)
        //{
        //    Clients.All.send("Hi");
        //   // Clients.User(userId).send(this.message);
        //}

        //public void removeNotification()
        //{

        //}
    }
}