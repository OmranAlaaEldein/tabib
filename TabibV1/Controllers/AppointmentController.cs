using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TabibV1.Models;
using Microsoft.AspNet.Identity;
using TabibV1.OtherClass;

namespace TabibV1.Controllers
{
    public class AppointmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Appointment/
        public ActionResult Index()
        {
            //var userID = User.Identity.GetUserId(); 

            ViewBag.time = GetTimeOfDoctors();
            return View(db.myAppointments.ToList());
        }

        public  Dictionary<string, SelectList> GetTimeOfDoctors(){

            Dictionary<string, SelectList> time = new Dictionary<string, SelectList>();
            var t = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            
            var Appointments = db.myAppointments.Include(r => r.doctor).Where(r => DbFunctions.TruncateTime(r.appointmentsTime) > t).ToList(); // without diplucate doctor
            
            foreach (var item in Appointments)
            {
                if (! time.Keys.Contains(item.doctor.doctorsId)) {
                    DoctorAppointment myTime = db.myDoctors.Where(y => y.doctorsId ==item.doctor.doctorsId).Select(y=> y.DoctorAppointments).FirstOrDefault();
                    List<appointments> myAppointments = Appointments.Where(xx => xx.doctor.doctorsId == item.doctor.doctorsId && xx.appointmentsTime.Date ==item.appointmentsTime.Date).ToList();

                    List<SelectListItem> avaliableTime = new List<SelectListItem>();
                    avaliableTime.Add(new SelectListItem { Selected = true, Text = item.appointmentsTime.TimeOfDay.ToString(), Value = item.appointmentsTime.TimeOfDay.ToString() });

                    for (int k = 0; myTime.firstFrom.AddMinutes(k * 30).TimeOfDay < myTime.firstTo.TimeOfDay; k++)
                    {
                        if (!myAppointments.Any(xx => xx.appointmentsTime.TimeOfDay.Equals(myTime.firstFrom.AddMinutes(k * 30).TimeOfDay)))
                        {
                            avaliableTime.Add(new SelectListItem { Selected = false, Text = myTime.firstFrom.AddMinutes(k * 30).TimeOfDay.ToString(), Value = myTime.firstFrom.AddMinutes(k * 30).TimeOfDay.ToString() });
                        }
                    }

                    for (int k = 0; myTime.secondFrom.AddMinutes(k * 30).TimeOfDay < myTime.secondFrom.TimeOfDay; k++)
                    {
                        if (!myAppointments.Any(xx => xx.appointmentsTime.TimeOfDay.Equals(myTime.secondFrom.AddMinutes(k * 30).TimeOfDay)))
                        {
                            avaliableTime.Add(new SelectListItem { Selected = false, Text = myTime.secondFrom.AddMinutes(k * 30).TimeOfDay.ToString(), Value = myTime.secondFrom.AddMinutes(k * 30).TimeOfDay.ToString() });
                        }
                    }
                    if (avaliableTime.Count > 0)
                        time.Add(item.doctor.doctorsId.ToString(), new SelectList(avaliableTime, "Value", "Text"));
                    }
            }
            return time;
        }


        // GET: /Appointment/Create
        public ActionResult Create()
        {
            Dictionary<string, Dictionary<string, string>> Doctors = new Dictionary<string, Dictionary<string, string>>();
            List<string> specialties = db.myDoctors.Select(x=>x.specialty).Distinct().ToList();
            List<SelectListItem> DoctorListItem = new List<SelectListItem>();
                
            for (int i = 0; i < specialties.Count();i++ ){
                DoctorListItem.Add(new SelectListItem { Selected = false, Text = specialties[i], Value = specialties[i] });

                string aa = specialties[i];
                Dictionary<string, string> temp = db.myDoctors.Where(x => x.specialty.Equals(aa)).ToDictionary(x => x.doctorsId, x => x.user.UserName + " " + x.user.LastName);
                Doctors.Add(specialties[i], temp);
            }

            ViewBag.Doctors = Doctors;
            ViewBag.specialties = new SelectList(DoctorListItem, "Value", "Text");
           
            return View();
        }

        // POST: /Appointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "appointmentsId,DoctorName,Question,Note,evaluation,appointmentsTime,doctor")] appointments appointments, DateTime Days, TimeSpan Time)
        {
            if (ModelState.IsValid)
            {
                appointments.doctor = db.myDoctors.Where(x => x.doctorsId == appointments.doctor.doctorsId).FirstOrDefault();
                appointments.DoctorName = appointments.doctor.user.UserName + " " + appointments.doctor.user.LastName; //db.myDoctors.Where(x => x.doctorsId == appointments.doctor.doctorsId).Select(x => x.user.UserName + " " + x.user.LastName).FirstOrDefault();
                appointments.patient = db.myPatients.Where(x => x.user.UserName.Equals(User.Identity.Name)).FirstOrDefault();  //??
                appointments.appointmentsTime = new DateTime(Days.Year, Days.Month, Days.Day, Time.Hours, Time.Minutes, Time.Seconds);
                appointments.evaluation = 1;

                db.myAppointments.Add(appointments);
                db.SaveChanges();

                Notifications test2 = new Notifications(appointments.appointmentsId.ToString(), false, "new appointments at " + appointments.appointmentsTime + ",by:" + appointments.patient.user.UserName + " " + appointments.patient.user.LastName, "info");
                test2.addNotifications(appointments.doctor.user.UserName);
                return RedirectToAction("Index");
            }

            return View(appointments);
        }

        // GET: /Appointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointments appointments = db.myAppointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: /Appointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="appointmentsId,DoctorName,Question,Note,evaluation,appointmentsTime")] appointments appointments,string Time)
        {
            if (ModelState.IsValid)
            {
                DateTime t= Convert.ToDateTime(Time);
                appointments.appointmentsTime = new DateTime(appointments.appointmentsTime.Year, appointments.appointmentsTime.Month, appointments.appointmentsTime.Day, t.Hour, t.Minute, t.Second);
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();

                Notifications test2 = new Notifications(appointments.appointmentsId.ToString(), false, "edit appointments at " + appointments.appointmentsTime + ",by:" + appointments.patient.user.UserName + " " + appointments.patient.user.LastName, "info");
                test2.addNotifications(appointments.doctor.user.UserName);
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index"); // error ?? 
        }

        // GET: /Appointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointments appointments = db.myAppointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: /Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int appointmentsId)
        {
            appointments appointments = db.myAppointments.Find(appointmentsId);
            db.myAppointments.Remove(appointments);
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
