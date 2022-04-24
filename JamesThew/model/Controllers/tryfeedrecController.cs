using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using model;

namespace model.Controllers
{
    public class tryfeedrecController : Controller
    {
        private DefaultConnectionEntities db = new DefaultConnectionEntities();

        // GET: tryfeedrec
        public ActionResult Index()
        {
            return View(db.Feedback_Recipe.ToList());
        }

        // GET: tryfeedrec/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback_Recipe feedback_Recipe = db.Feedback_Recipe.Find(id);
            if (feedback_Recipe == null)
            {
                return HttpNotFound();
            }
            return View(feedback_Recipe);
        }

        // GET: tryfeedrec/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tryfeedrec/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Feedback")] Feedback_Recipe feedback_Recipe)
        {
            if (ModelState.IsValid)
            {
                db.Feedback_Recipe.Add(feedback_Recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feedback_Recipe);
        }

        // GET: tryfeedrec/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback_Recipe feedback_Recipe = db.Feedback_Recipe.Find(id);
            if (feedback_Recipe == null)
            {
                return HttpNotFound();
            }
            return View(feedback_Recipe);
        }

        // POST: tryfeedrec/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Feedback")] Feedback_Recipe feedback_Recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback_Recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback_Recipe);
        }

        // GET: tryfeedrec/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback_Recipe feedback_Recipe = db.Feedback_Recipe.Find(id);
            if (feedback_Recipe == null)
            {
                return HttpNotFound();
            }
            return View(feedback_Recipe);
        }

        // POST: tryfeedrec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback_Recipe feedback_Recipe = db.Feedback_Recipe.Find(id);
            db.Feedback_Recipe.Remove(feedback_Recipe);
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
