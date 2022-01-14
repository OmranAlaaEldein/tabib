using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using TabibV1.Models;
using TabibV1.OtherClass;


namespace TabibV1.Controllers
{
    public class ConsulationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Display
        /// </summary>
        public ActionResult Index()
        {

            List<consulations> result =null;
            if (User.IsInRole("Admin"))
            {
                result = db.myConsulations.OrderBy(x => x.Isreplaied).ThenBy(x => x.DateOfconsulation).ToList();
            }
            if (User.IsInRole("patient"))
            {
                result = db.myConsulations.Include(x => x.patient.user).Where(x => x.patient.user.UserName.Equals(User.Identity.Name))
                    .OrderBy(x => x.DateOfconsulation).ToList();
            }
            if (User.IsInRole("Doctor"))
            {
                result = db.myConsulations.Include(x => x.doctor.user).Include(x => x.patient.user).Where(x => x.doctor.user.UserName.Equals(User.Identity.Name))
                     .OrderBy(x => x.Isreplaied).ThenBy(x => x.DateOfconsulation).ToList();
            }

            List<string> items = db.myDoctors.Select(x => x.specialty).Distinct().ToList();
            if (items.Count > 0)
            {
                List<SelectListItem> DoctorListItem = new List<SelectListItem>();
                for (int i = 0; i < items.Count; i++)
                {
                    DoctorListItem.Add(new SelectListItem { Selected = false, Text = items[i], Value = items[i] });
                }
                DoctorListItem.Add(new SelectListItem { Selected = false, Text = "all", Value = "all" });
                ViewBag.DoctorListItem = new SelectList(DoctorListItem, "Value", "Text");
            }
            else
                ViewBag.DoctorListItem = null;
           
            return View(result);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            consulations consulations = db.myConsulations.Find(id);
            if (consulations == null)
            {
                return HttpNotFound();
            }
            return View(consulations);
        }

        /// <summary>
        /// Create
        /// </summary>
        [Authorize(Roles = "patient")]
        public ActionResult Create()
        {
            List<string> items=db.myDoctors.Select(x=>x.specialty).Distinct().ToList();
            if (items.Count > 0)
            {
                List<SelectListItem> DoctorListItem = new List<SelectListItem>();
                for (int i = 0; i < items.Count; i++)
                {
                    DoctorListItem.Add(new SelectListItem { Selected = false, Text = items[i], Value = items[i] });
                }
                ViewBag.DoctorListItem = new SelectList(DoctorListItem,"Value","Text");
            }
            else
                ViewBag.DoctorListItem = null;
           
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "patient")]
        public ActionResult Create([Bind(Include = "title,Question,specialty")] consulations consulations, HttpPostedFileBase upload)
        {   
            if (ModelState.IsValid)
            {
                consulations.patient = db.myPatients.Where(x => x.user.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                consulations.doctor = db.myDoctors.Where(s=>s.specialty.Equals(consulations.specialty)).Select(s => new 
                          { 
                             Item = s,
                             Count = s.consulation.Count()
                          }).OrderBy(s=>s.Count).ThenBy(s=>s.Item.evaluate).FirstOrDefault().Item;
                consulations.DateOfconsulation = DateTime.Now;
                consulations.DoctorName = consulations.doctor.user.UserName + " " + consulations.doctor.user.UserName;

                
                db.myConsulations.Add(consulations);
                db.SaveChanges();

                string fileName = consulations.consulationID + "." + upload.FileName.Substring(upload.FileName.IndexOf(".") + 1);
                consulations.PathImage = "/images/consulations/" + fileName;
                string path = Path.Combine(Server.MapPath("~/images/consulations"), fileName);
                upload.SaveAs(path);
                db.SaveChanges();

                //(redirect)Notifications test = new Notifications(consulations.consulationID.ToString(),false, "need to reply:" + consulations.Question + ",by:" + consulations.patient.user.UserName + " " + consulations.patient.user.LastName, "info");test.addNotifications(consulations.doctor.user.UserName);
                Notifications test2 = new Notifications(consulations.consulationID.ToString(), false, "need to reply:" + consulations.Question + ",by:" + consulations.patient.user.UserName + " " + consulations.patient.user.LastName, "info");
                test2.addNotifications("admin");
                return RedirectToAction("Index");
            }
            return View(consulations);
        }


        /// <summary>
        /// Edit
        /// </summary>
        [Authorize(Roles = "Doctor,patient")]
        public ActionResult Edit(int? consulationID)
        {
            if (consulationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            consulations consulations = db.myConsulations.Find(consulationID);
            if (consulations == null)
            {
                return HttpNotFound();
            }

            List<string> items = db.myDoctors.Select(x => x.specialty).ToList();
            if (items.Count > 0)
            {
                List<SelectListItem> DoctorListItem = new List<SelectListItem>();
                for (int i = 0; i < items.Count; i++)
                {
                    DoctorListItem.Add(new SelectListItem { Selected = false, Text = items[i], Value = items[i] });
                }
                ViewBag.DoctorListItem = new SelectList(DoctorListItem, "Value", "Text");
            }
            else
                ViewBag.DoctorListItem = null;
           
            return View(consulations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Doctor,patient")]
        public ActionResult Edit([Bind(Include = "consulationID,title,Question,reply,Isreplaied,specialty,PathImage,DateOfconsulation,DateOfreply,doctor,patient")] consulations consulations, string OldSpecialty, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                
                if (consulations.reply != null && consulations.Isreplaied == false)
                {
                    consulations.DateOfreply = DateTime.Now;
                    consulations.Isreplaied = true;
                    sendemail(consulations.patient.user.Email, "replayed", "dotor replayed on your consulation : " + consulations.reply + "</br><a href='" + HttpContext.Request.Url.AbsoluteUri.Replace("/Edit", "") + "#consulation_" + consulations.consulationID + "'> click </a> to read ");

                    Notifications test = new Notifications(consulations.consulationID.ToString(), false,"doctor (" + consulations.doctor.user.UserName + " " + consulations.doctor.user.LastName + ") reply:" + consulations.reply,"info");
                    test.addNotifications(consulations.patient.user.UserName);
                }
                if (!consulations.specialty.Equals(OldSpecialty))
                {
                    var  tempDoctor= db.myDoctors.
                        Include(m=>m.consulation).
                        Where(s => s.specialty.Equals(consulations.specialty) || s.specialty.ToLower().Equals("all")).
                            Select(s => new{
                                Item = s,
                                Count = s.consulation.Count()}).
                        OrderBy(s => s.Item.specialty.Contains("all")).
                                ThenBy(s => s.Count).
                        ThenBy(s => s.Item.evaluate).
                        FirstOrDefault();
                    if (tempDoctor != null)
                    {
                        var oldDoctorName = consulations.doctor.user.UserName;
                        consulations.doctor = tempDoctor.Item;
                        consulations.DoctorName = consulations.doctor.user.UserName + " " + consulations.doctor.user.UserName;
                        Notifications test = new Notifications(consulations.consulationID.ToString() , false, "need to reply:" + consulations.Question + ",by:" + consulations.patient.user.UserName + " " + consulations.patient.user.LastName,"info");
                        test.addNotifications(consulations.doctor.user.UserName);
                        test.removeNotification(oldDoctorName);
                    }
                }
                if (upload != null)
                {
                    string oldFile = Server.MapPath("~/" + consulations.PathImage);
                    FileInfo file = new FileInfo(oldFile);
                    if (file.Exists)
                        file.Delete();
                    string fileName = consulations.consulationID + "." + upload.FileName.Substring(upload.FileName.IndexOf(".") + 1);
                    consulations.PathImage = "/images/consulations/" + fileName;
                    string path = Path.Combine(Server.MapPath("~/images/consulations"), fileName);
                    upload.SaveAs(path);
                }
                db.Entry(consulations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consulations);
        }


        public bool sendemail(string email, string Subject, string message)
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




        /// <summary>
        /// Delete
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? consulationID)
        {
            if (consulationID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            consulations consulations = db.myConsulations.Find(consulationID);
            if (consulations == null)
            {
                return HttpNotFound();
            }
            return View(consulations);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int consulationID)
        {
            consulations consulations = db.myConsulations.Find(consulationID);
            db.myConsulations.Remove(consulations);
            db.SaveChanges();

            FileInfo file = new FileInfo(Server.MapPath("~/" + consulations.PathImage));
            if (file.Exists)
            { file.Delete(); }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
