using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlfaAccounting.Models;
/// <summary>
/// Name:Mie Tanaka
/// Name:26/05/2017
/// Description: allows users to create, view, edit, delete categories

namespace AlfaAccounting.Controllers
{

    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns Categories/Index view filled with a list of all categories
        /// </summary>
        /// <returns> returns list of categories saved on database </returns>
        /// <includesource>yes</includesource>
        public ActionResult Index()
        {
                return View(db.Categories.ToList());
        }


        /// <summary>
        /// Returns a blank Categories/Create view of category
        /// </summary>
        /// <returns></returns>
        /// <includesource>yes</includesource>
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// returns Categories/Index view after input data got updated in the database
        /// if error return Categories/create view with current category data
        /// </summary>
        /// <param name="category"></param>
        /// <returns> Category/Index </returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        /// <summary>
        /// Returns Categories/Edit view filled with previously selected category name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <includesource>yes</includesource>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        /// <summary>
        /// Returns Categories/Index View after updating changed data on database
        /// if error returns current view
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Returns Categories/Index View</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        /// <summary>
        /// Returns category index view after deleting selected category name on click of confirming delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Categories/Index</returns>
        /// <includesource>yes</includesource>
        public ActionResult Delete(int id)
        {
            try { 
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                ModelState.AddModelError("customError", "Error: " + ex);
                return RedirectToAction("Index");
            }
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
