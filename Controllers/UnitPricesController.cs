using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlfaAccounting.Models;

namespace AlfaAccounting.Controllers
{/// <summary>
/// Mie Tanaka
/// 26/05/2017
/// Varsion 0.0
/// </summary>

    public class UnitPricesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns UnitPrices/Index view filled with a list of all categories
        /// </summary>
        /// <returns> returns list of UnitPrices saved on database </returns>
        public ActionResult Index()
        {
            return View(db.UnitPrices.ToList());
        }


        // GET: UnitPrices/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}


        /// <summary>
        /// returns UnitPrices/Index view after input data got updated in the database
        /// if error return UnitPrices/create view with current category data
        /// </summary>
        /// <param name="unitPrice"></param>
        /// <returns> UnitPrices/Index </returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UnitPriceId,UnitPriceValue,UnitPriceDescription")] UnitPrice unitPrice)
        {
            
            if (unitPrice.UnitPriceValue == 0)
            {
                ViewBag.Error = "Unit Price to be filled in";
                return View(db.UnitPrices.ToList());
            }
            
            if (unitPrice.UnitPriceDescription == null)
            {
                ViewBag.Error = "Unit Description to be filled in";
                return View(db.UnitPrices.ToList());
            }

          //  List<SelectListItem> ObjItem = new List<SelectListItem>()
          //  {
          //new SelectListItem {Text="Standard",Value="0"},
          //new SelectListItem {Text="Discout90",Value="1" },
          //new SelectListItem {Text="Discout80",Value="2"},
          //new SelectListItem {Text="Discout70",Value="3"},
          //new SelectListItem {Text="Discout60",Value="4" },
          //  };
          //  ViewBag.UnitPriceDescriptionList = ObjItem;

            if (ModelState.IsValid)
            {
                db.UnitPrices.Add(unitPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitPrice);
        }


        /// <summary>
        /// Returns UnitPrices/Edit view filled with previously selected category name
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
            UnitPrice unitPrice = db.UnitPrices.Find(id);
            if (unitPrice == null)
            {
                return HttpNotFound();
            }
            return View(unitPrice);
        }


        /// <summary>
        /// Returns UnitPrices/Index View after updating changed data on database
        /// if error returns current view
        /// </summary>
        /// <param name="unitPrice"></param>
        /// <returns>Returns UnitPrices/Index</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitPriceId,UnitPriceValue,UnitPriceDescription")] UnitPrice unitPrice/*,int? id*/)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //UnitPrice unitPrice = db.UnitPrices.Find(id);
            if (unitPrice == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                db.Entry(unitPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitPrice);
        }



        /// <summary>
        /// Returns UnitPrice index view after deleting selected category name on click of confirming delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns> UnitPrices/Index</returns>
        /// <includesource>yes</includesource>
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            UnitPrice unitPrice = db.UnitPrices.Find(id);
            db.UnitPrices.Remove(unitPrice);
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
