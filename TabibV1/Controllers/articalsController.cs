using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TabibV1.Models;

namespace TabibV1.Controllers
{
    [RequireHttps]
    public class articalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /articals/
        public ActionResult Index(string category = "", string search = "", int page = 1)
        {
            var result = db.myArticals.Where(e => (category.Equals("") ? true : e.typeOfarticls.Equals(category)) && (search.Equals("") ? true : e.TextArticles.Contains(search))).ToList();
            
            ViewBag.allcategory = db.myArticals.Select(e => e.typeOfarticls).Distinct().ToList();
            ViewBag.CurrentPage = page;
            ViewBag.search = search;
            ViewBag.category = category;
            ViewBag.Contoller="articals";
            int firstItem = (page > 0) ? ((page - 1) * 3) : 0;// page range [1-lenght/3(+1 if ,n)]
            int numberItem = (page > 0) ? Math.Min(3, (result.Count() > 0) ? result.Count() - firstItem : 0) : Math.Min(3, result.Count()); //db.myArticals.Count() - Math.Min(page * 3, numberItem)
            ViewBag.numberPage = Math.Ceiling(Convert.ToDouble(result.Count() / 3.0));


            if (Session["notification"] == null) // to get number consultion need to reply ()
            {
                if (User.IsInRole("Admin"))  // to admin all consultion not have reply
                    Session["notification"] = db.myConsulations.Where(x => x.Isreplaied == false).Count();
                else  // to user all consultion by this user need to reply
                    Session["notification"] = db.myConsulations.Include(x => x.patient).Where(x => x.Isreplaied == false
                        && x.patient.user.UserName.Equals(User.Identity.Name)).Count();
            }

            return View(result.GetRange(firstItem, numberItem));

        }

        // GET: /articals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articals articals = db.myArticals.Find(id);
            if (articals == null)
            {
                return HttpNotFound();
            }
            return View(articals);
        }

        // GET: /articals/Create
        [Authorize(Roles = "Admin")] 
        public ActionResult Create()
        {
            return View();
        }

        // POST: /articals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "articalsID,titleOfarticls,typeOfarticls,TextArticles,PathOfImage,DateOfarticals,articlWriter")] articals articals, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {


                string path = Path.Combine(Server.MapPath("~/Images"), upload.FileName);
                upload.SaveAs(path);
                articals.PathOfImage = "/Images/" + upload.FileName;
                articals.DateOfarticals = DateTime.Now;
                

                db.myArticals.Add(articals);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(articals);
        }

        // GET: /articals/Edit/5
        [Authorize(Roles = "Admin")] 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articals articals = db.myArticals.Find(id);
            if (articals == null)
            {
                return HttpNotFound();
            }
            return View(articals);
        }

        // POST: /articals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "articalsID,titleOfarticls,typeOfarticls,TextArticles,PathOfImage,DateOfarticals,articlWriter")] articals articals, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {


                if (upload != null)
                { //delete old image before add new
                    string oldFile = Server.MapPath("~/" + articals.PathOfImage);
                    FileInfo file = new FileInfo(oldFile);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    string path = Path.Combine(Server.MapPath("~/Images"), upload.FileName);
                    upload.SaveAs(path);
                    articals.PathOfImage = "/Images/" + upload.FileName;
                }
                




                db.Entry(articals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articals);
        }

        // GET: /articals/Delete/5
        [Authorize(Roles = "Admin")] 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            articals articals = db.myArticals.Find(id);
            if (articals == null)
            {
                return HttpNotFound();
            }
            return View(articals);
        }

        // POST: /articals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int articalsID)
        {

            articals articals = db.myArticals.Find(articalsID);

            string oldFile = Server.MapPath("~/" + articals.PathOfImage);
            FileInfo file = new FileInfo(oldFile);
            if (file.Exists)
            { file.Delete(); } 

          
            db.myArticals.Remove(articals);
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
