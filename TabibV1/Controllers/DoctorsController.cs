using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TabibV1.Models;
using TabibV1.OtherClass;

namespace TabibV1.Controllers
{
    [AllowCrossSiteJsonAttribute]
    public class DoctorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Doctors/
        public ActionResult Index(int page = 1, string username = "", string lastname = "", int evelate = -1, string specialty = "all", string address="")
        {

            var mydoctors = db.myDoctors.Include(d => d.user).Where(e => ((evelate == -1) ? true : e.evaluate.Equals(evelate)) && ((username.Equals("")) ? true : (e.user.UserName).Contains(username)) && ((lastname.Equals("")) ? true : (e.user.LastName).Contains(lastname)) && (address.Equals("") ? true : e.address.textAddress.Equals(address)) && (specialty.Equals("all") ? true : e.specialty.Equals(specialty)));
            ViewBag.numberPage = Math.Ceiling(Convert.ToDouble(mydoctors.Count() / 12.0));
            ViewBag.CurrentPage = page;
            ViewBag.Contoller = "Doctors";
            string[] search={username,lastname,(evelate>-1)?evelate.ToString():"",specialty,address};

            ViewBag.search = search;
            

            int firstItem = (page > 0) ? ((page - 1) * 12) : 0;// page range [1-lenght/3(+1 if ,n)]
            int numberItem = (page > 0) ? Math.Min(12, (mydoctors.Count() > 0) ? mydoctors.Count() - firstItem : 0) : Math.Min(12, mydoctors.Count()); //db.myArticals.Count() - Math.Min(page * 3, numberItem)
            var mydoctorsResult = mydoctors.ToList().GetRange(firstItem, numberItem);
            
            var all="[ ";
            foreach (var ele in mydoctorsResult)
            {   if (ele.address.lang != null && ele.address.lat!=null)
                    all = all + "{\"mess\":\"" + ele.user.UserName + " " + ele.user.LastName + "    " + ele.specialty + "\",\"lang\":" + ele.address.lang + ",\"lat\":" + ele.address.lat + "},";
            }
            ViewBag.Loactions =new HtmlString(all.Remove(all.Length-1)+ "]");

            return View(mydoctorsResult);
        }

        // GET: /Doctors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctors doctors = db.myDoctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // GET: /Doctors/Create
        public ActionResult Create()
        {
           return View();
        }

        // POST: /Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Log]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin")]
        public ActionResult Create([Bind(Include = "Email,UserName,LastName,DateBirthday,evaluate,specialty,firstFrom,firstTo,secondFrom,secondTo,Days,lat,lang,textAddress")] DoctorViewModel doctors, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                
                var newDoctorAppointment = new DoctorAppointment() { Days = doctors.GetDays(), firstFrom = doctors.firstFrom, firstTo = doctors.firstTo, secondFrom = doctors.secondFrom, secondTo = doctors.secondTo };
                var newAddress = new Address() { lang = doctors.lang, lat = doctors.lat, textAddress = doctors.textAddress, Location = DbGeography.FromText("POINT("+doctors.lat+" "+doctors.lang+")") };
                string  fileName = doctors.UserName + "_" + doctors.LastName + "." + upload.FileName.Substring(upload.FileName.IndexOf(".") + 1);
                string path = Path.Combine(Server.MapPath("~/images/doctors"), fileName);   //id
                var newuser = new ApplicationUser() { Email = doctors.Email, UserName = doctors.UserName, LastName = doctors.LastName, DateBirthday = doctors.DateBirthday, PathOfImage = "/images/doctors/" + fileName };
                
               var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
               
                var result =  UserManager.Create(newuser,newuser.UserName);
                if (result.Succeeded)
                {
                    var newDoctor = new doctors()
                    {
                        user = newuser,// db.Users.Where(x=>x.Email.Equals(doctors.Email)).FirstOrDefault(),
                        evaluate = doctors.evaluate,
                        specialty = (doctors.specialty != "all") ? doctors.specialty : "",
                        DoctorAppointments = newDoctorAppointment,
                        address = newAddress
                    };
                    db.myDoctors.Add(newDoctor);
                    db.SaveChanges();

                    UserManager.AddToRole(newDoctor.user.Id, "Doctor");
                    upload.SaveAs(path);

                    return RedirectToAction("Index");
                }
            }

            return View(doctors);
        }
        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    //filterContext.Result = RedirectToAction("Error", "InternalError");

        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/Sahred/Error.cshtml"
        //    };

        //    base.OnException(filterContext);
        //}
        // GET: /Doctors/Edit/5
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctors doctors = db.myDoctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            var docview = new DoctorViewModel() { doctorsId = doctors.doctorsId, UserName = doctors.user.UserName, LastName = doctors.user.LastName, PathOfImage = doctors.user.PathOfImage, specialty = doctors.specialty, evaluate = doctors.evaluate, Days = doctors.DoctorAppointments.GetDays(), firstFrom = doctors.DoctorAppointments.firstFrom, firstTo = doctors.DoctorAppointments.firstTo, secondFrom = doctors.DoctorAppointments.secondFrom, secondTo = doctors.DoctorAppointments.secondTo, textAddress = doctors.address.textAddress, lang = doctors.address.Location.Longitude.ToString(), lat = doctors.address.Location.Latitude.ToString() };//doctors.address.lang doctors.address.lat
            return View(docview);
        }

        // POST: /Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult Edit([Bind(Include = "doctorsId,UserName,LastName,PathOfImage,evaluate,specialty,firstFrom,firstTo,secondFrom,secondTo,Days,lat,lang,textAddress")] DoctorViewModel doctors, HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {
                var oldDoctor = db.myDoctors.Include(i => i.user).Include(i => i.address).Include(i => i.DoctorAppointments).Where(x => x.doctorsId == doctors.doctorsId).FirstOrDefault();
                oldDoctor.user.UserName = doctors.UserName;
                oldDoctor.user.LastName = doctors.LastName;
                oldDoctor.evaluate = doctors.evaluate;
                oldDoctor.specialty = doctors.specialty;
                oldDoctor.address.lang = doctors.lang;
                oldDoctor.address.lat = doctors.lat;
                oldDoctor.address.Location = DbGeography.FromText("POINT("+doctors.lat+" "+doctors.lang+")");
                oldDoctor.address.textAddress = doctors.textAddress;
                oldDoctor.DoctorAppointments.Days = doctors.GetDays();
                oldDoctor.DoctorAppointments.firstFrom = doctors.firstFrom;
                oldDoctor.DoctorAppointments.firstTo = doctors.firstTo;
                oldDoctor.DoctorAppointments.secondFrom = doctors.secondFrom;
                oldDoctor.DoctorAppointments.secondTo = doctors.secondTo;
                
                if (upload != null)
                {
                    string oldFile = Server.MapPath("~/" + doctors.PathOfImage);
                    FileInfo file = new FileInfo(oldFile);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    string fileName = doctors.UserName + "_" + doctors.LastName + "." + upload.FileName.Substring(upload.FileName.IndexOf(".") + 1);
                    string path = Path.Combine(Server.MapPath("~/images/doctors"), fileName);   //id
                    upload.SaveAs(path);
                    oldDoctor.user.PathOfImage = "/images/doctors/" + fileName;
                }

                //db.Entry(oldDoctor).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(doctors);
        }

        // GET: /Doctors/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctors doctors = db.myDoctors.Find(id);
            if (doctors == null)
            {
                return HttpNotFound();
            }
            return View(doctors);
        }

        // POST: /Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string doctorsId)
        {
            doctors doctors = db.myDoctors.Include(x=>x.user).Where(e=>e.doctorsId==doctorsId).FirstOrDefault();
            
            db.myAddress.Remove(doctors.address);  
            db.myDoctorAppointment.Remove(doctors.DoctorAppointments);  
            db.myAppointments.RemoveRange(doctors.appointment);
            
            FileInfo file = new FileInfo(Server.MapPath("~/" + doctors.user.PathOfImage));
            if (file.Exists)
            { file.Delete(); } 
            db.Users.Remove(doctors.user);
            db.myDoctors.Remove(doctors);

            db.SaveChanges();
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
