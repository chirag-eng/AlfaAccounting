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
{    /// <summary>
     /// Mie Tanaka
     /// 22/05/2017 Version1
     /// Allows Admin change deposit rate
     /// </summary>

    public class DepositRatesController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DepositRates
        /// <summary>
        /// Returns DepositRates/Index view filled with a depositrate
        /// if there are not rate stored in the database it create
        /// record with value 0.1f to display
        /// </summary>
        /// <returns> returns list of UnitPrices saved on database </returns>
        /// <includesource>yes</includesource>
        public ActionResult Index()
        {
            DepositRate deposit = db.DepositRates.FirstOrDefault();
            if(deposit == null)
            {
                deposit.DepositRateValue = 0.1f;
            }
            return View(deposit);
        }

        /// <summary>
        /// returns Depositrates/Index view after input data got updated in the database
        /// if error return Depositrate/Index view with current value
        /// </summary>
        /// <param name="depositRate"></param>
        /// <returns> UnitPrices/Index </returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "DepositRateId,DepositRateValue")] DepositRate depositRate)
        {
                        DepositRate depo = db.DepositRates.FirstOrDefault();
            depo.DepositRateValue = depositRate.DepositRateValue;
            if (ModelState.IsValid)
            {
                db.Entry(depo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositRate);
        }

        //// GET: DepositRates/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: DepositRates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DepositRateId,DepositRateValue")] DepositRate depositRate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DepositRates.Add(depositRate);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(depositRate);
        //}

        //// GET: DepositRates/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DepositRate depositRate = db.DepositRates.Find(id);
        //    if (depositRate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(depositRate);
        //}

        //// POST: DepositRates/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "DepositRateId,DepositRateValue")] DepositRate depositRate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(depositRate).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(depositRate);
        //}

        //// GET: DepositRates/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DepositRate depositRate = db.DepositRates.Find(id);
        //    if (depositRate == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(depositRate);
        //}

        //// POST: DepositRates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DepositRate depositRate = db.DepositRates.Find(id);
        //    db.DepositRates.Remove(depositRate);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        /// <summary>
        /// Dispose entity frame work database after finished using it
        /// </summary>
        /// <param name="disposing"></param>
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
