using AlfaAccounting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlfaAccounting.Controllers
{
    public class BackendController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Invoice newInvoice = new Invoice();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        public class JsonEvent
        {
            public string id { get; set; }
            public string text { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }

        public ActionResult Events(DateTime? start, DateTime? end)
        {

            //         SQL: SELECT* FROM[event] WHERE NOT (([end] <= @start) OR ([start] >= @end))
            var events = from ev in db.Bookings.AsEnumerable() where !(ev.BookingEndDateTime <= start || ev.BookingStartDateTime >= end) select ev;

            var result = events
                .Select(e => new JsonEvent()
                {
                    start = e.BookingStartDateTime.ToString("s"),
                    end = e.BookingEndDateTime.ToString("s"),
                    text = e.BookingStatus,
                    id = e.BookingId.ToString()
                })
                .ToList();

            return new JsonResult
            {
                Data = result
            };
        }

        public ActionResult Create(string start, string end, string name)
        {
            var toBeCreated = new Booking
            {
                BookingStartDateTime = Convert.ToDateTime(start),
                BookingEndDateTime = Convert.ToDateTime(end),
                BookingStatus = name
            };
//            var defaultUnitPrices = db.UnitPrices.Where(u => u.UnitPriceDescription == "Standard").ToDictionary<UnitPrice, string>(u => u.UnitPriceDescription);
            var newBooking = new Booking()
            {
                BookingStartDateTime = Convert.ToDateTime(start),
                BookingEndDateTime = Convert.ToDateTime(end),
                BookingStatus = name,
//                UnitPriceId = defaultUnitPrices["Standard"].UnitPriceId,
                UnitPriceId = db.UnitPrices.SingleOrDefault(u=>u.UnitPriceDescription == "Standard").UnitPriceId,
//                UnitPrice = defaultUnitPrices["Standard"],
                Id = User.Identity.GetUserId()
            };
            //var newBkgInvLine = new BookingInvoiceLine()
            //{
            //    UnitPriceId = defaultUnitPrices["Standard"].UnitPriceId,              
            //    BookingDuration = newBooking.BookingEndDateTime - newBooking.BookingStartDateTime,
            //    BookingAdjustmentHrs = 0f,
            //    Subtotal = defaultUnitPrices["Standard"].UnitPriceValue * (float)(newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).TotalMinutes / 60,
            //    ItemDescription = newBooking.BookingStartDateTime.Date.ToString("d")
            //        + " From " + newBooking.BookingStartDateTime.Hour + ":" + newBooking.BookingStartDateTime.Minute
            //        + " to " + newBooking.BookingEndDateTime.Minute + ":" + newBooking.BookingEndDateTime.Minute
            //        + " "+ (newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).Hours + "hr" + (newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).Minutes + "min",
            //    BookingId = newBooking.BookingId
            //};
//            newBooking.BookingDuration = newBooking.BookingEndDateTime - newBooking.BookingStartDateTime;
             newBooking.Subtotal = db.UnitPrices.SingleOrDefault(u=>u.UnitPriceId == newBooking.UnitPriceId).UnitPriceValue * (float)newBooking.BookingDuration.TotalMinutes / 60;
            newBooking.Subtotal = newBooking.UnitPrice.UnitPriceValue * (float)newBooking.BookingDuration.TotalMinutes / 60;
            newBooking.ItemDescription = newBooking.BookingStartDateTime.Date.ToString("d")
                    + " From " + newBooking.BookingStartDateTime.Hour + ":" + newBooking.BookingStartDateTime.Minute
                    + " to " + newBooking.BookingEndDateTime.Minute + ":" + newBooking.BookingEndDateTime.Minute
                    + newBooking.BookingDuration.Hours + "hr" + newBooking.BookingDuration.Minutes + "min";

//               db.Bookings.Add(newBooking);
//               db.SaveChanges();

            List<Booking> listNewBookingToBeSavedToDb = new List<Booking>();
            listNewBookingToBeSavedToDb.Add(newBooking);
            Session["ListNewBookingToBeSavedToDb"] = listNewBookingToBeSavedToDb;

            //List<BookingInvoiceLine> listNewBookingInvoiceLineToBeSavedToDb = new List<BookingInvoiceLine>();
            //listNewBookingInvoiceLineToBeSavedToDb.Add(newBkgInvLine);
            //Session["ListNewBookingInvoiceLineToBeSavedToDb"] = listNewBookingInvoiceLineToBeSavedToDb;

            //newInvoice.Bookings.Add(newBooking);
            //Session["ListNewBookingToBeSavedToDb"] = newInvoice;

            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeCreated.BookingId } } };

        }

        public ActionResult Move(int id, string newStart, string newEnd)
        {
            var existingbkgs = db.Bookings.Where(b => b.BookingId == id);
            if (existingbkgs == null)
            {
                var overlappingbkgs = db.Bookings.Where(b => b.BookingEndDateTime >= Convert.ToDateTime(newStart) && b.BookingStartDateTime <= Convert.ToDateTime(newEnd));
                if (overlappingbkgs == null)
                {
                    var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                    toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                    toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                    //           db.SubmitChanges();
                    //db.SaveChanges();
                    var listNewBookingToBeSavedToDb = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
                    foreach (Booking nbkg in listNewBookingToBeSavedToDb)
                    {
                        var newbkgid = nbkg.BookingId.Equals(id);
                        if (newbkgid)
                        {
                            nbkg.BookingStartDateTime = Convert.ToDateTime(newStart);
                            nbkg.BookingEndDateTime = Convert.ToDateTime(newEnd);
                        }
                    }
                    Session["ListNewBookingToBeSavedToDb"] = listNewBookingToBeSavedToDb;
                    return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
                }
            }
            return null;
        }


        public ActionResult Resize(int id, string newStart, string newEnd)
        {
            /*           var existingBkgsWithInvlookup = db.Bookings.Where(b => b.ApplicationUser.UserName == User.Identity.Name).ToList();
                                                                    && b.BookingId == id 
                                                                    && b.Invoices.Any()).ToLookup(b=>b.BookingId, b=>b.Invoices).ToList();
                       var existingBkgslookup = db.Bookings.Where(b => b.BookingId == id && !b.Invoices.Any()).ToList();
                                   var existingBkgs = db.Bookings.Where(b => b.BookingId == id && b.InvoiceId.ToString().Contains(null)).ToList();

                       check if existing booking with invoice exist, if exist, check if payment exist for the invoice if not, allow user to save
                       if (existingBkgsWithInvlookup.Count()>0) {
                                      var invid = existingBkgs.TakeWhile(b=>!b.InvoiceId.ToString().Equals(null)).ToString();

                           var existingBkgsInvoice = new List<Invoice>();
                                           var queryable = existingBkgslookup.AsQueryable();
                               foreach (var iv in existingBkgsWithInvlookup)
                               {
                                   existingBkgsInvoice.Add((Invoice)db.Invoices.Where(i => i.Bookings == iv..ElementAt(0).SingleOrDefault()));
                               };

                           var existingBkgsPayments = new List<Payment>();

                               existingBkgsPayments.Add((Payment)db.Payments.Where(p => p.InvoiceId == existingBkgsInvoice.ElementAt(0).InvoiceId).SingleOrDefault());

                                       var existingBkgsInvoice = db.Invoices.Where(i => i.InvoiceId.ToString().Contains(invid));
                                       var existingBkgsPayments = db.Payments.Where(p => p.Invoice.InvoiceId.ToString().Contains(invid));

                           check if payments of the invoice exist, if null allows user to book.
                           if (existingBkgsPayments.Count() == 0)
                           {
                               var bkgs = db.Bookings.Where(b => b.BookingEndDateTime >= Convert.ToDateTime(newStart) && b.BookingStartDateTime <= Convert.ToDateTime(newEnd));
                               if (bkgs == null)
                               {
                                   var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                                   toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                                   toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                                   db.SubmitChanges();
                                   db.SaveChanges();

                                   return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
                               }
                           }
                       }
                       check if booking without invoice  exist if so.
                       if (existingBkgsWithInvlookup.Count() == 0)
                       {   //check if booking time and start does not over lap existing boooking, if not allows user to book.
                           var bkgs = db.Bookings.Where(b => b.BookingEndDateTime >= Convert.ToDateTime(newStart) && b.BookingStartDateTime <= Convert.ToDateTime(newEnd));
                           if (bkgs == null)
                           {
                               var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                               toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                               toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                               db.SubmitChanges();
                               db.SaveChanges();

                               return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
                           }
                       }
                       return null;
                      */
            //var loggedInUser = User.Identity.Name;
            //var existingBkgsWithInv = db.Bookings.Where(b => b.ApplicationUser.UserName == loggedInUser
            //                                             && b.BookingId == id
            //                                            && b.Invoices.Any()).ToList()/*.ToLookup(b=>b.BookingId, b=>b.Invoices).ToList()*/;
            //var bkgs = db.Bookings.Where(b => b.BookingEndDateTime >= Convert.ToDateTime(newStart) && b.BookingStartDateTime <= Convert.ToDateTime(newEnd));

            //    if (existingBkgsWithInv.Count() > 0)
            //    {
            //        var existingBkgsInvoice = new List<Invoice>();
            //        var existingBkgsWithInvElementi = new Booking();
            //        for (int i = 0; i < existingBkgsWithInv.Count(); i++)
            //        {
            //            existingBkgsWithInvElementi = existingBkgsWithInv.ElementAt(i);
            //            for (int j = 0; j < existingBkgsWithInvElementi.Invoices.Count(); j++)
            //            {
            //                existingBkgsInvoice.Add(existingBkgsWithInvElementi.Invoices.ElementAt(j));
            //            }
            //        }

            //        var existingBkgsPayments = new List<Payment>();
            //        var existingBkgsInvoiceElementi = new Invoice();

            //        for (int k = 0; k < existingBkgsInvoice.Count(); k++)
            //        {
            //            existingBkgsInvoiceElementi = existingBkgsInvoice.ElementAt(k);
            //            for (int l = 0; l < existingBkgsInvoiceElementi.Payments.Count(); l++)
            //            {
            //                db.Payments.Contains(existingBkgsInvoiceElementi.Payments.ElementAt(l));
            //            }
            //        }

            //        if (existingBkgsPayments == null)
            //        {
            //            var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
            //            toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
            //            toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
            ////            db.SubmitChanges();
            //            db.SaveChanges();

            //            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
            //        }
            //    }
            //    else if (bkgs == null)
            if (id != 0)
            {
                var existingbkgs = db.Bookings.Where(b => b.BookingId == id);
                if (existingbkgs == null)
                {
                    var overlappingbkgs = db.Bookings.Where(b => b.BookingEndDateTime >= Convert.ToDateTime(newStart) && b.BookingStartDateTime <= Convert.ToDateTime(newEnd));
                    if (overlappingbkgs == null)
                    {
                        var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                        toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                        toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                        //       db.SubmitChanges();
                        //db.SaveChanges();

                        var listNewBookingToBeSavedToDb = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
                        foreach (Booking nbkg in listNewBookingToBeSavedToDb)
                        {
                            var newbkgid = nbkg.BookingId.Equals(id);
                            if (newbkgid)
                            {
                                nbkg.BookingStartDateTime = Convert.ToDateTime(newStart);
                                nbkg.BookingEndDateTime = Convert.ToDateTime(newEnd);
                            }
                        }
                        Session["ListNewBookingToBeSavedToDb"] = listNewBookingToBeSavedToDb;

                        return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
                    }
                }
            }
            ViewBag.Message = "You can't change your already paid booking from this page, click manage booking";
            return null;
        }
    }
}