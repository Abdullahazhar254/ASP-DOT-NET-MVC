using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using model;

namespace model.Controllers
{
    public class RecipesController : Controller
    {
        private DefaultConnectionEntities db = new DefaultConnectionEntities();

        // GET: Recipes
        public ActionResult Index()
        {
             var recipes = db.Recipes.Include(r => r.AspNetRole).Include(r => r.AspNetUser);
            return View(recipes.ToList());
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }
        
        // GET: Recipes/Create
        public ActionResult Create(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {

                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/Content/images/" + ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);

                }

                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Label = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.Posted_by = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Recipe_Id,Recipe_Name,Description,Ingredients,Label,Posted_by")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                

                db.Recipes.Add(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Label = new SelectList(db.AspNetRoles, "Id", "Name", recipe.Label);
            ViewBag.Posted_by = new SelectList(db.AspNetUsers, "Id", "Email", recipe.Posted_by);
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.Label = new SelectList(db.AspNetRoles, "Id", "Name", recipe.Label);
            ViewBag.Posted_by = new SelectList(db.AspNetUsers, "Id", "Email", recipe.Posted_by);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Recipe_Id,Recipe_Name,Description,Ingredients,Label,Posted_by")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Label = new SelectList(db.AspNetRoles, "Id", "Name", recipe.Label);
            ViewBag.Posted_by = new SelectList(db.AspNetUsers, "Id", "Email", recipe.Posted_by);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
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
        public ActionResult Feedback()
        {
            return PartialView("_Feedback");
        }

        public PartialViewResult _Feedback()
        {
            //Feedback_Recipe fr = new Feedback_Recipe();
            ViewBag.Label = new SelectList(db.AspNetRoles, "Id", "Name");
            ViewBag.Posted_by = new SelectList(db.AspNetUsers, "Id", "Email");

            return PartialView();
        }

        //public PartialViewResult _Comment()
        //{


        //    return PartialView();
        //}

    }
}
