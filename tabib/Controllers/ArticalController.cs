using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tabib.Models;
using System.IO;
namespace tabib.Controllers
{
    public class ArticalController : Controller
    {
        private UsersContext db = new UsersContext();

        public ActionResult Index()     // GET: /artical/
        {   int count = 0; List<artical> result;
            using (db)
            {   if (db.Visiting.Count() > 0)  //if there is data
                {   // get number of visitin by this user
                    Visiting User = db.Visiting.Where(x => x.IpAddressVisitor.Equals(Request.UserHostAddress)).FirstOrDefault ();
                    if (User == null) { // if new user set count = 0
                        Visiting newUSer = new Visiting { IpAddressVisitor = Request.UserHostAddress, countVisit = 0 };
                        db.Visiting.Add(newUSer);
                    }else { //if old user plus one
                        User.countVisit = User.countVisit + 1;
                        count = User.countVisit; }
                } else                { //if this user is first user
                    Visiting newUSer = new Visiting { IpAddressVisitor = Request.UserHostAddress, countVisit = 0 };
                    db.Visiting.Add(newUSer);
                }
                db.SaveChanges();
                List<artical> array = db.DocArticals.ToList();
                result = array;
                if (array.Count > 0)  //if there is articals so reorder artical by reorderArtical function 
                { result = reorderArtical((count > array.Count) ? array.Count - 1 : count, array);                
                }if (Session["notification"]==null) // to get number consultion need to reply ()
                {   if (User.Identity .Name .Equals ("admin"))  // to admin all consultion not have reply
                        Session["notification"] = db.CusConsulation.Where(x => x.Isreplaied == false).Count();
                    else  // to user all consultion by this user need to reply
                        Session["notification"] = db.CusConsulation.Include(x => x.UserProfile).Where(x => x.Isreplaied == false
                            && x.UserProfile.UserName.Equals(User.Identity.Name)).Count(); }
            } return View(result);  }

        //Reorder articles by starting from the old article with a specific degree 
        //determined by the number of times this user visits the site
        public List<artical> reorderArtical (int index,List<artical> myArtical) {
            List<artical> newArray = new List<artical> (); 
            
            newArray.AddRange(myArtical.GetRange(index, myArtical.Count -index));
            myArtical.RemoveRange(index, myArtical.Count - index);
                
            newArray.AddRange(myArtical.GetRange(0, myArtical.Count ));
            myArtical.RemoveRange(0, myArtical.Count );


            return newArray;
        }





         [Authorize(Roles = "adminstration")]  // GET: /artical/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
          [Authorize(Roles = "adminstration")]  // POST: /artical/Create
        [ValidateAntiForgeryToken]
        public ActionResult Create(artical artical, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {//save image in server
                string path = Path.Combine(Server.MapPath("~/Images"), upload.FileName);
                upload.SaveAs(path);
                artical.PathOfImage = "/Images/" + upload.FileName;

                artical.DateOfarticals = DateTime.Now;
                db.DocArticals.Add(artical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artical);
        }


            [Authorize(Roles = "adminstration")]   // GET: /artical/Edit/5
        public ActionResult Edit(int id = 0)
        {
            artical artical = db.DocArticals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }


        [HttpPost]
            [Authorize(Roles = "adminstration")]  // POST: /artical/Edit/5
        [ValidateAntiForgeryToken]
        public ActionResult Edit(artical artical, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload!=null)  
                { //delete old image before add new
                    string oldFile = Server.MapPath("~/" + artical.PathOfImage);
                    FileInfo file = new FileInfo(oldFile);
                    if (file.Exists)
                    {   file.Delete();
                        } 
                    
                    string path = Path.Combine(Server.MapPath("~/Images"), upload.FileName);
                    upload.SaveAs(path);
                    artical.PathOfImage = "/Images/" + upload.FileName;
                }
                

                db.Entry(artical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artical);
        }



         [Authorize(Roles = "adminstration")]   // GET: /artical/Delete/5
        public ActionResult Delete(int id = 0)
        {
            artical artical = db.DocArticals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }

        [HttpPost, ActionName("Delete")]
         [Authorize(Roles = "adminstration")]  // POST: /artical/Delete/5
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {  //delete image before delte artical
            artical artical = db.DocArticals.Find(id);
            string oldFile = Server.MapPath("~/" + artical.PathOfImage);
            FileInfo file = new FileInfo(oldFile);
            if (file.Exists)
            { file.Delete();} 
                    
                   
            db.DocArticals.Remove(artical);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        



        /////////////////////////////////////
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult Details(int id = 0)  // GET: /artical/Details/5
        {
            artical artical = db.DocArticals.Find(id);
            if (artical == null)
            {
                return HttpNotFound();
            }
            return View(artical);
        }
    }
}