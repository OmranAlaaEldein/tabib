using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tabib.Models;
using System.Net.Mail;
using System.Net;


namespace tabib.Controllers
{
    [Authorize]
    public class ConsulationController : Controller
    {
        private UsersContext db = new UsersContext();
        public ActionResult Index()  // GET: /Consulation/
        {
            List<consulation> result;
            if (User.Identity.Name.ToLower().Equals("admin"))
            {
                result = db.CusConsulation.OrderBy(x => x.Isreplaied).ThenBy (x=>x.DateOfconsulation ).ToList ();
            }
            else
            { // get consultion order by not replaid then by date
                result= db.CusConsulation.Include(x => x.UserProfile).Where(x => x.UserProfile.UserName.Equals(User.Identity.Name))
                    .OrderBy(x => x.Isreplaied).ThenBy(x => x.DateOfconsulation).ToList(); 
            }
             
             return View(result);
        }
        public ActionResult Details(int id = 0)  // GET: /Consulation/Details/5
        {
            consulation consulation = db.CusConsulation.Find(id);
            if (consulation == null)
            {   return HttpNotFound();
            }
            return View(consulation);
        }

        

        public ActionResult Create()          // GET: /Consulation/Create
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(consulation myconsulation, HttpPostedFileBase upload)  // POST: /Consulation/Create
        {
            if (ModelState.IsValid)
            { // save image to server
                string path = Path.Combine(Server.MapPath("~/Images"), upload.FileName);
                upload.SaveAs(path);
                myconsulation.PathImage = "/Images/" + upload.FileName;
                
                myconsulation.DateOfconsulation = DateTime.Now;
                myconsulation.UserProfile = db.UserProfiles.Where(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault(); //???
                
                db.CusConsulation.Add(myconsulation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myconsulation);
        }


          [Authorize(Roles = "adminstration")]
        public ActionResult Edit(int id = 0)          // GET: /Consulation/Edit/5
        {
            consulation consulation = db.CusConsulation.Find(id);
            if (consulation == null)
            {
                return HttpNotFound();
            }
            return View(consulation);
        }
        [HttpPost]
         [Authorize(Roles = "adminstration")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(consulation consulation)  // POST: /Consulation/Edit/5
        {
            if (ModelState.IsValid)
            {
                consulation oldconsulation = db.CusConsulation.Include("UserProfile").FirstOrDefault(e => e.consulationID.Equals(consulation.consulationID));
                if (consulation.reply != null && consulation.reply.Length > 0)  //&& oldconsulation.Isreplaied ==false
                {  // if there is reply to not repld consultion add date repld and plus 1 to notification
                    consulation.DateOfreply = DateTime.Now;
                    if (oldconsulation.Isreplaied == false)
                    {
                        Session["notification"] = Convert.ToInt16(Session["notification"]) - 1;
                        consulation.Isreplaied = true;
                    }
                    sendemail(oldconsulation.UserProfile.Email, "replayed", "dotor re1played on onsula;tion : " + consulation.reply + "\n to read : ");// link
                }

                db.Entry(oldconsulation).CurrentValues.SetValues(consulation);//  = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consulation);
        }

        public bool sendemail(string email,string Subject ,string message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["FromMail"]);
                mail.Subject = Subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.To.Add(email);

                SmtpClient smtp = new SmtpClient();
                smtp.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
                smtp.EnableSsl = true;
                
                

                NetworkCredential netw = new NetworkCredential();
                netw.UserName = mail.From.Address;
                netw.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];

                smtp.UseDefaultCredentials = true;
                smtp.Credentials = netw; 
                smtp.Port = 587;

                  //smtp is server , need newredational
                smtp.Send(mail);  //  send email to smtp

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

           
        }

       
        
        
        
        
        public ActionResult Delete(int id = 0)    // GET: /Consulation/Delete/5
        {
            consulation consulation = db.CusConsulation.Find(id);
            if (consulation == null)
            {
                return HttpNotFound();
            }
            return View(consulation);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)  // POST: /Consulation/Delete/5
        {
            consulation consulation = db.CusConsulation.Find(id);
            db.CusConsulation.Remove(consulation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}