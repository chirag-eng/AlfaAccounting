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
    /// <summary>
    /// Mie Tanaka
    /// 22/5/2017 Version 1
    /// Load Existing Booking to BookDates Calendar View 
    /// Creates New Booking, alows Edit of booking date by resizing and moving
    /// selectiong box on the calender, of only the current session booking,
    /// </summary>
    /// <includesource>yes</includesource>

    [AllowAnonymous]
    public class BackendController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// JsonEvent Class olny used to store and manipulate 
        /// selected data passed from the BookDates View.
        /// </summary>
        public class JsonEvent
        {
            public string id { get; set; }
            public string text { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }
        /// <summary>
        /// Returns Data from database and parse it into JsonEvent class data to display in javascript on BookDates View 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Json result data to BokDates View</returns>
        /// <includesource>yes</includesource>
        public ActionResult Events(DateTime? start, DateTime? end)
        {

            //         SQL: SELECT* FROM[event] WHERE NOT (([end] <= @start) OR ([start] >= @end))
            var events = from ev in db.Bookings.AsEnumerable() where !(ev.BookingEndDateTime <= start || ev.BookingStartDateTime >= end) && ev.BookingStatus != BookingStatus.Cancelled select ev;

            var result = events
                .Select(e => new JsonEvent()
                {
                    start = e.BookingStartDateTime.ToString("s"),
                    end = e.BookingEndDateTime.ToString("s"),
                    text = e.BookingStatus.ToString(),
                    id = e.BookingId.ToString()
                })
                .ToList();

            return new JsonResult
            {
                Data = result
            };
        }
        /// <summary>
        /// Save Bookings if input is valid
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="name"></param>
        /// <returns>data to the BookDates view calender</returns>
        /// <includesource>yes</includesource>
        public ActionResult Create(string start, string end, string name)
        {
            DateTime startT = Convert.ToDateTime(start);
            DateTime endT = Convert.ToDateTime(end);
            //find out if user try to book past time

            if(startT< DateTime.Now)
            {
                TempData["viewBag.Error"] = "You can not book any date and time before now.";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                return RedirectToAction("BookDates");
                
            }
            //find out if there are any time overlapping bookings
            var overlappingbkgs = db.Bookings.Where(b => b.BookingEndDateTime > startT && b.BookingStartDateTime < endT && b.BookingStatus != BookingStatus.Cancelled).Count();
            // if already booking exist between start and end time, return the bookdate view with alert message else book
            if (overlappingbkgs > 0)
            {
                TempData["viewBag.Error"] = "You can not change already booked details";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                return RedirectToAction("BookDates"); 
            }
            else { 
            var toBeCreated = new Booking
            {
                BookingStartDateTime = Convert.ToDateTime(start),
                BookingEndDateTime = Convert.ToDateTime(end),
                BookingStatus = (BookingStatus)Enum.Parse(typeof(BookingStatus),name)
            };
            var userid = User.Identity.GetUserId();
            //            var DictionaryUnitPrices = db.UnitPrices.Where(u => u.UnitPriceDescription == "Standard").ToDictionary<UnitPrice, string>(u => u.UnitPriceDescription);
            var standardUnitPrice = db.UnitPrices.SingleOrDefault(u => u.UnitPriceDescription == "Standard");
            var newBooking = new Booking()
            {
                BookingStartDateTime = Convert.ToDateTime(start),
                BookingEndDateTime = Convert.ToDateTime(end),
                BookingStatus = (BookingStatus)Enum.Parse(typeof(BookingStatus), name),
                UnitPriceId = standardUnitPrice.UnitPriceId,
               // UnitPriceId = DictionaryUnitPrices["Standard"].UnitPriceId,
                Id = userid
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

            newBooking.Subtotal = db.UnitPrices.SingleOrDefault(u => u.UnitPriceId == newBooking.UnitPriceId).UnitPriceValue * (float)(newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).TotalMinutes / 60;
            newBooking.BookingDeposit = newBooking.Subtotal * db.DepositRates.Select(d=>d.DepositRateValue).FirstOrDefault();
            newBooking.ItemDescription = newBooking.BookingStartDateTime.Date.ToString("d")
                    + " From " + newBooking.BookingStartDateTime.Hour + ":" + newBooking.BookingStartDateTime.Minute
                    + " to " + newBooking.BookingEndDateTime.Hour + ":" + newBooking.BookingEndDateTime.Minute
                    + "  "+ (newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).Hours + "hr" + (newBooking.BookingEndDateTime - newBooking.BookingStartDateTime).Minutes + "min";

                           db.Bookings.Add(newBooking);
                           db.SaveChanges();
            AddToCart(newBooking.BookingId);

            return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeCreated.BookingId } } };
           }
        }

        private void AddToCart(int id)
        {
            // Retrieve the booking from the database
            var addedBooking = db.Bookings
                .Single(b => b.BookingId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedBooking);
        }
        /// <summary>
        /// Saves the changes of new and end date if not overlapping with existing booking.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStart"></param>
        /// <param name="newEnd"></param>
        /// <returns>updated data to the BookDates view calender</returns>
        /// <includesource>yes</includesource>
        public ActionResult Move(int id, string newStart, string newEnd)
        {
            DateTime newStartT = Convert.ToDateTime(newStart);
            DateTime newEndT = Convert.ToDateTime(newEnd);
            //find out if user try to change booking to past time
            
            if (newStartT < DateTime.Now)
            {
                TempData["viewBag.Error"] = "You can not book any date and time before now.";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                return RedirectToAction("BookDates");               
            }
            //find out if the selected booking is overlapping with existing booking
            var overlappingbkgs = db.Bookings.Where(b => b.BookingEndDateTime > newStartT && b.BookingStartDateTime < newEndT && b.BookingStatus != BookingStatus.Cancelled && b.Invoice != null).Count();
            //find out if the selected booking is already paid bookings or cancelled bookings
            var notPaidBkgs = db.Bookings.Where(b => b.Invoice == null && b.BookingId == id).ToList();
            // if not paidBkgs exist(is more than 0), it is ok to amend
            if (notPaidBkgs.Count() > 0)
            {
                var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                db.SaveChanges();

                return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
            }
            else if (overlappingbkgs>0)
            {
                TempData["viewBag.Error"] = "You can not change already booked details";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                return RedirectToAction("BookDates");
            }
            return View();
        }

        /// <summary>
        /// Saves the changes of new and end date if not overlapping with existing booking.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStart"></param>
        /// <param name="newEnd"></param>
        /// <returns>updated data to the BookDates view calender</returns>
        /// <includesource>yes</includesource>
        public ActionResult Resize(int id, string newStart, string newEnd)
        {   DateTime newStartT = Convert.ToDateTime(newStart);
            DateTime newEndT = Convert.ToDateTime(newEnd);
            //find out if user try to change booking to past time
            if (newStartT< DateTime.Now)
            {
                TempData["viewBag.Error"] = "You can not book any date and time before now.";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                return RedirectToAction("BookDates");
            }
            //find out if the selected booking is overlapping with existing booking
            var overlappingbkgs = db.Bookings.Where(b => b.BookingEndDateTime > newStartT && b.BookingStartDateTime < newEndT && b.BookingStatus != BookingStatus.Cancelled && b.Invoice != null).Count();
            //find out if the selected booking is already paid bookings or cancelled bookings
            var notPaidBkgs = db.Bookings.Where(b => b.Invoice == null && b.BookingId == id).ToList();
            // if not paidBkgs exist(is more than 0), it is ok to amend
            if (notPaidBkgs.Count() > 0)
            {
                    var toBeResized = (from ev in db.Bookings where ev.BookingId == id select ev).First();
                toBeResized.BookingStartDateTime = Convert.ToDateTime(newStart);
                toBeResized.BookingEndDateTime = Convert.ToDateTime(newEnd);
                db.SaveChanges();

                return new JsonResult { Data = new Dictionary<string, object> { { "id", toBeResized.BookingId } } };
                
            }
            else if (overlappingbkgs >0)
            {
                //RedirectToAction(Request.RawUrl);
                TempData["viewBag.Error"] = "You can not change already booked details";
                ViewBag.Error = TempData["viewBag.Error"].ToString();
                //ViewBag.Message = "You can't change detail of booking you already paid from this page, click manage booking";
                return RedirectToAction("BookDates");
            }
            return View();
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    HttpContext ctx = HttpContext.Current;

        //    // check if session is supported
        //    if (ctx.Session != null)
        //    {

        //        // check if a new session id was generated
        //        if (ctx.Session.IsNewSession)
        //        {

        //            // If it says it is a new session, but an existing cookie exists, then it must
        //            // have timed out
        //            string sessionCookie = ctx.Request.Headers["Cookie"];
        //            if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
        //            {
        //                
        //            }
        //        }
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

    }

}