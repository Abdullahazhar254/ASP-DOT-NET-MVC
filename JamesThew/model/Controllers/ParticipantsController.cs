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
    public class ParticipantsController : Controller
    {
        private DefaultConnectionEntities db = new DefaultConnectionEntities();

        // GET: Participants
        public ActionResult Index()
        {
           // var contestlist = db.Contests.ToList();
           var participants = db.Participants.Include(p => p.AspNetUser).Include(p => p.Contest);
            return View(participants);
        }

        // GET: Participants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);
        }
        public ActionResult CreateP(int? Id)
        {
            //String name = db.Contests.Where(x => x.Id == Id).Select(x => x.Name).SingleOrDefault();
           // ViewBag.Contest = Name;
            // Participant participant = db.Participants.Find(id);
            ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.Contest_Id = new SelectList(db.Contests.Where(x => x.Id == Id), "Id", "Name");
           
            
            return View();
        }

        // GET: Participants/Create
        //[HttpPost]
        //public ActionResult CreateP(Contest contest)
        //{
        //    ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email");
        //    ViewBag.Contest_Id = new SelectList(db.Contests, "Id", "Name");
        //    return View();
        //}

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateP([Bind(Include = "Id,Contest_Id,User_Id,User_Name,Ingredients,Description")] Participant participant)
        {
            if (ModelState.IsValid)
            {
                db.Participants.Add(participant);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email", participant.User_Id);
            ViewBag.Contest_Id = new SelectList(db.Contests, "Id", "Image", participant.Contest_Id);
            return RedirectToAction("Index","Home");
        }

        // GET: Participants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email", participant.User_Id);
            ViewBag.Contest_Id = new SelectList(db.Contests, "Id", "Image", participant.Contest_Id);
            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Contest_Id,User_Id,User_Name,Ingredients,Description")] Participant participant)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(participant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_Id = new SelectList(db.AspNetUsers, "Id", "Email", participant.User_Id);
            ViewBag.Contest_Id = new SelectList(db.Contests, "Id", "Image", participant.Contest_Id);
            return View(participant);
        }

        // GET: Participants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participant participant = db.Participants.Find(id);
            if (participant == null)
            {
                return HttpNotFound();
            }
            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participant participant = db.Participants.Find(id);
            db.Participants.Remove(participant);
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
        public ActionResult AnnounceWinner(int id)
        {
            var partAnnounce = db.Participants.Where(x => x.Contest_Id == id).ToList();
            return View("Index",partAnnounce);
        }
    }
}
