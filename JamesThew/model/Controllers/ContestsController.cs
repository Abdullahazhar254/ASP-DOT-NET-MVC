using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using model;

namespace model.Controllers
{
    public class ContestsController : Controller
    {
        private DefaultConnectionEntities db = new DefaultConnectionEntities();

        // GET: Contests
        public ActionResult Index()
        {
            List<Contest> li;
            //var date = DateTime.Now.Date;
            ChangeStatus();
            if (!User.IsInRole("admin"))
            {
               li = db.Contests.Where(y => y.Status == "active").ToList();
            }
            else
            {
                li = db.Contests.Where(y => y.Status == "inactive").ToList();
            }
            return View(li);
        }
        
        public ActionResult IndexP(int? id)
        {
            //ViewBag.Contest = contest.Name;
            return RedirectToAction("CreateP", "Participants",new { Id=id});
            //return View("../Views/Participants/CreateP",new { Name=contest.Name});
        }

        // GET: Contests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // GET: Contests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contest contest, HttpPostedFileBase file)
        {
            var now = DateTime.Now.Date;
            if(contest.Ending_Date<now)
            {
                contest.Status = "inactive";

            }
            else
            {
                contest.Status = "active";
            }
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Content/images/" + ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);

                }

                db.Contests.Add(contest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contest);
        }

        // GET: Contests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Name,Description,Starting_Date,Ending_Date,Winner_Id")] Contest contest,string id)
        {
            contest.Winner_Id = id;
            if (ModelState.IsValid)
            {
                db.Entry(contest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contest);
        }

        public ActionResult Announcement()
        {
           List<Contest> li =db.Contests.Where(x => x.Winner_Id != null).ToList();
            List<string> cName = new List<string>();
            List<string> winnerName = new List<string>();
           foreach(var item in li)
            {
                cName.Add(item.Name);
                winnerName.Add(db.AspNetUsers.Where(x => x.Id == item.Winner_Id).Select(x => x.UserName).SingleOrDefault());
            }
            ViewBag.CName = cName;
            ViewBag.WName = winnerName;
            return View();
        }

            public ActionResult Announced(int id)
        {
            String p = db.Participants.Where(x => x.Id == id).Select(x => x.User_Id).SingleOrDefault();
            Contest c = db.Contests.Where(x => x.Id == id).SingleOrDefault();
            c.Winner_Id = p;
            if (ModelState.IsValid)
            {
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Announcement");
            }
            return View("Announcement");
        }

        // GET: Contests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contest contest = db.Contests.Find(id);
            if (contest == null)
            {
                return HttpNotFound();
            }
            return View(contest);
        }

        // POST: Contests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contest contest = db.Contests.Find(id);
            db.Contests.Remove(contest);
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
        public void ChangeStatus()
        {
            var now = DateTime.Now.Date;
            List<Contest> li=db.Contests.ToList();
            foreach(var item in li)
            {
                if(item.Ending_Date<=now)
                {
                    item.Status = "inactive";
                    db.SaveChanges();
                }
            }
        }
        

    }
}
