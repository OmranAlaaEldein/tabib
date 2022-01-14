using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TabibV1.Models;

namespace TabibV1.Controllers
{
    public class patientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /patient/
        public ActionResult Index()
        {
            List<patients> result = null;
            if (User.IsInRole("Admin"))
            {
                result = db.myPatients.Include(p => p.user).ToList();
            }
            if (User.IsInRole("patient"))
            {
                result = db.myPatients.Include(p => p.user).Where(p=>p.user.UserName==User.Identity.Name).ToList();
            }
            if (User.IsInRole("Doctor"))
            {
                result = db.myConsulations.Where(p => p.doctor.user.UserName == User.Identity.Name).Select(p=>p.patient).ToList();
                result.AddRange(db.myConsulations.Where(p => p.doctor.user.UserName == User.Identity.Name).Select(p => p.patient).ToList());
                result.Distinct();
            }

            var mypatients = db.myPatients.Include(p => p.user);
            return View(mypatients.ToList());
        }

        // GET: /patient/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patients patients = db.myPatients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        // GET: /patient/Create
        public ActionResult Create()
        {
            //ViewBag.patientsId = new SelectList(db.IdentityUsers, "Id", "UserName");
            return View();
        }

        // POST: /patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="patientsId,countVisit")] patients patients)
        {
            if (ModelState.IsValid)
            {
                db.myPatients.Add(patients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.patientsId = new SelectList(db.IdentityUsers, "Id", "UserName", patients.patientsId);
            return View(patients);
        }

        // GET: /patient/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patients patients = db.myPatients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            //ViewBag.patientsId = new SelectList(db.IdentityUsers, "Id", "UserName", patients.patientsId);
            return View(patients);
        }

        // POST: /patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="patientsId,countVisit")] patients patients)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patients).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.patientsId = new SelectList(db.IdentityUsers, "Id", "UserName", patients.patientsId);
            return View(patients);
        }

        // GET: /patient/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patients patients = db.myPatients.Find(id);
            if (patients == null)
            {
                return HttpNotFound();
            }
            return View(patients);
        }

        // POST: /patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            patients patients = db.myPatients.Find(id);
            db.myPatients.Remove(patients);
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
