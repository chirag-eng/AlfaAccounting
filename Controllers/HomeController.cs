using AlfaAccounting.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlfaAccounting.Controllers
{
    /// <summary>
    /// Name:Mie Tanaka
    /// Version 0.0
    /// Name:26/05/2017
    /// Description: returns HomeIndex Home/contact view
    /// </summary>

    public class HomeController : Controller
        {
        ApplicationDbContext db = new ApplicationDbContext();
        //returns Home/Index view
        /// <summary>
        /// returns Home/Index View
        /// </summary>
        /// <returns>returns Home/Index View</returns>
        [AllowAnonymous]// allow this method accessible to unautonrized users
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        ///rerurns Home/Contact view
        /// </summary>
        /// <returns> rerurns Home/Contact view</returns>
        [AllowAnonymous] // allow this method accessible to unautonrized users
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}