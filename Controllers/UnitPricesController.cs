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
{
    public class UnitPricesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UnitPrices
        public ActionResult Index()
        {
            return View(db.UnitPrices.ToList());
        }

        // GET: UnitPrices/Details/5
        public ActionResult Details(int? id)
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

        // GET: UnitPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnitPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UnitPriceId,UnitPriceValue,UnitPriceDescription")] UnitPrice unitPrice)
        {
            if (ModelState.IsValid)
            {
                db.UnitPrices.Add(unitPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(unitPrice);
        }

        // GET: UnitPrices/Edit/5
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

        // POST: UnitPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UnitPriceId,UnitPriceValue,UnitPriceDescription")] UnitPrice unitPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unitPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unitPrice);
        }

        // GET: UnitPrices/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: UnitPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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
