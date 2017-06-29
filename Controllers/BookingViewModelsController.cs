using AlfaAccounting.Models;
using Braintree;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlfaAccounting.Controllers;
using System.Net;
using System.Data.Entity;
using System.Reflection;


namespace AlfaAccounting.Controllers
{/// <summary>
 /// Mie Tanaka
 /// Version 0.0
 /// 26/05/2017
 /// </summary>
    //[ApplicationSignInManager.SessionTimeout]
    public class BookingViewModelsController : Controller
    {
        private ApplicationUserManager _userManager;
        public IBraintreeConfiguration config = new BraintreeConfiguration();
        ApplicationDbContext db = new ApplicationDbContext();


        // GET: Dashboard
        /// <summary>
        /// with user specified year parameter, calcuates and displays the graph data
        /// also calucate and display user visits notification, 
        /// admin visit notification and visit status update notification
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Graph and notification in View</returns>
        /// <includesource>yes</includesource>
        public ActionResult Dashboard(int? year)
        {   // create a list of years that get listed in select list in year combo box
            List<int> years = new List<int>() { 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030 };
            ViewBag.Year = new SelectList(years, DateTime.Now.Year);
            if (year == null || year == 0)
            {
                year = DateTime.Now.Year;
            }

            int nextyear = (int)year + 1;
            //delcare each months revenue from apr to Dec assigning a zero float value
            //get list of each months and each month's revenue variable gets assinged 
            // the add up of the the credit and debt total of each list. 
            var aprRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 4, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 4, 30, 0, 0, 0)).ToList().ForEach(ph => aprRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Apr = aprRevenue;
            var mayRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 5, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 5, 31, 0, 0, 0)).ToList().ForEach(ph => mayRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.May = mayRevenue;
            var junRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 6, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 6, 30, 0, 0, 0)).ToList().ForEach(ph => junRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Jun = junRevenue;
            var julRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 7, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 7, 31, 0, 0, 0)).ToList().ForEach(ph => julRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Jul = julRevenue;
            var augRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 8, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 8, 31, 0, 0, 0)).ToList().ForEach(ph => aprRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Aug = augRevenue;
            var sepRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 9, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 9, 30, 0, 0, 0)).ToList().ForEach(ph => mayRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Sep = sepRevenue;
            var octRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 10, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 10, 31, 0, 0, 0)).ToList().ForEach(ph => junRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Oct = octRevenue;
            var novRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 11, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 11, 30, 0, 0, 0)).ToList().ForEach(ph => julRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Nov = novRevenue;
            var decRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime((int)year, 12, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime((int)year, 12, 31, 0, 0, 0)).ToList().ForEach(ph => julRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Dec = decRevenue;
            var janRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime(nextyear, 1, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime(nextyear, 1, 31, 0, 0, 0)).ToList().ForEach(ph => janRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Jan = janRevenue;
            var febRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime(nextyear, 2, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime(nextyear, 2, 28, 0, 0, 0)).ToList().ForEach(ph => febRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Feb = febRevenue;
            var marRevenue = 0f; db.PaymentHistories.Where(ph => ph.PaymentDateTime >= new DateTime(nextyear, 3, 1, 0, 0, 0) && ph.PaymentDateTime <= new DateTime(nextyear, 3, 31, 0, 0, 0)).ToList().ForEach(ph => marRevenue += (ph.CreditAmount - ph.DebitAmount)); ViewBag.Mar = marRevenue;
            // calcualate annual Revenue amount for heading of the graph
            var annualRevenue = aprRevenue + mayRevenue + junRevenue + julRevenue + augRevenue + sepRevenue + octRevenue + novRevenue + decRevenue + janRevenue + febRevenue + marRevenue;
            ViewBag.AnnualRevenue = annualRevenue;
            // calcualate annual total invoiced amount for heading of the graph
            var annualTotalInv = 0f; db.Invoices.Where(i => i.InvoiceIssueDate >= new DateTime((int)year, 4, 1, 0, 0, 0) && i.InvoiceIssueDate <= new DateTime(nextyear, 3, 31, 0, 0, 0) && i.Bookings.Count() == i.Bookings.Where(b => b.BookingStatus != BookingStatus.Booked).Count()).ToList().ForEach(i => annualTotalInv += (i.ReceivableDepositAmount + i.ReceivableRemainingAmount - i.PayableDepositAmount - i.PayableAmount + i.InvoiceCancellationFee)); ViewBag.AnnualTotalInv = annualTotalInv;

            // calcualate annual total invoiced amount for heading of the graph
            var noUserVisitConfirmationGetSent = db.Bookings.Where(b => (DbFunctions.DiffHours(DateTime.Now, b.BookingStartDateTime)) <= 24 && b.VisitConfirmationSent == false).Count(); ViewBag.NoUserVisitConfirmationGetSent = noUserVisitConfirmationGetSent;

            //var annualUnpaidInv = 0f; db.Invoices.Where(i => (i.ReceivableDepositAmount + i.ReceivableRemainingAmount - i.PayableDepositAmount - i.PayableAmount + i.InvoiceCancellationFee) !=  (i.Payments.FirstOrDefault().CreditDepositAmount + i.Payments.FirstOrDefault().CreditRemainingAmount - i.Payments.FirstOrDefault().DebitDepositAmount - i.Payments.FirstOrDefault().DebitRemainingAmount + i.Payments.FirstOrDefault().CreditCancellationFee) && i.Payments.FirstOrDefault().PaymentType == PaymentType.Remaining.ToString()).ToList().ForEach(i=> annualUnpaidInv += (i.ReceivableRemainingAmount - i.PayableDepositAmount - i.PayableAmount + i.InvoiceCancellationFee)); ViewBag.AnnualUnpaidInv = annualUnpaidInv;

            // Get the count of Booking that require Booking Status updated by Admin
            int? bkgUpdateRequiredCount = (from b in db.Bookings
                                           where b.BookingStatus == BookingStatus.Booked && b.BookingEndDateTime < DateTime.Now
                                           select (int?)b.BookingId).Count();
            if (bkgUpdateRequiredCount == null) { bkgUpdateRequiredCount = 0; }
            ViewBag.BkgUpdateRequiredCount = bkgUpdateRequiredCount;

            //Get Today’s visit list count to send mail to Admin
            var todaysVisit = db.Bookings.Where(b => DbFunctions.TruncateTime(DateTime.Now) == DbFunctions.TruncateTime(b.BookingStartDateTime)).ToList();

            ViewBag.NotodaysVisit = todaysVisit.Count();
            return View();
        }


        /// <summary>
        /// On Click Ation to send todays visit's users detail
        /// </summary>
        /// <returns> DashBoard View </returns>
        /// <includesource>yes</includesource>
        public async Task<ActionResult> SendAdminTodaysVisitText()
        {
            var todaysVisit = db.Bookings.Where(b => DbFunctions.TruncateTime(DateTime.Now) == DbFunctions.TruncateTime(b.BookingStartDateTime)).ToList();
            if (todaysVisit.Count() > 0)
            {
                var message = new List<string>();
                foreach (var bk in todaysVisit) { var user = db.Users.Find(bk.Id); message.Add("You have visist on " + bk.ItemDescription + " with " + user.Companyname + ", " + user.Street + ", " + user.Postcode + ", " + user.PhoneNumber + "."); }
                string messages = string.Join("\r\n <br/>", message);
                var adminUserId = UserManager.FindByEmail("alfaacc00unting2017@gmail.com").Id;
                await UserManager.SendEmailAsync(adminUserId, "today's alfa accounting visits", messages);
            }
            return RedirectToAction("Dashboard", "BookingViewModels");
        }
        /// <summary>
        /// Calculate and display count of invoice and total amount of unpaid invoice sum
        /// </summary>
        /// <returns>View with notification data</returns>
        /// <includesource>yes</includesource>
        public ActionResult DashBoardUser()
        {
            var loggedinUserId = User.Identity.GetUserId();

            //find out invoice of logged in users 
            //and number of bookings in one invoice matches with number of booking which status is not booked
            //and total receivale remaining invoice amount and total paid remaining credit amount does not match    
            var tempInv = db.Invoices.Where(i => i.Id == loggedinUserId && i.Bookings.Count() == i.Bookings.Where(b=>b.BookingStatus != BookingStatus.Booked).Count() && (i.ReceivableDepositAmount + i.ReceivableRemainingAmount - i.PayableDepositAmount - i.PayableAmount + i.InvoiceCancellationFee) != (i.Payments.FirstOrDefault().CreditDepositAmount + i.Payments.FirstOrDefault().CreditRemainingAmount - i.Payments.FirstOrDefault().DebitDepositAmount - i.Payments.FirstOrDefault().DebitRemainingAmount + i.Payments.FirstOrDefault().CreditCancellationFee)).ToList();

            var noOfunpaidInvoice = tempInv.Count();
            var unpaidamount = 0f;
            //db.Invoices.Where(i => i.Id == loggedinUserId && (i.ReceivableDepositAmount + i.ReceivableRemainingAmount - i.PayableDepositAmount - i.PayableAmount + i.InvoiceCancellationFee) != (i.Payments.FirstOrDefault().CreditDepositAmount + i.Payments.FirstOrDefault().CreditRemainingAmount - i.Payments.FirstOrDefault().DebitDepositAmount - i.Payments.FirstOrDefault().DebitRemainingAmount + i.Payments.FirstOrDefault().CreditCancellationFee)).ToList()
                tempInv.ForEach(i => unpaidamount += ((i.ReceivableRemainingAmount  - i.PayableAmount) - (i.Payments.FirstOrDefault().CreditRemainingAmount - i.Payments.FirstOrDefault().DebitRemainingAmount)));
            ViewBag.UnpaidInvoice = unpaidamount;
            ViewBag.NoOfUnpaidInvoice = noOfunpaidInvoice;
            return View();
        }

        /// <summary>
        /// Returns a BookDates View that displays calender makde of java script
        /// </summary>
        /// <returns></returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        public ActionResult BookDates()
        {
            return View();
        }

        /// <summary>
        /// opens empty booking start end date input view.
        /// </summary>
        /// <returns> empty AddBooking View </returns>
        [AllowAnonymous]
        public ActionResult AddBooking()
        {
            return View();
        }

        /// <summary>
        /// post the input value and if successful save it to the database return user back to BookDates
        /// if unsuccessful returns current AddBooking view with posted data and error message
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// if value state is Returns BookDates view
        /// if not valid, returns the current AddBooking view with posted data and error message
        /// </returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult AddBooking([Bind(Include = "BookingId,BookedDate,BookingStartDateTime,BookingEndDateTime,BookingStatus,BookingAdjustmentHrs,ItemDescription,Subtotal,BookingDeposit,BookingCancellationFee,UnitPriceId,Id,InvoiceId")] Booking booking)
        public ActionResult AddBooking(AddBookingViewModel model)
        {   //as jquery datetimepicker only returns string convert the start date and enddate to stging
            var startDate = Convert.ToDateTime(model.BookingStartDateTime);
            var endDate = Convert.ToDateTime(model.BookingEndDateTime);

            if (model.BookingStartDateTime == null || model.BookingEndDateTime == null)
            {
                ViewBag.Error = "You must inupt";
                return View(model);
            }

            if (startDate < DateTime.Now || endDate < DateTime.Now)
            {
                ViewBag.Error = "You can't book any time before now";
                return View(model);
            }
            //if user by mistake click book when start date and time is the same it returns back current view.
            if (startDate == endDate)
            {
                ViewBag.Error = "Your end time must be at least 30min after start time";
                return View(model);
            }
            if (startDate.Date != endDate.Date)
            {
                ViewBag.Error = "You can't book over two days";
                return View(model);
            }
            if ((endDate - startDate).TotalHours > 8)
            {
                ViewBag.Error = "You can't book more than 8 hrs at one time";
                return View(model);
            }

            Booking booking = new Booking();
            booking.BookedDate = DateTime.Now;
            booking.BookingStatus = BookingStatus.Booked;
            booking.BookingAdjustmentHrs = 0f;
            booking.UnitPriceId = db.UnitPrices.Where(u => u.UnitPriceDescription == "Standard").SingleOrDefault().UnitPriceId;
            booking.BookingStartDateTime = startDate;
            booking.BookingEndDateTime = endDate;
            booking.ItemDescription = startDate.ToString("d")
                    + " From " + startDate.Hour + ":" + startDate.Minute
                    + " to " + endDate.Hour + ":" + endDate.Minute
                    + "  " + (endDate - startDate).Hours + "hr" + (endDate - startDate).Minutes + "min";
            booking.Subtotal = db.UnitPrices.SingleOrDefault(u => u.UnitPriceId == booking.UnitPriceId).UnitPriceValue * (float)(endDate - startDate).TotalMinutes / 60; ;
            booking.BookingDeposit = booking.Subtotal * db.DepositRates.Select(d => d.DepositRateValue).FirstOrDefault();
            booking.BookingCancellationFee = 0f;
            booking.Id = User.Identity.GetUserId();
            

            //find out if the selected booking is overlapping with existing booking
            var overlappingbkgscount = db.Bookings.Where(b => b.BookingEndDateTime > startDate && b.BookingStartDateTime < endDate && b.BookingStatus != BookingStatus.Cancelled && b.Invoice != null).Count();

            if (overlappingbkgscount > 0)
            {
                var overlappingbkgsStartime = db.Bookings.Where(b => b.BookingEndDateTime > startDate && b.BookingStartDateTime < endDate && b.BookingStatus != BookingStatus.Cancelled && b.Invoice != null).FirstOrDefault().BookingStartDateTime;
                var overlappingbkgEndtime = db.Bookings.Where(b => b.BookingEndDateTime > startDate && b.BookingStartDateTime < endDate && b.BookingStatus != BookingStatus.Cancelled && b.Invoice != null).FirstOrDefault().BookingEndDateTime;
                if (startDate < overlappingbkgsStartime && overlappingbkgEndtime < endDate)
                {
                    ViewBag.Error = "You can not book over the existing booking";
                    //ViewBag.Message = "You can't change your already paid booking from this page, click manage booking";
                    return RedirectToAction("BookDates");
                }
                if ((overlappingbkgsStartime < endDate && endDate < overlappingbkgEndtime))
                {
                    ViewBag.Error = "Your end time is overlapping with already booked time slot";
                    //endDate value changes to overlapping bkg end time.
                    //any value related to endDate get updated
                    endDate = overlappingbkgsStartime;
                    booking.BookingEndDateTime = endDate;
                    booking.ItemDescription = startDate.ToString("d")
                    + " From " + startDate.Hour + ":" + startDate.Minute
                    + " to " + endDate.Hour + ":" + endDate.Minute
                    + "  " + (endDate - startDate).Hours + "hr" + (endDate - startDate).Minutes + "min";
                    booking.Subtotal = db.UnitPrices.SingleOrDefault(u => u.UnitPriceId == booking.UnitPriceId).UnitPriceValue * (float)(endDate - startDate).TotalMinutes / 60; ;
                    booking.BookingDeposit = booking.Subtotal * db.DepositRates.Select(d => d.DepositRateValue).FirstOrDefault();
                }
                if (overlappingbkgsStartime < startDate && startDate < overlappingbkgEndtime)
                {
                    ViewBag.Error = "Your start time is overlapping with already booked time slot";
                    //endDate value changes to overlapping bkg end time.
                    //any value related to startDate get updated
                    startDate = overlappingbkgEndtime;
                    booking.BookingStartDateTime = startDate;
                    booking.ItemDescription = startDate.ToString("d")
                    + " From " + startDate.Hour + ":" + startDate.Minute
                    + " to " + endDate.Hour + ":" + endDate.Minute
                    + "  " + (endDate - startDate).Hours + "hr" + (endDate - startDate).Minutes + "min";
                    booking.Subtotal = db.UnitPrices.SingleOrDefault(u => u.UnitPriceId == booking.UnitPriceId).UnitPriceValue * (float)(endDate - startDate).TotalMinutes / 60; ;
                    booking.BookingDeposit = booking.Subtotal * db.DepositRates.Select(d => d.DepositRateValue).FirstOrDefault();
                }
                if (ModelState.IsValid)
                {
                    //save the booking to the database
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    //add the booking to shopping cart
                    AddToCart(booking.BookingId);
                    return RedirectToAction("BookDates");
                }
            }
            if (overlappingbkgscount == 0)
            {
                if (ModelState.IsValid)
                {
                    //save the booking to the database
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    //add the booking to shopping cart
                    AddToCart(booking.BookingId);
                    return RedirectToAction("BookDates");
                }
            }

            return View(model);
        }
        /// <summary>
        /// Helper method to Add Booking To the Cart BookingId
        /// </summary>
        /// <param name="id"></param>
        /// <includesource>yes</includesource>
        private void AddToCart(int id)
        {
            // Retrieve the booking from the database
            var addedBooking = db.Bookings
                .FirstOrDefault(b => b.BookingId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedBooking);
        }
        /// <summary>
        /// Create model using bookings in side booking cart
        /// Get cart
        /// Get cart items
        /// Set list of new Bookings from the items with foreach loop
        /// Create New CofirmBookingViewModels,
        /// Create New Invoice
        /// Set the deposit amount from the booking data in the new invoice
        /// Save Invoice as NewInvoice session
        /// </summary>
        /// <returns>returns the view filled with the model</returns>
        /// <includesource>yes</includesource>
        public ActionResult ConfirmBooking()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var items = cart.GetCartItems();
            var listNewBookings = new List<Booking>();

            foreach (var item in items)
            {
                listNewBookings.Add(db.Bookings.FirstOrDefault(b => b.BookingId == item.BookingId));
            }
            var model = new ConfirmBookingViewModel()
            {
                InvoiceIssueDate = DateTime.Now,
                ApplicationUser = UserManager.FindById(User.Identity.GetUserId()),
                Bookings = listNewBookings,
                ReceivableRemainingAmount = 0f,
                PayableAmount = 0f,
                PayableDepositAmount = 0f
            };
            var newInvoice = new Invoice()
            {
                InvoiceIssueDate = model.InvoiceIssueDate,
                ReceivableRemainingAmount = model.ReceivableRemainingAmount,
                //ReceivableDepositAmount = model.ReceivableDepositAmount,
                PayableAmount = model.PayableAmount,
                PayableDepositAmount = model.ReceivableDepositAmount,
                Id = User.Identity.GetUserId(),
            };

            //This Invoices bookings collections get this invoice object added to thier Invoice collection
            foreach (Booking bk in model.Bookings)
            {
                model.ReceivableDepositAmount += bk.Subtotal * db.DepositRates.Select(d => d.DepositRateValue).FirstOrDefault();
                //bk.Invoices.Add(newInvoice); b-i many to many
                bk.Invoice = newInvoice;
            };
            newInvoice.ReceivableDepositAmount = model.ReceivableDepositAmount;
            newInvoice.Bookings = model.Bookings;
            Session["NewInvoice"] = newInvoice;
            return View(model);

        }

        // GET: BookingViewModels/ConfirmVisits/5
        /// <summary>
        /// Get booking using the passed id
        /// saves it as a tempdata BookingOriginal
        /// Get invoice using the BookingId of the booking
        /// saves it as a tempdata InvoiceOriginal
        /// retrn it to the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Confirm Booking</returns>
        /// <includesource>yes</includesource>
        public ActionResult ConfirmVisit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", booking.InvoiceId);
            ViewBag.UnitPriceId = new SelectList(db.UnitPrices, "UnitPriceId", "UnitPriceValue", booking.UnitPriceId);
            TempData["BookingOriginal"] = booking;
            var selectedInvoriginal = (Invoice)db.Invoices.Where(i => i.InvoiceId == booking.InvoiceId).FirstOrDefault();
            TempData["InvoiceOriginal"] = selectedInvoriginal;
            return View(booking);
        }

        // POST: Bookings/Edit/5
        /// <summary>
        /// Get posted booking data,depending on the booking status chosen manipulate data and redirect to apprprirate view
        /// if booking status is unchanged or curtailed/extended without adjustment hrs input returns current view with error
        /// if booking status is visited, startDate, endDate, and item description updated and the booking saved to database Returns AdminBooking View
        /// if booking status is extended, subtotal, deposit cancellationfee, startDate, endDate, and item description of booking and invoice remaining amount 
        /// save that updated data to TempData "BookingUpdated" and Tempdta "InvoiceUpdated" and pass it to the VisitRefundConfirmation View
        /// if booking status is curtailed and amount to be reduced is more than paid deposit ammount, subtotal, deposit cancellationfee, startDate, endDate, and item description of booking and invoice remaining amount
        /// save that updated data to TempData "BookingUpdated" and Tempdta "InvoiceUpdated" and pass it to the VisitRefundConfirmation View.
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>if bookig status is Visited or extended return user backt to InvoiceIndex, 
        ///                if booking status is Cancelled or Curtained and amount to be reduced is more than paid deposit ammount, returns RefundConfirmation View</returns>
        /// <includesource>Yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmVisit([Bind(Include = "BookingId,BookedDate,BookingStartDateTime,BookingEndDateTime,BookingStatus,BookingAdjustmentHrs,ItemDescription,Subtotal,BookingDeposit,BookingCancellationFee,UnitPriceId,Id,InvoiceId")] Booking booking)
        {
            var bookingOriginal = TempData["BookingOriginal"] as Booking;
            var invoiceOriginal = TempData["InvoiceOriginal"] as Invoice;

            if (booking.BookingStatus == BookingStatus.Booked)
            {
                TempData["ConfirmVisitError"] = "Booking status must be changed";
                return RedirectToAction("ConfirmVisit");
            }
            if ((booking.BookingStatus == BookingStatus.Curtailed || booking.BookingStatus == BookingStatus.Extended) && booking.BookingAdjustmentHrs == 0)
            {
                TempData["ConfirmVisitError"] = "You must input adjustment hours";
                return RedirectToAction("ConfirmVisit");
            }
            //to booking, allocate subtotal, deposit, cancellationfee value depending on the booking status chosen
            //save item description to booking as it is null at the moment
            booking.ItemDescription = bookingOriginal.ItemDescription;
            booking.Id = bookingOriginal.Id;
            booking.BookingId = bookingOriginal.BookingId;
            //get depostirate
            var depositrate = (float)db.DepositRates.SingleOrDefault().DepositRateValue;
            //check if unit price has changed in model, 
            //if so set current unit price as model to calculate else use original one.
            var orginalBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == bookingOriginal.UnitPriceId).FirstOrDefault().UnitPriceValue;
            var thisBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == booking.UnitPriceId).FirstOrDefault().UnitPriceValue;
            var currentUnitPrice = 0f;
            if (orginalBkgUnitPrice == thisBkgUnitPrice)
            { currentUnitPrice = orginalBkgUnitPrice; }
            else { currentUnitPrice = thisBkgUnitPrice; }
            if (booking.BookingStatus == BookingStatus.Visited)
            {
                booking.Subtotal = bookingOriginal.Subtotal;
                booking.BookingDeposit = bookingOriginal.BookingDeposit;
                booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
                if (ModelState.IsValid)
                {
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
            }
            if (booking.BookingStatus == BookingStatus.Extended)
            {
                booking.Subtotal = bookingOriginal.Subtotal + ((float)booking.BookingAdjustmentHrs * currentUnitPrice);
                booking.BookingDeposit = bookingOriginal.BookingDeposit;
                booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
                if (ModelState.IsValid)
                {
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
            }

            if (booking.BookingStatus == BookingStatus.Curtailed)
            {
                var amountToBeDeducted = (float)booking.BookingAdjustmentHrs * currentUnitPrice;
                var remainingAmount = (float)bookingOriginal.Subtotal - (float)bookingOriginal.BookingDeposit;
                if (amountToBeDeducted > remainingAmount)
                {
                    booking.Subtotal = bookingOriginal.Subtotal - bookingOriginal.Subtotal;
                    booking.BookingDeposit = bookingOriginal.BookingDeposit - (amountToBeDeducted - remainingAmount);
                    booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
                }
                else
                {
                    booking.Subtotal = bookingOriginal.Subtotal - amountToBeDeducted;
                    booking.BookingDeposit = bookingOriginal.BookingDeposit;
                    booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
                    if (ModelState.IsValid)
                    {
                        db.Entry(booking).State = EntityState.Modified;
                        db.SaveChanges();
                        //return RedirectToAction("Index");
                    }
                }
            }
            if (booking.BookingStatus == BookingStatus.Cancelled)
            {
                booking.Subtotal = bookingOriginal.Subtotal - bookingOriginal.Subtotal;
                booking.BookingDeposit = bookingOriginal.BookingDeposit - bookingOriginal.BookingDeposit;
                booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
            }


            //to booking, allocate subtotal, deposit, cancellationfee value depending on the booking status chosen
            //find the invoice record that needs to be updated by the id

            var selectedInv = (Invoice)db.Invoices.Where(i => i.InvoiceId == booking.InvoiceId).FirstOrDefault();
            if (booking.BookingStatus == BookingStatus.Visited)
            {   //add remaining aount
                selectedInv.ReceivableRemainingAmount = selectedInv.ReceivableRemainingAmount + (booking.Subtotal - bookingOriginal.BookingDeposit);


                if (ModelState.IsValid)
                {
                    db.Entry(selectedInv).State = EntityState.Modified;
                    db.SaveChanges();

                    //to find out if it is ok to send out invoice
                    //check if total booking subtotal amount equals to total invoiced amount
                    float subtotalSumOfBookings = 0f;
                    selectedInv.Bookings.ToList().ForEach(i => subtotalSumOfBookings += i.Subtotal);
                    float invoicedSumOfInvoice
                    = selectedInv.ReceivableDepositAmount - selectedInv.PayableDepositAmount
                    + selectedInv.ReceivableRemainingAmount - selectedInv.PayableAmount + selectedInv.InvoiceCancellationFee;
                    if (invoicedSumOfInvoice == subtotalSumOfBookings)
                    {
                        //send Invoice and payment request email
                        string callbackUrl = await SendInvoiceUrlEmailTokenAsync(invoiceOriginal.Id, invoiceOriginal.InvoiceId, "Your Invoice and payment request from Alfa Accounting");
                    }
                    return RedirectToAction("InvoiceIndex");
                }
            }
            if (booking.BookingStatus == BookingStatus.Extended)
            {
                var originalRemainingAmount = (float)(bookingOriginal.Subtotal - bookingOriginal.BookingDeposit);
                var aditionalcharge = (float)(booking.BookingAdjustmentHrs * currentUnitPrice);
                selectedInv.ReceivableRemainingAmount = selectedInv.ReceivableRemainingAmount + aditionalcharge + originalRemainingAmount;
                if (ModelState.IsValid)
                {
                    db.Entry(selectedInv).State = EntityState.Modified;
                    db.SaveChanges();
                    //send Invoice and payment request email
                    string callbackUrl = await SendInvoiceUrlEmailTokenAsync(invoiceOriginal.Id, invoiceOriginal.InvoiceId, "Your Invoice and payment request from Alfa Accounting");
                    return RedirectToAction("InvoiceIndex");
                }
            }
            //if curtailed and if curtailed hr deduction is more than original remaining amount,
            //save amended booking and amdend invoice and pass it to create controller so that when payment is successful, 
            //the those detail are saved.
            if (booking.BookingStatus == BookingStatus.Curtailed)
            {
                var amountToBeDeducted = (float)booking.BookingAdjustmentHrs * currentUnitPrice;
                var remainingAmount = (float)bookingOriginal.Subtotal - (float)bookingOriginal.BookingDeposit;
                if (amountToBeDeducted > remainingAmount)
                {
                    selectedInv.ReceivableRemainingAmount = selectedInv.ReceivableRemainingAmount - selectedInv.ReceivableRemainingAmount;
                    selectedInv.ReceivableDepositAmount = selectedInv.ReceivableDepositAmount - (amountToBeDeducted - remainingAmount);
                    selectedInv.InvoiceCancellationFee = selectedInv.InvoiceCancellationFee + bookingOriginal.BookingCancellationFee;
                }
                else
                {
                    selectedInv.ReceivableRemainingAmount = selectedInv.ReceivableRemainingAmount + bookingOriginal.Subtotal - amountToBeDeducted;
                    selectedInv.ReceivableDepositAmount = bookingOriginal.BookingDeposit;
                    selectedInv.InvoiceCancellationFee = selectedInv.InvoiceCancellationFee + bookingOriginal.BookingCancellationFee;
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedInv).State = EntityState.Modified;
                        db.SaveChanges();
                        //send Invoice and payment request email
                        string callbackUrl = await SendInvoiceUrlEmailTokenAsync(invoiceOriginal.Id, invoiceOriginal.InvoiceId, "Your Invoice and payment request from Alfa Accounting");
                        return RedirectToAction("InvoiceIndex");

                    }
                }
            }
            if (booking.BookingStatus == BookingStatus.Cancelled)
            {
                selectedInv.PayableAmount = selectedInv.PayableAmount + selectedInv.PayableAmount;
                selectedInv.PayableDepositAmount = selectedInv.PayableDepositAmount + bookingOriginal.BookingDeposit;
                selectedInv.InvoiceCancellationFee = selectedInv.InvoiceCancellationFee + selectedInv.InvoiceCancellationFee;
            }

            //TempData["alertMessage"] = "If you cartailed your service or cancelled booking, you now need to refund the difference manually from refund section";
            if (booking.BookingStatus == BookingStatus.Curtailed || booking.BookingStatus == BookingStatus.Cancelled)
            {
                TempData["BookingOriginal"] = bookingOriginal;
                TempData["BookingUpdated"] = booking;
                TempData["InvoiceOriginal"] = invoiceOriginal;
                TempData["InvoiceUpdated"] = selectedInv;
                return RedirectToAction("VisitRefundConfirmation");
            }

            //change payemnt Id data

            //change payment history data



            ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", booking.Id);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "Id", booking.InvoiceId);
            ViewBag.UnitPriceId = new SelectList(db.UnitPrices, "UnitPriceId", "UnitPriceValue", booking.UnitPriceId);

            return View(booking);
        }

        private async Task<string> SendInvoiceUrlEmailTokenAsync(string userID, int invID, string subject)
        {
            var username = UserManager.FindById(userID).Email;
            var adminUserId = UserManager.FindByEmail("alfaacc00unting2017@gmail.com").Id;
            var callbackUrl = Url.Action("InvoiceDetail", "BookingViewModels", new { id = invID }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject, "Please confirm your Invoice by clicking <a href=\"" + callbackUrl + "\">here</a>");
            await UserManager.SendEmailAsync(adminUserId, "Invoice send to " + username, "Invoice: " + invID + "< a href =\"" + callbackUrl + "\">here</a>" + " link has been sent to " + username);

            return callbackUrl;

        }

        //Get AmendBookDate
        /// <summary>
        /// Find Booking matches with the passed Id, 
        /// Create model of a booking that both start end date is in string to work with jquery ui
        /// Find Invoice of the found Booking
        /// Pass the model to return the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>model data in Amend Book Dates View</returns>
        /// <includesource>yes</includesource>
        public ActionResult AmendBookDates(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            AmendBookDatesViewModel model = new AmendBookDatesViewModel();
            model.BookingId = booking.BookingId;
            model.BookedDate = booking.BookedDate;
            model.BookingStartDateTime = (booking.BookingStartDateTime).ToString("dd/MM/yy HH:mm");
            model.BookingEndDateTime = (booking.BookingEndDateTime).ToString("dd/MM/yy HH:mm");
            model.UnitPriceId = (int)booking.UnitPriceId;
            model.InvoiceId = (int)booking.InvoiceId;
            //ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", booking.Id);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", booking.InvoiceId);
            ViewBag.UnitPriceId = new SelectList(db.UnitPrices, "UnitPriceId", "UnitPriceValue", booking.UnitPriceId);
            TempData["BookingOriginal"] = booking;
            var selectedInvoriginal = (Invoice)db.Invoices.Where(i => i.InvoiceId == booking.InvoiceId).FirstOrDefault();
            TempData["InvoiceOriginal"] = selectedInvoriginal;
            return View(model);

        }

        //Post AmendBookDates
        /// <summary>
        /// Find Booking using BookingId passed BookingOriginal TempData
        /// and assign original data to binding local variable booking
        /// Find Invoice using Invoiceid from Invoice Original TempData
        /// and assing original data to local variable selectedInv
        /// Calcuate duration of original and updated booking
        /// if both duration is the same, startDate, endDate, 
        /// Item Description, subtotal, of the booking save as BookingUpdated TempData 
        /// if Original duration is less than increase booking duration, 
        /// updates startDate, endDate, Item Description, subtotal, of the booking 
        /// also selectedInv's Recievable deposit amount get increased that get saved as InvoiceUpdated.
        /// that get passed to AmendBookingconfirmed view
        /// if Original duration is more than updated booking duration, 
        /// updates the startDate, endDate, Item Description, subtotal, of the booking that get saved as BookingUpdated
        /// also selectedInv's Recievable deposit amount get reduced that get saved as InvoiceUpdated.
        /// then BookingOriginal, InvoiceOriginal, BookingUpdated, InvoiceUpdated get passed to AddBookingConfirmation View
        /// if not successful it returns the current view
        /// </summary>
        /// <param name="model"></param>
        /// <returns>redirect user to AmendBookingConfirmation view, if not successful returns current view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AmendBookDates([Bind(Include = "BookingId,BookedDate,BookingStartDateTime,BookingEndDateTime,UnitPriceId,Id,InvoiceId")] AmendBookDatesViewModel model)
        {   //get original booking data from temp data BookingOriginal
            var bookingOriginal = TempData["BookingOriginal"] as Booking;
            //Create booking with data model using the data from original booking as passed model has no bookingid. 
            //Start date and End date get passed as string by jquery ui date picker 
            //that needs converted to date time.
            Booking booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingOriginal.BookingId);
            booking.BookingStartDateTime = Convert.ToDateTime(model.BookingStartDateTime);
            booking.BookingEndDateTime = Convert.ToDateTime(model.BookingEndDateTime);
            booking.ItemDescription = booking.ItemDescription = booking.ItemDescription = booking.BookingStartDateTime.Date.ToString("d")
                    + " From " + booking.BookingStartDateTime.Hour + ":" + booking.BookingStartDateTime.Minute
                    + " to " + booking.BookingEndDateTime.Hour + ":" + booking.BookingEndDateTime.Minute
                    + "  " + (booking.BookingEndDateTime - booking.BookingStartDateTime).Hours + "hr" + (booking.BookingEndDateTime - booking.BookingStartDateTime).Minutes + "min";

            //get data from temp data InvoiceOriginal to get invoice inside of original booking
            var invoiceOriginal = TempData["InvoiceOriginal"] as Invoice;
            var selectedInv = (Invoice)db.Invoices.Where(i => i.InvoiceId == bookingOriginal.InvoiceId).FirstOrDefault();
            //to booking, allocate subtotal, deposit, cancellationfee value depending on the booking status chosen
            if (booking.BookingStartDateTime == bookingOriginal.BookingStartDateTime && booking.BookingEndDateTime == bookingOriginal.BookingEndDateTime)
            {
                TempData["ErrorMessage"] = "You have not changed date and time";
                return RedirectToAction("MyBooking");
            }
            if (booking.BookingStartDateTime == booking.BookingEndDateTime)
            {
                TempData["ErrorMessage"] = "Your new start date and end date was the same time";
                return RedirectToAction("MyBooking");
            }
            if (booking.BookingStartDateTime <= DateTime.Now || booking.BookingEndDateTime <= DateTime.Now)
            {
                TempData["ErrorMessage"] = "Your selected time was in the past";
                return RedirectToAction("MyBooking");
            }
            if (booking.BookingStartDateTime.Date != booking.BookingEndDateTime.Date)
            {
                TempData["ErrorMessage"] = "You can't book over two days";
                return RedirectToAction("MyBooking");
            }
            var totalduration = (booking.BookingEndDateTime-booking.BookingStartDateTime).TotalHours;
            if (totalduration > 8)
            {
                TempData["ErrorMessage"] = "You can't book more than 8 hrs at one time";
                return RedirectToAction("MyBooking");
            }

            //get depostirate
            var depositrate = (float)db.DepositRates.FirstOrDefault().DepositRateValue;
            //get original unit price value
            var orginalBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == bookingOriginal.UnitPriceId).FirstOrDefault().UnitPriceValue;
            //get original booking duration and amended booking duration to compare if the duration changed.
            var originalBookingDuration = (float)((bookingOriginal.BookingEndDateTime - bookingOriginal.BookingStartDateTime).TotalHours);
            var amendedBookingDuration = (float)((booking.BookingEndDateTime - booking.BookingStartDateTime).TotalHours);

            //if duration did not change update start date and end date without any amendment to invoice, payment, payment hisotry
            //and if amended booking duration is less than original booking hrs and before 48hrs refund the difference
            //and if amended booking during is more than original booking hrs charge extra deposit,

            ////if today - original BookingStartDateTime is less than 48hrs, show message to let user know that it incur cancelation amendment fee of £25, 
            ////and if amended Booking Start date Time is less than 48 hrs, show message to let user know that it incur cancelation amendment fee of £25,
            ////if confirmed yes add £25 of Cancellationfee
            ////var hrsBtwNowAndOriginalStartDate = (float)((bookingOriginal.BookingStartDateTime - DateTime.Now).TotalHours);
            ////var hrsBtwNewAndAmendedStartDate = (float)((booking.BookingStartDateTime - DateTime.Now).TotalHours);

            ////if ((hrsBtwNowAndOriginalStartDate <= 48 && hrsBtwNewAndAmendedStartDate <= 48) || hrsBtwNowAndOriginalStartDate <= 48|| hrsBtwNewAndAmendedStartDate <= 48)
            ////{
            ////    ViewBag.message = "your original booking hours is within 48 hrs from today, cancelling or making changes to this booking incur £25 of administration fee";
            ////    booking.BookingCancellationFee = bookingOriginal.BookingCancellationFee + 25;
            ////    selectedInv.InvoiceCancellationFee = selectedInv.InvoiceCancellationFee + 25;
            ////}

            //find out original and amended deposit to get difference between.
            var originalDeposit = originalBookingDuration * orginalBkgUnitPrice * depositrate;
            var amendedDeposit = amendedBookingDuration * orginalBkgUnitPrice * depositrate;
            //update the values of booking subtotal, booking deposit, which are null at the moment
            //and also update the values of invoice recievable deposit and invoice cancellation fee.
            if (originalBookingDuration == amendedBookingDuration)
            {
                TempData["BookingOriginal"] = bookingOriginal;
                TempData["BookingUpdated"] = booking;
                TempData["InvoiceOriginal"] = invoiceOriginal;
                TempData["InvoiceUpdated"] = selectedInv;
                return RedirectToAction("AmendBookingConfirmation");
            }
            else if (originalBookingDuration < amendedBookingDuration)
            {
                booking.Subtotal = amendedBookingDuration * orginalBkgUnitPrice;
                booking.BookingDeposit = amendedDeposit;
                selectedInv.ReceivableDepositAmount = selectedInv.ReceivableDepositAmount + (amendedDeposit - bookingOriginal.BookingDeposit);
                TempData["BookingOriginal"] = bookingOriginal;
                TempData["BookingUpdated"] = booking;
                TempData["InvoiceOriginal"] = invoiceOriginal;
                TempData["InvoiceUpdated"] = selectedInv;
                return RedirectToAction("AmendBookingConfirmation");
            }
            else if (originalBookingDuration > amendedBookingDuration)
            {
                booking.Subtotal = amendedBookingDuration * orginalBkgUnitPrice;
                booking.BookingDeposit = amendedDeposit;
                selectedInv.ReceivableDepositAmount = selectedInv.ReceivableDepositAmount - (bookingOriginal.BookingDeposit - amendedDeposit);
                TempData["BookingOriginal"] = bookingOriginal;
                TempData["BookingUpdated"] = booking;
                TempData["InvoiceOriginal"] = invoiceOriginal;
                TempData["InvoiceUpdated"] = selectedInv;
                // calculaet refund amount 
                var refundamount = 0f;
                //if original duration is more than amended booking duration the difference must be refunded.
                refundamount = (bookingOriginal.BookingDeposit - booking.BookingDeposit);
                if (refundamount <= 0)
                {
                    TempData["ErrorMessage"] = "Your end date time is smaller than start date time";
                    return RedirectToAction("MyBooking");
                }

                return RedirectToAction("AmendBookingConfirmation");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", booking.Id);
            ViewBag.UnitPriceId = new SelectList(db.UnitPrices, "UnitPriceId", "UnitPriceValue", booking.UnitPriceId);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId", booking.InvoiceId);
            return View(model);
        }

        /// <summary>
        /// Create VisitRefundViewModel model using data passed from tempdata BookingOriginal, BookingUpudated, Invoice Original, Invoice Updated
        /// that get passed to amend booking confirmation view
        /// </summary>
        /// <returns>AmendBookingConfirmation with the model data</returns>
        /// <includesource>yes</includesource>
        public ActionResult AmendBookingConfirmation()
        {

            //extract temp data passed from the AmendBookDates controller
            var bookingOriginal = TempData["BookingOriginal"] as Booking;
            var bookingUpdated = TempData["BookingUpdated"] as Booking;
            var invoiceOriginal = TempData["InvoiceOriginal"] as Invoice;
            var invoiceUpdated = TempData["InvoiceUpdated"] as Invoice;
            //in case back button is clicked
            if (bookingOriginal != null)
            {
                //put above before update and after updated booking data into list to dispaly in the view
                var listBothBookings = new List<Booking>();
                listBothBookings.Add(bookingOriginal);
                listBothBookings.Add(bookingUpdated);
                //put above before update and after updated invoice data into list to dispaly in the view
                var listBothInvoices = new List<Invoice>();
                listBothInvoices.Add(invoiceOriginal);
                listBothInvoices.Add(invoiceUpdated);
                //get user and pay history to display in the view
                ApplicationUser user = db.Users.Where(u => u.Id == bookingOriginal.Id).FirstOrDefault();
                //get payment history to display in the view
                List<PaymentHistory> listpayhis = db.PaymentHistories.Where(ph => ph.InvoiceId == bookingOriginal.InvoiceId).ToList();
                //clculate the refund or additional payment amount 
                //first get unit price
                var orginalBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == bookingOriginal.UnitPriceId).FirstOrDefault().UnitPriceValue;
                //get original booking duration and amended booking duration to compare if the duration changed.
                var originalBookingDuration = (float)((bookingOriginal.BookingEndDateTime - bookingOriginal.BookingStartDateTime).TotalHours);
                var amendedBookingDuration = (float)((bookingUpdated.BookingEndDateTime - bookingUpdated.BookingStartDateTime).TotalHours);
                ////get depostirate
                //var depositrate = (float)db.DepositRates.SingleOrDefault().DepositRateValue;
                //var originalDeposit = originalBookingDuration * orginalBkgUnitPrice*depositrate;
                //var amendedDeposit = amendedBookingDuration * orginalBkgUnitPrice*depositrate;
                var refundamount = 0f;
                var additionalpayamount = 0f;
                //if original duration is less thatn amended booking duration additional payment of the difference must be charged
                if (originalBookingDuration <= amendedBookingDuration)
                {
                    additionalpayamount = bookingUpdated.BookingDeposit - bookingOriginal.BookingDeposit + bookingUpdated.BookingCancellationFee;
                }
                //if original duration is more than amended booking duration the difference must be refunded.
                else if (originalBookingDuration > amendedBookingDuration)
                {
                    refundamount = (bookingOriginal.BookingDeposit - bookingUpdated.BookingDeposit);
                }

                //create view model reusing the visit refund viewmodel;
                VisitRefundViewModel model = new VisitRefundViewModel();

                model.ApplicationUser = user;
                model.Bookings = listBothBookings;
                model.Invoices = listBothInvoices;
                model.RefundAmount = refundamount;
                model.AdditionalPaymentAmount = additionalpayamount;
                model.PaymentHistories = listpayhis;
                TempData["AmendBookingModel"] = model;
                TempData["BookingUpdated"] = bookingUpdated;
                TempData["InvoiceUpdated"] = invoiceUpdated;
                //return the view the model data above
                return View(model);
            }
            // in case back button is clicked return view without model
            return View("MyBooking");
        }
        /// <summary>
        /// Only when additional payment amount is 0,
        /// and confirm button is clicked boolean value get passed to this action
        /// updates corresponding booking records in database
        /// </summary>
        /// <param name="answer"></param>
        /// <returns>MyBooking View</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AmendBookingConfirmation(bool answer)
        {
            if (answer == true)
            {
                var bookingUpdated = TempData["BookingUpdated"] as Booking;
                if (ModelState.IsValid)
                {
                    db.Entry(bookingUpdated).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("MyBooking");
        }


        /// <summary>
        /// creates MyBookingCancellation model using the routed booking id by finding it from the database
        /// if BookingStartDateTime is less than 48 hrs from now, add £25 of cancellation fee to the model,
        /// Saves the model as MyBookingCancellationRequest session and if cancellation fee is more than paid deposit
        /// that session is used to pass the charge fee.
        /// that model get passed to cancel booking view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>cancel booking view filled with model data</returns>
        /// <includesource>yes</includesource>
        public ActionResult CancelBooking(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userid = User.Identity.GetUserId();

            var bkg = db.Bookings.Where(b => b.BookingId == id && b.Id == userid).FirstOrDefault();

            var model = new MyBookingCancellation()
            {
                BookingId = bkg.BookingId,
                BookedDate = bkg.BookedDate,
                ItemDescription = bkg.ItemDescription,
                BookingStartDate = bkg.BookingStartDateTime,
                BookingCancellationFee = bkg.BookingCancellationFee,
                Subtotal = bkg.Subtotal,
                BookingDeposit = bkg.BookingDeposit,
                BookingStatus = "Cancelled",
                UnitPriceValue = db.UnitPrices.Find(bkg.UnitPriceId).UnitPriceValue,
                //Invoices = bkg.Invoices, i - b many to many
                Invoice = bkg.Invoice,
                ApplicationUser = bkg.ApplicationUser
            };
            //set cancellation fee if BookingStartDate is less than 48 hrs from now
            var hrsleft = (model.BookingStartDate - DateTime.Now).TotalHours;
            if (hrsleft <= 48 && hrsleft > 0)
            {
                model.BookingCancellationFee = 25f;
            }
            //get a list of booking paymet through the booking's invoice
            var paylist = new List<Payment>();
            //set payments of the invoice
            foreach (var item in bkg.Invoice.Payments/*invInbksJoinedWithPayment*/)
            {
                var pay = item.Invoice.Payments;
                if (pay != null)
                {
                    foreach (var p in pay)
                    {
                        paylist.Add(p);
                        model.Payments = paylist;
                    }
                }
            }
            //save model as a session to pass it to next controller
            Session["MyBookingCancellationRequest"] = model;
            //direct user 
            return View(model);
        }

        //public ActionResult NewBookingPaymentCancel(int? id)
        //{
        //    var gateway = config.GetGateway();
        //    var clientToken = gateway.ClientToken.generate();
        //    ViewBag.ClientToken = clientToken;

        //    var myBookingCancellationRequest = Session["MyBookingCancellationRequest"] as MyBookingCancellation;

        //    if (myBookingCancellationRequest != null)
        //    {
        //        if (myBookingCancellationRequest.BookingDeposit > 0 && myBookingCancellationRequest.BookingStatus == "Cancelled")
        //        {
        //            //extract booking deposit amount
        //            var bookingDeposit = myBookingCancellationRequest.BookingDeposit;

        //            //extract payment type 
        //            var paymentType = "";
        //            foreach (var item in myBookingCancellationRequest.Payments)
        //            {
        //                paymentType = item.PaymentType;
        //            }

        //            //extract bokingcancellationfee
        //            var bookingCancellationFee = myBookingCancellationRequest.BookingCancellationFee;

        //            //extract invoiced amounts
        //            //var invReceivableDepositAmount = 0f;
        //            //var invReceivableRemainingAmount = 0f;
        //            //var invPayableDepositAmount = 0f;
        //            //var invPayableAmount = 0f;
        //            //foreach (var item in myBookingCancellationRequest.Invoices) b-i many to many
        //            //{
        //            //    invReceivableDepositAmount = item.ReceivableDepositAmount;
        //            //    invReceivableRemainingAmount = item.ReceivableRemainingAmount;
        //            //    invPayableDepositAmount = item.PayableDepositAmount;
        //            //    invPayableAmount = item.PayableAmount;
        //            //}
        //            var invReceivableDepositAmount = myBookingCancellationRequest.Invoice.ReceivableDepositAmount;
        //            //var invReceivableRemainingAmount = myBookingCancellationRequest.Invoice.ReceivableRemainingAmount;
        //            //var invPayableDepositAmount = myBookingCancellationRequest.Invoice.PayableDepositAmount;
        //            //var invPayableAmount = myBookingCancellationRequest.Invoice.PayableAmount;
        //            if (bookingCancellationFee == 0 && (paymentType == "Deposit" || paymentType == "DepositCancelled"))
        //            {
        //                var amountValue = bookingDeposit;
        //                //check if refund value is valid amount, if yes pass the amount to the current view if not navigate user back to MyBooking
        //                if (amountValue > 0) { ViewBag.amountValue = amountValue; }
        //                else return View("MyBooking");

        //            }
        //            if (bookingCancellationFee > 0 && (paymentType == "Deposit" || paymentType == "DepositCancelled"))
        //            {
        //                var amountValue = 0f;
        //                if (bookingCancellationFee > 0 && bookingDeposit > 0)
        //                {
        //                    amountValue = bookingCancellationFee;
        //                    ViewBag.amountValue = amountValue;
        //                    return View("New");

        //                }
        //                else if (bookingCancellationFee == 0 && bookingDeposit > 0)
        //                {
        //                    amountValue = bookingDeposit;
        //                    ViewBag.amountValue = amountValue;
        //                }
        //                //check if refund value is valid amount, if yes pass the amount to the current view if not navigate user back to MyBooking
        //                if (amountValue > 0) { ViewBag.amountValue = amountValue; }
        //                else return View("MyBooking");
        //            }
        //        }
        //    }
        //    return View();
        //}





        /// <summary>
        /// Array to sotre transaction status sent by braintree
        /// </summary>
        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                             TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };
        /// <summary>
        /// Get client token to access braintree gateway,
        /// if newInvoice session data is not null, pass pay newInvoiceSeccion's recievable deposit amount as pay amount to pay newly booked deposit
        /// if MyBookingCancellationRequest session data is not null, 
        ///      if cancellation fee is more than 0 and paymentType is Deposit or DepositCancelled, pay amount is booking cancellation fee to pay cancelation fee.
        ///      if booking cancellation fee is more than 0, cancellation fee is more than Booking deposit, deposit becomes pay amount to pay cancelation fee.
        ///      if booking status is cancelled but booking deposit is more than 0, refund amount is original cancelled deposit amount to refund deposit.
        ///      ** only in this case return New view with Get Refund Post form.
        /// if tempInvoiceDetail is not null pay amount becomes tempInvoiceDetail.receibableRemainingAmount - tempInvoiceDetail.PayableAmount to pay remaining booking fee
        ///      and saves tempInvoiceDatail as TempData
        /// if invoiceUpdated or bookingUpdated is not null, amount becomes posted amount from the AmendBookingConfirmation for user to pay extra hrs deposit charge
        /// </summary>
        /// <returns> Returns New View with Pay or New view with refund amount, only when cancellation refund data passsed</returns>
        /// <includesource>yes</includesource>
        public ActionResult New(/*int? id*/)
        {
            var gateway = config.GetGateway();
            var clientToken = gateway.ClientToken.generate();
            ViewBag.ClientToken = clientToken;

            var newInvoice = Session["NewInvoice"] as Invoice;
            //var SelectedPaymentIndexViewModel = Session["SelectedPaymentIndexViewModel"] as PaymentIndexViewModel;
            var cancelledbkg = Session["MyBookingCancellationRequest"] as MyBookingCancellation;

            var tempInvoiceDetail = TempData["tempInvoiceDetailViewModel"] as InvoiceDetailViewModel;
            var bookingUpdated = TempData["BookingUpdated"] as Booking;
            var invoiceUpdated = TempData["InvoiceUpdated"] as Invoice;
            var amendBookingmodel = TempData["AmendBookingModel"] as VisitRefundViewModel;
            //new booking deposit payment from ConfirmBooking
            if (newInvoice != null && cancelledbkg == null && tempInvoiceDetail == null)
            {
                ViewBag.amountValue = newInvoice.ReceivableDepositAmount;
            }
            //canceled amount payment or refund from Cancel Booking
            if (newInvoice == null && cancelledbkg != null && tempInvoiceDetail == null)
            {
                if ((cancelledbkg.BookingCancellationFee > 0 && cancelledbkg.BookingDeposit < cancelledbkg.BookingCancellationFee) /*|| (Model.BookingDeposit > 0 && Model.BookingStatus == "Cancelled")*/)
                {
                    ViewBag.amountValue = cancelledbkg.BookingCancellationFee;
                }
                if (cancelledbkg.BookingDeposit > 0 && cancelledbkg.BookingStatus == "Cancelled")
                {
                    //extract booking deposit amount
                    var bookingDeposit = cancelledbkg.BookingDeposit;
                    //extract payment type 
                    var paymentType = "";
                    foreach (var item in cancelledbkg.Payments) { paymentType = item.PaymentType; }
                    if (cancelledbkg.BookingCancellationFee == 0 && (paymentType == "Deposit" || paymentType == "DepositCancelled"))
                    {
                        var amountValue = bookingDeposit;
                        //check if refund value is valid amount, if yes pass the amount to the current view if not navigate user back to MyBooking
                        if (amountValue > 0)
                        {
                            ViewBag.refundamountValue = amountValue;
                            //return View("NewBookingPaymentCancel");
                        }
                        else return View("MyBooking");
                    }
                    if (cancelledbkg.BookingCancellationFee > 0 && (paymentType == "Deposit" || paymentType == "DepositCancelled"))
                    {
                        var amountValue = 0f;
                        if (cancelledbkg.BookingCancellationFee > 0 && bookingDeposit > 0)
                        {
                            amountValue = cancelledbkg.BookingCancellationFee;
                            ViewBag.amountValue = amountValue;
                            //return View("New");
                        }
                        else if (cancelledbkg.BookingCancellationFee == 0 && bookingDeposit > 0)
                        {
                            amountValue = bookingDeposit;
                            ViewBag.refundamountValue = amountValue;
                            //return View("NewBookingPaymentCancel");
                        }
                        //check if refund value is valid amount, if yes pass the amount to the current view if not navigate user back to MyBooking
                        if (amountValue > 0) { ViewBag.amountValue = amountValue; }
                        else return View("MyBooking");
                    }
                }
            }
            //remaining amount payment from paymentIndex
            //var listPaymentIndexViewModels = Session["ListPaymentIndexViewModel"] as List<PaymentIndexViewModel>;
            //var listMyBookingsViewModelBase = Session["ListMyBookingsViewModelBase"] as List<MyBookingsViewModelBase>;
            //if (listPaymentIndexViewModels != null && listMyBookingsViewModelBase != null && id != null)
            //{
            //    if (listPaymentIndexViewModels != null && id != 0)
            //    {
            //        var result = listPaymentIndexViewModels.Where(l => l.InvoiceId == id).FirstOrDefault();
            //        if (result.ReceivableRemainingAmount == 0 && result.ReceivableDepositAmount != 0 && result.PayableDepositAmount == 0 && result.PayableAmount == 0)
            //        {ViewBag.amountValue = (result.Subtotal - result.BookingDeposit);}
            //        Session["ListPaymentIndexViewModels"] = null;
            //        Session["SelectedPaymentIndexViewModels"] = result;
            //    }
            //    if (listMyBookingsViewModelBase != null && id != 0)
            //    {
            //        var result = listMyBookingsViewModelBase.Where(l => l.InvoiceId == id).FirstOrDefault();
            //        if (result.ReceivableRemainingAmount == 0 && result.ReceivableDepositAmount != 0 && result.PayableDepositAmount == 0 && result.PayableAmount == 0)
            //        {ViewBag.amountValue = (result.Subtotal - result.BookingDeposit);}
            //        Session["ListMyBookingsViewModelBase"] = null;
            //        Session["SelectedPaymentIndexViewModels"] = result;
            //    }
            //}
            //remaining amount payment from IndexDetail
            if (newInvoice == null && cancelledbkg == null && tempInvoiceDetail != null)
            {
                ViewBag.amountValue = tempInvoiceDetail.ReceivableRemainingAmount - tempInvoiceDetail.PayableAmount;
                TempData["tempInvoiceDetailViewModel"] = tempInvoiceDetail;
            }
            //amended amount payment from AmendBookingConfirmation
            if (invoiceUpdated != null && bookingUpdated != null)
            {   //amount posted from AmendBookingConfirmation when amended hrs is more than original and user asked to pay the extra deposit.
                var amount = Convert.ToDecimal(Request["amount"]);
                ViewBag.amountValue = amount;
                TempData["BookingUpdated"] = bookingUpdated;
                TempData["InvoiceUpdated"] = invoiceUpdated;
            }

            return View();
        }


        //public ActionResult NewPayRemaining(int? id)
        //{
        //    var gateway = config.GetGateway();
        //    var clientToken = gateway.ClientToken.generate();
        //    ViewBag.ClientToken = clientToken;

        //    var listPaymentIndexViewModels = Session["ListPaymentIndexViewModel"] as List<PaymentIndexViewModel>;
        //    var listMyBookingsViewModelBase = Session["ListMyBookingsViewModelBase"] as List<MyBookingsViewModelBase>;

        //    if (listPaymentIndexViewModels != null && id != 0)
        //    {
        //        var result = listPaymentIndexViewModels.Where(l => l.InvoiceId == id).FirstOrDefault();

        //        if (result.ReceivableRemainingAmount == 0 && result.ReceivableDepositAmount != 0 && result.PayableDepositAmount == 0 && result.PayableAmount == 0)
        //        {
        //            ViewBag.amountValue = (result.Subtotal - result.BookingDeposit);
        //        }

        //        Session["ListPaymentIndexViewModels"] = null;
        //        Session["SelectedPaymentIndexViewModels"] = result;
        //    }
        //    if (listMyBookingsViewModelBase != null && id != 0)
        //    {
        //        var result = listMyBookingsViewModelBase.Where(l => l.InvoiceId == id).FirstOrDefault();

        //        if (result.ReceivableRemainingAmount == 0 && result.ReceivableDepositAmount != 0 && result.PayableDepositAmount == 0 && result.PayableAmount == 0)
        //        {
        //            ViewBag.amountValue = (result.Subtotal - result.BookingDeposit);
        //        }

        //        Session["ListMyBookingsViewModelBase"] = null;
        //        Session["SelectedPaymentIndexViewModels"] = result;
        //    }

        //    return View("New");
        //}


        /// <summary>
        /// Open Gateway to the Braintree and make payment of the passed amount and submit for the settlement
        /// when successful and if NewInvoice Session data is not null saves the new booking 
        ///                     to the Boooking Invoice, Payemnt and Payment History databases and clear shopping cart item.
        ///                     if tempInvoiceDetailVeiwmodel TempData is not null saves the remainging payment amount related update 
        ///                     to the Invoice, Booking, Payemnt and Payment History databases
        ///                     if MyBookingCancelationRequrest Session data is not null, cancelation related payment amount updated on 
        ///                     booking, invoice, payment and paymentHisotry database
        ///                     if BookingUpdated and InvoiceUpdated TempData that is from amdend booking acion is not null, 
        ///                     appropriate extra depsit charge or cancellation fee or reduced hrs deposit refund related change get amended to 
        ///                     Booking, invoice, payemt and paymenthistory database.
        /// </summary>
        /// <returns> returns show view</returns>
        /// <includesource>yes</includesource>
        public ActionResult Create()
        {
            var gateway = config.GetGateway();
            Decimal amount;

            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch (FormatException e)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("New");
            }

            var nonce = Request["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                //                var listNewBookingToBeSavedToDb = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
                //if new deposit payment is getting paid passed from confirm booking
                var newInvoice = Session["NewInvoice"] as Invoice;
                if (newInvoice != null)
                {
                    //temporality empty the booking so that entity framework change tracker can save Invoice records in invoice table.
                    var newBkgs = newInvoice.Bookings;
                    newInvoice.Bookings = null;
                    db.Invoices.Add(newInvoice);
                    db.SaveChanges();
                    //save the invoice in those booking records in database many to many
                    foreach (var ngkg in newBkgs)
                    {
                        //db.Bookings.SingleOrDefault(b => b.ItemDescription == ngkg.ItemDescription).Invoices.Add(newInvoice); b-i many to many
                        db.Bookings.SingleOrDefault(b => b.ItemDescription == ngkg.ItemDescription).Invoice = newInvoice;
                        db.SaveChanges();
                    };

                    //save payemnt detail to the database 
                    var userid = User.Identity.GetUserId();
                    var newPayment = new Payment()
                    {
                        PaymentDateTime = DateTime.Now,
                        //CreditAmount = (float)amount,
                        //DebitAmount = 0f,
                        CreditDepositAmount = (float)amount,
                        DebitDepositAmount = 0f,
                        CreditRemainingAmount = 0f,
                        DebitRemainingAmount = 0f,
                        CreditCancellationFee = 0f,
                        Id = userid,
                        InvoiceId = newInvoice.InvoiceId,
                        PaymentType = "Deposit",
                        TransactionId = transaction.Id
                    };
                    db.Payments.Add(newPayment);
                    db.SaveChanges();

                    //Create newPaymentHistory
                    var newPaymentHistory = new PaymentHistory()
                    {
                        PaymentDateTime = DateTime.Now,
                        CreditAmount = (float)amount,
                        DebitAmount = 0f,
                        Id = userid,
                        PaymentId = newPayment.PaymentId,
                        InvoiceId = newInvoice.InvoiceId,
                        PaymentType = PaymentType.Deposit,
                        TransactionId = transaction.Id
                    };
                    db.PaymentHistories.Add(newPaymentHistory); ;
                    db.SaveChanges();

                    //if bookings that matches booking id in cartItems does not have invoices allocated at this point,
                    // remove them from Bookings database table,
                    //getcart items first
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    var cartItems = cart.GetCartItems();
                    //remove cart item and booking if stil does not allocated invoices
                    foreach (var cartItem in cartItems)
                    {
                        var cartItemBooking = db.Bookings.SingleOrDefault(b => b.BookingId == cartItem.BookingId);
                        //if (cartItemBooking.Invoices == null) b-i many to many
                        if (cartItemBooking.Invoice == null)
                        {
                            //cart.RemoveFromCart(cartItem.CartRecordId);
                            db.Bookings.Remove(cartItemBooking);
                        }
                    }
                    //empty cart
                    cart.EmptyCart();
                }
                //if remaining amount is getting paid 
                /*var selectedPaymentIndexViewModels = Session["SelectedPaymentIndexViewModels"] as PaymentIndexViewModel;*/
                var tempInvoiceDetail = TempData["tempInvoiceDetailViewModel"] as InvoiceDetailViewModel;
                if (tempInvoiceDetail != null /*|| selectedPaymentIndexViewModels != null*/)
                {
                    Invoice selectedInvoice = null;
                    //if (selectedPaymentIndexViewModels != null)
                    //{
                    //    selectedInvoice = db.Invoices.Find(selectedPaymentIndexViewModels.InvoiceId);
                    //}
                    //else
                    {
                        selectedInvoice = db.Invoices.Find(tempInvoiceDetail.InvoiceId);
                    }
                    if (selectedInvoice == null)
                    {
                        return HttpNotFound();
                    }
                    //find out sub total and deposit amount of bookings first
                    var TotalbkgSubT = 0f;
                    var TotalbkgDeposit = 0f;
                    foreach (var item in selectedInvoice.Bookings)
                    {
                        if (item.BookingStatus != BookingStatus.Cancelled)
                        {
                            TotalbkgSubT += item.Subtotal;
                            TotalbkgDeposit += item.BookingDeposit;
                        }
                    }
                    //insert recievable remaining amount to the selected Invoice
                    selectedInvoice.ReceivableRemainingAmount = TotalbkgSubT - TotalbkgDeposit;

                    // save it to the selectedInvoice modification to database
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedInvoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    Session["updatedInvoice"] = selectedInvoice;

                    //find payment id
                    var paymentid = db.Payments.FirstOrDefault(p => p.InvoiceId == tempInvoiceDetail.InvoiceId).PaymentId;

                    var selectedPayment = db.Payments.Find(paymentid);
                    if (selectedPayment == null)
                    {
                        return HttpNotFound();
                    }
                    selectedPayment.CreditRemainingAmount = (float)amount;
                    selectedPayment.PaymentType = "Remaining";
                    //selectedPayment.CreditAmount = (float)amount;

                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedPayment).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var userid = User.Identity.GetUserId();
                    var newPaymentHistory = new PaymentHistory()
                    {
                        PaymentDateTime = DateTime.Now,
                        CreditAmount = (float)amount,
                        DebitAmount = 0f,
                        Id = userid,
                        PaymentId = selectedPayment.PaymentId,
                        InvoiceId = selectedInvoice.InvoiceId,
                        PaymentType = PaymentType.Remaining,
                        TransactionId = transaction.Id
                    };
                    db.PaymentHistories.Add(newPaymentHistory); ;
                    db.SaveChanges();
                    //var userid = User.Identity.GetUserId();
                    //var newPayment = new Payment()
                    //{
                    //    PaymentDateTime = DateTime.Now,
                    //    CreditAmount = (float)amount,
                    //    DebitAmount = 0f,
                    //    CreditDepositAmount = 0f,
                    //    DebitDepositAmount = 0f,
                    //    CreditRemainingAmount = (float)amount,
                    //    DebitRemainingAmount = 0f,
                    //    CreditCancellationFee = 0f,
                    //    Id = userid,
                    //    InvoiceId = selectedInvoice.InvoiceId,
                    //    PaymentType = "Remaining",
                    //    TransactionId = transaction.Id
                    //};
                    //db.Payments.Add(newPayment);
                    //db.SaveChanges();
                    Session["SelectedPaymentIndexViewModels"] = null;
                }

                // cancellation fee getting paid
                var myBookingCancellationRequest = Session["MyBookingCancellationRequest"] as MyBookingCancellation;
                if (myBookingCancellationRequest != null)
                {
                    var selectedBooking = db.Bookings.Find(myBookingCancellationRequest.BookingId);
                    if (selectedBooking == null)
                    {
                        return HttpNotFound();
                    }
                    //change the booking status to "cancelled and assign cancellation fee of 25 to booking cancellation
                    selectedBooking.BookingStatus = BookingStatus.Cancelled;
                    selectedBooking.BookingCancellationFee = (float)amount;
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedBooking).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //find invoice id 
                    var invid = myBookingCancellationRequest.Invoice.InvoiceId;
                    var selectedInvoice = db.Invoices.Find(invid);
                    if (selectedInvoice == null)
                    {
                        return HttpNotFound();
                    }
                    selectedInvoice.InvoiceCancellationFee = (float)amount;
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedInvoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //find payment id
                    var paymentid = db.Payments.FirstOrDefault(p => p.InvoiceId == myBookingCancellationRequest.Invoice.InvoiceId).PaymentId;
                    //foreach (var item in selectedInvoice.Payments)
                    //{
                    //    if (item.PaymentType == "Deposit")
                    //    {
                    //        paymentid = item.PaymentId;
                    //    }
                    //}
                    var selectedPayment = db.Payments.Find(paymentid);
                    if (selectedPayment == null)
                    {
                        return HttpNotFound();
                    }
                    selectedPayment.CreditCancellationFee = selectedPayment.CreditCancellationFee + (float)amount;

                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedPayment).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    var userid = User.Identity.GetUserId();
                    var newPaymentHistory = new PaymentHistory()
                    {
                        PaymentDateTime = DateTime.Now,
                        CreditAmount = (float)amount,
                        DebitAmount = 0f,
                        Id = userid,
                        PaymentId = selectedPayment.PaymentId,
                        InvoiceId = selectedInvoice.InvoiceId,
                        PaymentType = PaymentType.CancellationFee,
                        TransactionId = transaction.Id
                    };
                    db.PaymentHistories.Add(newPaymentHistory); ;
                    db.SaveChanges();
                    //var userid = User.Identity.GetUserId();
                    //var newPayment = new Payment()
                    //{
                    //    PaymentDateTime = DateTime.Now,
                    //    CreditAmount = (float)amount,
                    //    DebitAmount = 0f,
                    //    //CreditDepositAmount = 0f,
                    //    //DebitDepositAmount = 0f,
                    //    //CreditRemainingAmount = 0f,
                    //    //DebitRemainingAmount = 0f,
                    //    //CreditCancellationFee = (float)amount,
                    //    Id = userid,
                    //    InvoiceId = selectedInvoice.InvoiceId,
                    //    PaymentType = "CancellationFee",
                    //    TransactionId = transaction.Id
                    //};
                    //db.Payments.Add(newPayment);
                    //db.SaveChanges();
                }

                //if amend Booking aditional payment is getting paid
                var bookingUpdated = TempData["BookingUpdated"] as Booking;
                var invoiceUpdated = TempData["InvoiceUpdated"] as Invoice;

                if (invoiceUpdated != null)
                {
                    //extract payment that matches with the invoiceid from the invoiceUpdated
                    Payment thisPayment = db.Payments.Where(p => p.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                    var thisPaymentid = thisPayment.PaymentId;
                    {
                        // set selected payemnt's payment type to Deposit cancelled
                        //and put amount in the payment debit amount
                        if (thisPayment == null)
                        { return HttpNotFound(); }

                        if (invoiceUpdated.InvoiceCancellationFee > 0)
                        { thisPayment.CreditCancellationFee = (float)amount; }
                        else { thisPayment.CreditDepositAmount = thisPayment.CreditDepositAmount + (float)amount; }

                        //if (invoiceUpdated != null && bookingUpdated.BookingStatus == BookingStatus.Cancelled) { thisPayment.PaymentType = "DepositCancelled"; }
                        if (ModelState.IsValid)
                        {
                            db.Entry(thisPayment).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        string userid = User.Identity.GetUserId();
                        var newPaymentHistory = new PaymentHistory()
                        {
                            PaymentDateTime = DateTime.Now,
                            CreditAmount = (float)amount,
                            DebitAmount = 0f,
                            Id = userid,
                            PaymentId = thisPaymentid,
                            InvoiceId = invoiceUpdated.InvoiceId,
                            PaymentType = PaymentType.Deposit,
                            TransactionId = transaction.Id
                        };
                        db.PaymentHistories.Add(newPaymentHistory); ;
                        db.SaveChanges();


                        //save updated invoice on to database
                        Invoice thisInvoice = db.Invoices.Where(i => i.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                        thisInvoice.ReceivableDepositAmount = invoiceUpdated.ReceivableDepositAmount;
                        thisInvoice.ReceivableRemainingAmount = invoiceUpdated.ReceivableRemainingAmount;
                        thisInvoice.PayableDepositAmount = invoiceUpdated.PayableDepositAmount;
                        thisInvoice.PayableAmount = invoiceUpdated.PayableAmount;

                        if (ModelState.IsValid)
                        {
                            db.Entry(thisInvoice).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        if (bookingUpdated == null)
                        {
                            return HttpNotFound();
                        }
                        Booking thisbooking = db.Bookings.Where(b => b.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                        thisbooking.BookingStartDateTime = bookingUpdated.BookingStartDateTime;
                        thisbooking.BookingEndDateTime = bookingUpdated.BookingEndDateTime;
                        thisbooking.ItemDescription = bookingUpdated.ItemDescription;
                        thisbooking.BookingStatus = bookingUpdated.BookingStatus;
                        thisbooking.Subtotal = bookingUpdated.Subtotal;
                        thisbooking.BookingDeposit = bookingUpdated.BookingDeposit;
                        thisbooking.BookingAdjustmentHrs = bookingUpdated.BookingAdjustmentHrs;
                        thisbooking.BookingCancellationFee = bookingUpdated.BookingCancellationFee;
                        thisbooking.UnitPriceId = bookingUpdated.UnitPriceId;
                        if (ModelState.IsValid)
                        {
                            db.Entry(thisbooking).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    TempData["BookingUpdated"] = bookingUpdated;
                }

                return RedirectToAction("Show", new { id = transaction.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;


                return RedirectToAction("New");
            }

        }

        /// <summary>
        /// Gets the transaction id and find transaction status info and displays
        /// Calls for the payment confirmation email function before return view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Show view</returns>
        /// <includesource>yes</includesource>
        public async Task<ActionResult> Show(String id)
        {
            var gateway = config.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ".";
            };

            ViewBag.Transaction = transaction;
            TempData["Transaction"] = transaction;

            //send payemnt confirmation email
            var sendnullstring = await SendPaymentConfirmationEmailTokenAsync(User.Identity.GetUserId(), "Your payment confirmation from Alfa Accounting");

            return View();
        }
        /// <summary>
        /// it ignores the user id and subejct passed, userId and subject get extracted from the passed sessions
        /// always send user and admin related emails each.
        /// if NewInvoice Session data is not null booking detail and user detail is extracted from it, and new booking confirmation email send to both user and admin.
        /// if updatedInvoiceCancelledkg is not null, booking cancellation by admin. booking detail and user detail is extracted from it, and  messagge get sent to both user and admin. 
        /// if MyBookingCancellationRequest is not null, Booking cancellation by user. booking detail and user detail is extracted from it, and message get sent to both user and admin.
        /// if BookingUpdated is not null, Booking ajustment done by admin. booking detail and user detail is extracted from it. message sent to both user and admin.
        /// if updatedInvoice is not null, Booking amendment done by customer. booking detail and user detail is extracted from it. payment confirmation message sent to both
        /// if InvoiceUpdatedDebited is not null, Booking amendment done by customer with refund. booking detail and user detail is extracted from it. refund confirmation message sent to both
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="subject"></param>
        /// <returns>null</returns>
        /// <includesource>yes</includesource>
        private async Task<string> SendPaymentConfirmationEmailTokenAsync(string userID, string subject)
        {
            var adminUserId = UserManager.FindByEmail("alfaacc00unting2017@gmail.com").Id;
            var trans = TempData["Transaction"] as Transaction;
            //var bkgs = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
            var newInv = Session["NewInvoice"] as Invoice;
            var cancelledbkgbyAdmin = Session["updatedInvoiceCancelledbkg"] as Invoice;
            var cancelledbkg = Session["MyBookingCancellationRequest"] as MyBookingCancellation;
            var bookingUpdated = TempData["BookingUpdated"] as Booking;
            var invs = Session["updatedInvoice"] as Invoice;
            var invoiceUpdatedDebited = TempData["InvoiceUpdatedDebited"] as Invoice;

            //when multiple bookings made, create a string list which stores booking item descriptions that get sent via email.
            var itemDescription = new List<string>();
            if (newInv != null) { foreach (var inv in newInv.Bookings) { itemDescription.Add(inv.ItemDescription); } }
            Session["NewInvoice"] = null;
            if (invs != null) { foreach (var item in invs.Bookings) { itemDescription.Add(item.ItemDescription); } }
            //empty inv session
            Session["updatedInvoice"] = null;
            if (bookingUpdated != null) { itemDescription.Add(bookingUpdated.ItemDescription); }
            //conver to booking detail description in to strings.
            string descriptions = string.Join(",", itemDescription);

            //string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            if (cancelledbkg != null)
            {
                var username = UserManager.FindById(cancelledbkg.Invoice.Id).Companyname;
                if (cancelledbkg.BookingCancellationFee == 0)
                {
                    await UserManager.SendEmailAsync(cancelledbkg.Invoice.Id, "Alfa Accounting: Confirmation of Booking Cancellation with refunt", trans.Amount + " has been refuned to your credit card account for the booking on " + cancelledbkg.ItemDescription /*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
                    await UserManager.SendEmailAsync(adminUserId, "Booking Cancellation without Fees", trans.Amount + " has been refuned to " + username + " credit card account for the cancelled booking on " + cancelledbkg.ItemDescription /*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
                }
                else
                {
                    await UserManager.SendEmailAsync(cancelledbkg.Invoice.Id, "Alfa Accounting: Confirmation of Booking Cancellation with Fees", "Cancellation fee of £ " + trans.Amount + " has been paid, and booking on " + cancelledbkg.ItemDescription + " has been cancelled.");
                    await UserManager.SendEmailAsync(adminUserId, "Booking Cancellation with Fees", "Cancellation fee of £ " + trans.Amount + " has been paid by " + username + ", and booking on " + cancelledbkg.ItemDescription + " has been cancelled.");
                }
                //empty cancelledbkg session
                Session["MyBookingCancellationRequest"] = null;
            }
            else if (invoiceUpdatedDebited != null)
            {
                var username = UserManager.FindById(invoiceUpdatedDebited.Id).Companyname;
                await UserManager.SendEmailAsync(invoiceUpdatedDebited.Id, "Alfa Accounting: Booking Amendment and Refund", "We have refundedd £ " + trans.Amount + "for the booking amendment on " + descriptions);
                await UserManager.SendEmailAsync(adminUserId, "Booking Amendment and Refund", "We have refundedd £ " + trans.Amount + "for the booking amendment in " + descriptions + "to " + username);
            }
            else if (/*bkgs != null ||*/ newInv != null)
            {
                var username = UserManager.FindById(userID).Companyname;
                await UserManager.SendEmailAsync(userID, subject, "Thank you very much for your payment of £ " + trans.Amount + "for the booking on " + descriptions /*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
                await UserManager.SendEmailAsync(adminUserId, "Booking Confirmation", "The payment of £ " + trans.Amount + "for the booking on " + descriptions + "paid by " + username /*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
            }
            else if (invs != null)
            {
                var username = UserManager.FindById(invs.Id).Companyname;
                await UserManager.SendEmailAsync(userID, subject, " Thank you very much for your payment of £ " + trans.Amount + "for the booking on " + descriptions);
                await UserManager.SendEmailAsync(adminUserId, "Remaining amount paid for booking for " + descriptions, "The payment of £ " + trans.Amount + "for the booking on " + descriptions + "paid by " + username);
            }
            else if (cancelledbkgbyAdmin != null)
            {
                var username = UserManager.FindById(cancelledbkgbyAdmin.Id).Companyname;
                await UserManager.SendEmailAsync(cancelledbkgbyAdmin.Id, "Alfa Accouting: Your booking cancellation and refund confirmation", "  £ " + trans.Amount + " has been refunded to you for the booking on " + descriptions + "by Alfa Accounting Administrator");
                await UserManager.SendEmailAsync(adminUserId, "Booking Cancellation by Alfa Accounting ", "The payment of £ " + trans.Amount + "for the booking on " + descriptions + " refunded to " + username + "by Alfa Accouting Admin");
            }
            else if (bookingUpdated != null)
            {
                var username = UserManager.FindById(bookingUpdated.Id).Companyname;
                await UserManager.SendEmailAsync(bookingUpdated.Id, "Your payemnt confirmation", " Thank you very much for your payment of £ " + trans.Amount + "for the booking on " + descriptions /*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
                await UserManager.SendEmailAsync(adminUserId, "Booking Amendment with Additional Fee paid", "The payment of £ " + trans.Amount + "for the booking on " + descriptions + " paid by " + username/*itemDescription.Aggregate((current, next) => current + ", " + next)*/);
            }


            return null;
        }

        /// <summary>
        /// allow access to the http owin context's properties
        /// </summary>
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
        /// <summary>
        /// Gets all bookings in the database by BookingStartdate.
        /// create AdminBookingsViewModel model using the data pass it to the view
        /// allows fileter model data between start and endDate paramenter date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>AdminBookingView</returns>
        /// <includesource>yes</includesource>
        public ActionResult AdminBooking(string startDate, string endDate)
        {
            AdminBookingsViewModel model = new AdminBookingsViewModel();
            if ((startDate != null && endDate != null))
            {
                if (startDate != "" && endDate != "")
                {
                    DateTime startDateT = Convert.ToDateTime(startDate);
                    DateTime endDateTemp = Convert.ToDateTime(endDate);
                    DateTime endDateT = endDateTemp.AddDays(1);

                    if (endDateT > startDateT)
                    {
                        model.Bookings = db.Bookings.OrderBy(b => b.BookingStartDateTime).Include(i => i.ApplicationUser).Where(b => b.BookingStartDateTime >= startDateT && b.BookingStartDateTime <= endDateT);
                        return View(model);
                    }
                    else model.Bookings = db.Bookings.OrderBy(b => b.BookingStartDateTime).Include(i => i.ApplicationUser);
                }
            }
          
            model.Bookings = db.Bookings.OrderBy(b => b.BookingStartDateTime).Include(b=>b.ApplicationUser).ToList();
           

            return View(model);

        }
        /// <summary>
        /// Get BookingStateDateTime is less tha 24hrs from now.
        /// and pass that list to the View
        /// </summary>
        /// <returns>Returns SendApptConfirmation View</returns>
        /// <includesource>yes</includesource>
        public ActionResult SendApptConfirmation()
        {
            var bookings = db.Bookings.Where(b => (DbFunctions.DiffHours(DateTime.Now, b.BookingStartDateTime)) <= 24 && b.VisitConfirmationSent == false).ToList();


            return View(bookings);
        }

        /// <summary>
        /// Get BookingStateDateTime is less tha 24hrs from now and send confirmation text to all users
        /// </summary>
        /// <returns> null</returns>
        /// <includesource>yes</includesource>
        public async Task<ActionResult> SendApptConfirmationText()
        {
            var bookings = db.Bookings.Where(b => (DbFunctions.DiffHours(DateTime.Now, b.BookingStartDateTime)) <= 24 && b.VisitConfirmationSent == false).ToList();

            foreach (var bk in bookings)
            {
                var user = UserManager.FindById(bk.Id);
                bk.VisitConfirmationSent = true;
                if (ModelState.IsValid)
                {
                    db.Entry(bk).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //// Generate the token and send it
                //var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);

                if (UserManager.SmsService != null)
                {
                    var callbackUrl1 = Url.Action("BookDates", "BookingViewModels", null, protocol: Request.Url.Scheme);
                    var message = new IdentityMessage
                    {
                        Destination = user.PhoneNumber,
                        Body = "Your appointment with Alfa Accounting is on : " + bk.BookingStartDateTime  /*+ code*/
                        + ". if your circumstance change and needs to amend booking date please do so online, if you want to cancel booking at this point you will be charged £25 of cancellation fee"
                    };
                    await UserManager.SmsService.SendAsync(message);
                }
                var callbackUrl2 = Url.Action("BookDates", "BookingViewModels", new { id = bk.InvoiceId }, protocol: Request.Url.Scheme);
                string subject = "Visit Confirmation form Alfa Accounting";
                await UserManager.SendEmailAsync(bk.Id, subject, "Your appointment with Alfa Accounting is on : " + bk.BookingStartDateTime  /*+ code*/
                        + ". if your circumstance change and needs to amend booking date please do so online by clicking <a href=\"" + callbackUrl2
                        + "\">here</a>, if you want to cancel booking at this point you will be charged £25 of cancellation fee");

            }
            return RedirectToAction("Dashboard", "BookingViewModels");
        }
        /// <summary>
        /// Get the Bookings and create query with Invoice and payment amount
        /// and set those data in to MyBookingsViewModelBase model list
        /// if startDate and endDate parameter passed, allows user fileter the data by BookingStartDateTime
        /// allows user export in csv, in pdf of the filtered or unfiletered data
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>MyBooking View with MyBookingsViewModelBase model data </returns>
        /// <includesource>yes</includesource>
        public ActionResult MyBooking(string startDate, string endDate)
        {
            using (db)
            {
                var userid = User.Identity.GetUserId();
                var result = (from b in db.Bookings
                                  //from i in b.Invoices  i - b many to many
                                  //join bi in db.Invoices on b.BookingId equals bi.InvoiceId
                              where b.Id == userid && b.Invoice != null
                              from p in b.Invoice.Payments.DefaultIfEmpty()
                              select new
                              {
                                  BookingId = b.BookingId,
                                  ItemDescription = b.ItemDescription,
                                  BookingStatus = b.BookingStatus,
                                  BookingStartDateTime = b.BookingStartDateTime,
                                  BookingDeposit = b.BookingDeposit,
                                  Subtotal = b.Subtotal,
                                  BookingCancellationFee = b.BookingCancellationFee,
                                  Bookings = /*b.Invoice == null ? null :*/ b.Invoice.Bookings,
                                  InvoiceId = b.Invoice == null ? 0 : b.Invoice.InvoiceId,
                                  InvoiceIssueDate = b.Invoice == null ? DateTime.MinValue : b.Invoice.InvoiceIssueDate,
                                  ReceivableDepositAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableDepositAmount,
                                  ReceivableRemainingAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableRemainingAmount,
                                  PayableDepositAmount = b.Invoice == null ? 0f : b.Invoice.PayableDepositAmount,
                                  PayableAmount = b.Invoice == null ? 0f : b.Invoice.PayableAmount,
                                  //PaymentsCredits = b.Invoice.Payments.Sum(ps=>p.CreditAmount),
                                  //PaymentsDebits = b.Invoice.Payments.Sum(ps => p.DebitAmount)
                                  PaymentDateTime = p == null ? null : p.PaymentDateTime,
                                  //CreditAmount = p == null ? 0f : p.CreditAmount,
                                  //DebitAmount = p == null ? 0f : p.DebitAmount,
                                  CreditDepositAmount = p == null ? 0f : p.CreditDepositAmount,
                                  DebitDepositAmount = p == null ? 0f : p.DebitDepositAmount,
                                  CreditRemainingAmount = p == null ? 0f : p.CreditRemainingAmount,
                                  DebitRemainingAmount = p == null ? 0f : p.DebitRemainingAmount,
                                  CreditCancellationFee = p == null ? 0f : p.CreditCancellationFee,
                                  PaymentType = p == null ? null : p.PaymentType
                              }).ToList();

                List<MyBookingsViewModelBase> models = new List<MyBookingsViewModelBase>();
                foreach (var item in result)
                {
                    var thismodel = new MyBookingsViewModelBase();

                    thismodel.BookingId = item.BookingId;
                    thismodel.ItemDescription = item.ItemDescription;
                    thismodel.BookingStatus = item.BookingStatus.ToString();
                    thismodel.BookingStartDateTime = item.BookingStartDateTime;
                    thismodel.BookingDeposit = item.BookingDeposit;
                    thismodel.Subtotal = item.Subtotal;
                    thismodel.InvoiceIssueDate = item.InvoiceIssueDate;
                    thismodel.PaidAmount = item.BookingStatus.ToString() == "Cancelled" && item.BookingCancellationFee > 0 ?
                    item.BookingCancellationFee + item.BookingDeposit :
                    item.BookingStatus.ToString() == "Cancelled" && item.BookingCancellationFee == 0 ?
                    item.BookingDeposit - item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount > 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.CreditDepositAmount - item.DebitDepositAmount :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Remaining" ?
                    item.CreditDepositAmount + item.CreditRemainingAmount :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Remaining" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount > 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount > 0 ?
                    item.CreditDepositAmount + item.CreditRemainingAmount - item.DebitDepositAmount - item.DebitRemainingAmount :
                    item.CreditCancellationFee > 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 ?
                    item.BookingDeposit + item.CreditCancellationFee :
                    0f;
                    thismodel.InvoicedDeposit = item.BookingStatus.ToString() == "Cancelled" ? item.ReceivableDepositAmount - item.PayableDepositAmount : item.BookingDeposit;
                    thismodel.InvoicedRemaining = item.BookingStatus.ToString() == "Cancelled" ?
                    item.ReceivableRemainingAmount - item.PayableAmount :
                    item.ReceivableRemainingAmount > 0 ?
                    item.Subtotal - item.BookingDeposit : 0f;
                    thismodel.ReceivableDepositAmount = item.ReceivableDepositAmount;
                    thismodel.ReceivableRemainingAmount = item.ReceivableRemainingAmount;
                    thismodel.PayableDepositAmount = item.PayableDepositAmount;
                    thismodel.PayableAmount = item.PayableAmount;
                    thismodel.PaymentDateTime = item.PaymentDateTime;
                    thismodel.PaymentType = item.BookingStatus.ToString() == "Cancelled" ? item.BookingStatus.ToString() : item.PaymentType;
                    //CreditAmount = item.CreditAmount,
                    //DebitAmount = item.DebitAmount,
                    thismodel.CreditDepositAmount = item.CreditDepositAmount;
                    thismodel.DebitDepositAmount = item.DebitDepositAmount;
                    thismodel.CreditRemainingAmount = item.CreditRemainingAmount;
                    thismodel.DebitRemainingAmount = item.DebitRemainingAmount;
                    thismodel.CreditCancellationFee = item.CreditCancellationFee;
                    thismodel.BookingCancellationFee = item.BookingCancellationFee;
                    thismodel.InvoiceId = item.InvoiceId;
                    //find out if number of bookings in one invoice matches with number of booking which status is not booked matches
                    //if matches make payment icon get displayed
                    var invBooking = item.Bookings.Where(b => b.InvoiceId == item.InvoiceId).ToList();
                    int bookedCount = 0;
                    foreach (Booking bk in invBooking) { if (bk.BookingStatus != BookingStatus.Booked) { bookedCount++; } }
                    if (bookedCount == invBooking.Count()) { thismodel.Statuscheck = true; }
                    models.Add(thismodel);
                }
                Session["ListMyBookingsViewModelBase"] = models;
                if (startDate != null && endDate != null)
                {
                    if (startDate != "" && endDate != "")
                    {
                        DateTime startDateT = Convert.ToDateTime(startDate);
                        DateTime endDateTemp = Convert.ToDateTime(endDate);
                        DateTime endDateT = endDateTemp.AddDays(1);
                        if (endDateT > startDateT)
                        { models = models.Where(m => m.BookingStartDateTime >= startDateT && m.BookingStartDateTime <= endDateT).ToList(); }
                        else { models = models.ToList(); }
                    }
                }
                return View(models);
            }
        }

        /// <summary>
        /// Get the query of Bookings with Invoice and payment amount by user id if user is logged in, 
        /// if admin/staff is logged in all user's invoice data get returned
        /// and set those data in to InvoiceIndexViewModel model list
        /// if startDate and endDate parameter passed, allows user fileter the model data by BookingStartDateTime
        /// it give choice user to export in csv, in pdf of the filtered or unfiletered model data
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns> InvoiceIndex View with InvoiceIndexModelView model list data</returns>
        /// <includesource>yes</includesource>
        public ActionResult InvoiceIndex(string startDate, string endDate)
        {

            var userid = User.Identity.GetUserId();
            var result = (from b in db.Bookings
                          orderby b.BookingStartDateTime
                          //from i in b.Invoices i-b many to many
                          //where b.Id == userid
                          select new
                          {
                              Id = b.Id,
                              InvoiceId = b.Invoice == null ? 0 : b.Invoice.InvoiceId,
                              BookingId = b.BookingId,
                              BookingStartDateTime = b.BookingStartDateTime,
                              ItemDescription = b.ItemDescription,
                              BookingStatus = b.BookingStatus,
                              Subtotal = b.Subtotal,
                              BookingDeposit = b.BookingDeposit,
                              Bookings = /*b.Invoice == null ? null :*/ b.Invoice.Bookings,
                              InvoiceIssueDate = b.Invoice == null ? DateTime.MinValue : b.Invoice.InvoiceIssueDate,
                              //InvoiceType = i.InvoiceType,
                              ReceivableDepositAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableDepositAmount,
                              ReceivableRemainingAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableRemainingAmount,
                              PayableDepositAmount = b.Invoice == null ? 0f : b.Invoice.PayableDepositAmount,
                              PayableAmount = b.Invoice == null ? 0f : b.Invoice.PayableAmount,
                              InvoiceCancellationFee = b.Invoice == null ? 0f : b.Invoice.InvoiceCancellationFee,
                          });
            if (User.IsInRole("User"))
            {    //asign new filtered value to the result
                result = result.Where(r => r.Id == userid && r.InvoiceId != 0);
                //convert it to the list
                result.ToList();
            }
            else
            {   //asign new filtered value to the result
                result = result.Where(r => r.InvoiceId != 0);
                //convert it to the list
                result.ToList();
            }

            List<InvoiceIndexViewModel> model = new List<InvoiceIndexViewModel>();
            foreach (var item in result)
            {
                var thismodel = new InvoiceIndexViewModel();
                thismodel.Id = item.Id;
                thismodel.Companyname = UserManager.FindById(item.Id).Companyname;
                thismodel.InvoiceId = item.InvoiceId;
                thismodel.BookingId = item.BookingId;
                thismodel.InvoiceIssueDate = item.InvoiceIssueDate;
                thismodel.Subtotal = item.Subtotal;
                thismodel.BookingDeposit = item.BookingDeposit;
                thismodel.ReceivableDepositAmount = item.ReceivableDepositAmount;
                thismodel.ReceivableRemainingAmount = item.ReceivableRemainingAmount;
                thismodel.PayableDepositAmount = item.PayableDepositAmount;
                thismodel.PayableAmount = item.PayableAmount;
                thismodel.InvoiceCancellationFee = item.InvoiceCancellationFee;
                //InvoiceType = item.InvoiceType;
                thismodel.ItemDescription = item.ItemDescription;
                thismodel.BookingStartDateTime = item.BookingStartDateTime;
                thismodel.BookingStatus = item.BookingStatus.ToString();
                thismodel.InvoicedDeposit = item.BookingStatus.ToString() == "Cancelled" ? item.ReceivableDepositAmount - item.PayableDepositAmount : item.BookingDeposit;
                thismodel.InvoicedRemaining = item.BookingStatus.ToString() == "Cancelled" ?
                    item.ReceivableRemainingAmount - item.PayableAmount :
                    item.ReceivableRemainingAmount > 0 ? item.Subtotal - item.BookingDeposit : 0f;
                //InvoicedDeposit = item.PayableDepositAmount <= 0 && item.ReceivableDepositAmount > 0 ?
                //item.ReceivableDepositAmount : 
                //item.PayableDepositAmount > 0 && item.ReceivableDepositAmount > 0 ?
                //item.ReceivableDepositAmount - item.PayableDepositAmount : 
                //0;
                //InvoicedRemaining = item.PayableAmount <= 0 && item.ReceivableRemainingAmount > 0 ? item.ReceivableRemainingAmount : item.PayableAmount > 0 && item.ReceivableRemainingAmount > 0 ? item.ReceivableDepositAmount - item.PayableDepositAmount : 0;                 

                //find out if number of bookings in one invoice matches with number of booking which status is not booked matches
                //if matches view invoice icon get displayed
                var invBooking = item.Bookings.Where(b => b.InvoiceId == item.InvoiceId).ToList();
                int bookedCount = 0;
                foreach (Booking bk in invBooking) { if (bk.BookingStatus != BookingStatus.Booked) { bookedCount++; } }
                if (bookedCount == invBooking.Count()) { thismodel.Statuscheck = true; }
                model.Add(thismodel);
            }

            if (startDate != null && endDate != null)
            {
                if (startDate != "" && endDate != "")
                {
                    DateTime startDateT = Convert.ToDateTime(startDate);
                    DateTime endDateTemp = Convert.ToDateTime(endDate);
                    DateTime endDateT = endDateTemp.AddDays(1);
                    if (endDateT > startDateT)
                    {
                        model = model.Where(m => m.BookingStartDateTime >= startDateT && m.BookingStartDateTime <= endDateT).ToList();
                    }
                    else
                    {
                        model = model.ToList();
                    }
                }
            }

            return View(model);
        }
        /// <summary>
        /// Gets Invoice Detail query using the passed id and 
        /// create view InvoiceDetailViewModel model that get passed the view
        /// Calcualate TotalPaid amount, calculate DueDate, TotalSubtotal,totalPayable 
        /// And passed those data to the view as view bag
        /// Set the InvoiceDetailViewModel as TempData so that when it get process and paid, the data get updated on database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>InvoiceDetail view with InvoiceDetailViewModel model data </returns>
        /// <includesource>yes</includesource>
        public ActionResult InvoiceDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var result = (from i in db.Invoices
                              //from b in i.Bookings
                          where (i.InvoiceId == id)
                          select new
                          {
                              InvoiceId = i.InvoiceId,
                              //InvoiceType = i.InvoiceType,
                              InvoiceIssueDate = i.InvoiceIssueDate,

                              ReceivableDepositAmount = i.ReceivableDepositAmount,
                              ReceivableRemainingAmount = i.ReceivableRemainingAmount,
                              PayableAmount = i.PayableAmount,
                              PayableDepositAmount = i.PayableDepositAmount,
                              InvoiceCancellationFee = i.InvoiceCancellationFee,
                              ApplicationUser = i.ApplicationUser,
                              Bookings = i.Bookings,
                              Payments = i.Payments

                          }).SingleOrDefault();
            //List<InvoiceDetailViewModel> model = new List<InvoiceDetailViewModel>();
            //foreach (var item in result)
            //{
            //    model.Add(new InvoiceDetailViewModel
            if (result == null)
            {
                return HttpNotFound();
            }
            var model = new InvoiceDetailViewModel()
            {
                InvoiceId = result.InvoiceId,
                //InvoiceType = result.InvoiceType,
                InvoiceIssueDate = result.InvoiceIssueDate,

                ReceivableDepositAmount = result.ReceivableDepositAmount,
                ReceivableRemainingAmount = result.ReceivableRemainingAmount,
                PayableAmount = result.PayableAmount,
                PayableDepositAmount = result.PayableDepositAmount,
                InvoiceCancellationFee = result.InvoiceCancellationFee,
                ApplicationUser = result.ApplicationUser,
                Bookings = result.Bookings,
                Payments = result.Payments
            };
            //    });
            //}
            // db.Invoices.Find(id);
            //if(model.Count()==0)
            if (model == null)
            {
                return HttpNotFound();
            }
            TempData["tempInvoiceDetailViewModel"] = model;
            var subt = 0f;
            var invdate = model.InvoiceIssueDate;
            foreach (var b in model.Bookings)
            {
                if (b.BookingStatus != BookingStatus.Cancelled)
                {
                    subt += b.Subtotal;
                    if (b.BookingStartDateTime > invdate)
                    {
                        invdate = b.BookingStartDateTime;
                    }
                }
            }
            var totalPaidAmount = 0f;
            if (model.Payments != null)
            {
                foreach (var p in model.Payments)
                {
                    totalPaidAmount += (p.CreditDepositAmount + p.CreditRemainingAmount - p.DebitDepositAmount - p.DebitRemainingAmount + p.CreditCancellationFee);
                    //totalPaidAmount += (p.CreditAmount + p.CreditAmount);
                }
            }
            ViewBag.TotalPaid = totalPaidAmount;
            ViewBag.DueDate = invdate.AddDays(14);
            ViewBag.TotalSubtotal = subt;
            ViewBag.TotalPayable = model.ReceivableDepositAmount + model.ReceivableRemainingAmount - model.PayableDepositAmount - model.PayableAmount + model.InvoiceCancellationFee;

            return View(model);
        }

        /// <summary>
        /// create a pdf view using tempdata tempInvoiceDetailViewModel
        /// </summary>
        /// <returns> InvoicedDetailToPDF view with passed model</returns>
        /// <includesource>yes</includesource>
        public ActionResult InvoiceDetailToPDF()
        {
            var model = TempData["tempInvoiceDetailViewModel"] as InvoiceDetailViewModel;
            //System.Web.HttpBrowserCapabilities browser = Request.Browser;
            //if (!HttpContext.Request.UserAgent.Contains("Chrome"))
            //{ return Redirect("https://support.google.com/chrome/answer/95346?co=GENIE.Platform%3DDesktop&hl=en-GB");}
            var subt = 0f;
            var invdate = model.InvoiceIssueDate;
            foreach (var b in model.Bookings)
            {
                if (b.BookingStatus != BookingStatus.Cancelled)
                {
                    subt += b.Subtotal;
                    if (b.BookingStartDateTime > invdate)
                    {
                        invdate = b.BookingStartDateTime;
                    }
                }
            }
            var totalPaidAmount = 0f;
            if (model.Payments != null)
            {
                foreach (var p in model.Payments)
                {
                    totalPaidAmount += (p.CreditDepositAmount + p.CreditRemainingAmount - p.DebitDepositAmount - p.DebitRemainingAmount + p.CreditCancellationFee);
                    //totalPaidAmount += (p.CreditAmount + p.CreditAmount);
                }
            }
            ViewBag.TotalPaid = totalPaidAmount;
            ViewBag.DueDate = invdate.AddDays(14);
            ViewBag.TotalSubtotal = subt;
            ViewBag.TotalPayable = model.ReceivableDepositAmount + model.ReceivableRemainingAmount - model.PayableDepositAmount - model.PayableAmount + model.InvoiceCancellationFee;
            TempData["tempInvoiceDetailViewModel"] = model;//saving it in case backbutton get hit and reload is needed
            return new Rotativa.ViewAsPdf("InvoiceDetailToPDF", model/*/*new {id=id }*/) { FileName = "Invoice" + model.InvoiceId + ".pdf" };
        }

        /// <summary>
        /// Get the Bookings from database and create query with invoice and payment amount of the logged in user id,if User is logged in,  
        /// if admin/staff is logged in all user's payment query data get returned
        /// and set above data into PaymentReceiptViewModel model list
        /// if startDate and endDate parameter passed, allows user fileter the data by the BookingStartDateTime
        /// allows user export in csv, in pdf of the filtered or unfiletered data
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>PaymentIndexView with the PaymentIndexView model list data</returns>
        /// <includesource>yes</includesource>
        public ActionResult PaymentIndex(string startDate, string endDate)
        {
            var userid = User.Identity.GetUserId();
            var result = (from b in db.Bookings
                              //from i in b.Invoice
                          from p in b.Invoice.Payments.DefaultIfEmpty()
                              //where b.Id == userid
                          select new
                          {
                              PaymentDateTime = p == null ? null : p.PaymentDateTime,
                              //DebitAmount = p == null ? 0f : p.DebitAmount,
                              //CreditAmount = p == null ? 0f : p.CreditAmount,
                              CreditDepositAmount = p == null ? 0f : p.CreditDepositAmount,
                              DebitDepositAmount = p == null ? 0f : p.DebitDepositAmount,
                              CreditRemainingAmount = p == null ? 0f : p.CreditRemainingAmount,
                              DebitRemainingAmount = p == null ? 0f : p.DebitRemainingAmount,
                              CreditCancellationFee = p == null ? 0f : p.CreditCancellationFee,
                              PaymentType = p == null ? null : p.PaymentType,
                              ItemDescription = b.ItemDescription,
                              BookingDeposit = b.BookingDeposit,
                              BookingStatus = b.BookingStatus.ToString(),
                              BookingStartDateTime = b.BookingStartDateTime,
                              BookingCancellationFee = b.BookingCancellationFee,
                              Subtotal = b.Subtotal,
                              Id = b.Invoice == null ? null : b.Invoice.Id,
                              InvoiceId = b.Invoice == null ? 0 : b.Invoice.InvoiceId,
                              InvoiceIssueDate = b.Invoice == null ? DateTime.MinValue : b.Invoice.InvoiceIssueDate,
                              Bookings = b.Invoice.Bookings,
                              //InvoiceType = i.InvoiceType,
                              ReceivableDepositAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableDepositAmount,
                              ReceivableRemainingAmount = b.Invoice == null ? 0f : b.Invoice.ReceivableRemainingAmount,
                              PayableDepositAmount = b.Invoice == null ? 0f : b.Invoice.PayableDepositAmount,
                              PayableAmount = b.Invoice == null ? 0f : b.Invoice.PayableAmount,
                              PaymentId = p == null ? 0 : p.PaymentId
                          });
            if (User.IsInRole("User"))
            {
                result = result.Where(r => r.Id == userid && r.InvoiceId != 0);
                result.ToList();
            }
            else
            {
                result = result.Where(r => r.InvoiceId != 0);
                result.ToList();
            }
            List<PaymentIndexViewModel> models = new List<PaymentIndexViewModel>();
            foreach (var item in result)
            {
                PaymentIndexViewModel thismodel = new PaymentIndexViewModel();

                //Id = item.Id;
                thismodel.Companyname = UserManager.FindById(item.Id).Companyname;
                thismodel.InvoiceId = item.InvoiceId;
                thismodel.PaymentDateTime = item.PaymentDateTime;
                thismodel.PaidAmount = item.BookingStatus.ToString() == "Cancelled" && item.BookingCancellationFee > 0 ?
                    item.BookingCancellationFee + item.BookingDeposit :
                    item.BookingStatus.ToString() == "Cancelled" && item.BookingCancellationFee == 0 ?
                    item.BookingDeposit - item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount > 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Deposit" ?
                    item.CreditDepositAmount - item.DebitDepositAmount :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Remaining" ?
                    item.CreditDepositAmount + item.CreditRemainingAmount :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 && item.PaymentType == "Remaining" ?
                    item.BookingDeposit :
                    item.CreditCancellationFee <= 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount > 0 && item.CreditRemainingAmount > 0 && item.DebitRemainingAmount > 0 ?
                    item.CreditDepositAmount + item.CreditRemainingAmount - item.DebitDepositAmount - item.DebitRemainingAmount :
                    item.CreditCancellationFee > 0 && item.CreditDepositAmount > 0 && item.DebitDepositAmount <= 0 && item.CreditRemainingAmount <= 0 && item.DebitRemainingAmount <= 0 ?
                    item.BookingDeposit + item.CreditCancellationFee :
                    0f;
                thismodel.ItemDescription = item.ItemDescription;
                thismodel.BookingDeposit = item.BookingDeposit;
                thismodel.Subtotal = item.Subtotal;
                thismodel.InvoiceIssueDate = item.InvoiceIssueDate;
                thismodel.BookingStatus = item.BookingStatus.ToString();
                thismodel.BookingStartDateTime = item.BookingStartDateTime;
                thismodel.InvoicedDeposit = item.BookingStatus.ToString() == "Cancelled" ? item.ReceivableDepositAmount - item.PayableDepositAmount : item.BookingDeposit;
                thismodel.InvoicedRemaining = item.BookingStatus.ToString() == "Cancelled" ?
                item.ReceivableRemainingAmount - item.PayableAmount :
                item.ReceivableRemainingAmount > 0 ?
                item.Subtotal - item.BookingDeposit :
                0f;
                thismodel.ReceivableDepositAmount = item.ReceivableDepositAmount;
                thismodel.ReceivableRemainingAmount = item.ReceivableRemainingAmount;
                thismodel.PayableDepositAmount = item.PayableDepositAmount;
                thismodel.PayableAmount = item.PayableAmount;
                //CreditAmount = item.CreditAmount;
                //DebitAmount = item.DebitAmount;
                thismodel.CreditDepositAmount = item.CreditDepositAmount;
                thismodel.DebitDepositAmount = item.DebitDepositAmount;
                thismodel.CreditRemainingAmount = item.CreditRemainingAmount;
                thismodel.DebitRemainingAmount = item.DebitRemainingAmount;
                thismodel.CreditCancellationFee = item.CreditCancellationFee;
                thismodel.PaymentType = item.PaymentType;
                thismodel.PaymentId = item.PaymentId;

                var invBooking = item.Bookings.Where(b => b.InvoiceId == item.InvoiceId).ToList();
                int bookedCount = 0;
                foreach (Booking bk in invBooking) { if (bk.BookingStatus != BookingStatus.Booked) { bookedCount++; } }
                if (bookedCount == invBooking.Count()) { thismodel.Statuscheck = true; }
                models.Add(thismodel);
            }
            Session["ListPaymentIndexViewModel"] = models;
            if (startDate != null && endDate != null)
            {
                if (startDate != "" && endDate != "")
                {
                    DateTime startDateT = Convert.ToDateTime(startDate);
                    DateTime endDateTemp = Convert.ToDateTime(endDate);
                    DateTime endDateT = endDateTemp.AddDays(1);
                    if (endDateT > startDateT)
                    {
                        models = models.Where(m => m.BookingStartDateTime >= startDateT && m.BookingStartDateTime <= endDateT).ToList();
                    }
                    else
                    {
                        models = models.ToList();
                    }
                }
            }
            return View(models);
        }

        /// <summary>
        /// Get Invoice record found by the passed id and create query with payemnt and Invoice data
        /// Calculate total credit, debt pass them to the view as viewbag
        /// set PaymentReceiptViewModel model with the query data
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PaymentReceipt View with the PaymentReceiptViewmodel model</returns>
        /// <includesource>yes</includesource>
        public ActionResult PaymentReceipt(int? id)
        {
            var userid = User.Identity.GetUserId();
            var result = (from i in db.Invoices
                              //from b in i.Bookings
                              //from p in i.Payments.DefaultIfEmpty()
                          where i.InvoiceId == id
                          select new
                          {
                              //BookingId = b.BookingId,
                              Payments = i.Payments,
                              //PaymentId = p.PaymentId,
                              //PaymentDateTime = p.PaymentDateTime,
                              //CreditAmount = p.CreditAmount,
                              //DebitAmount = p.DebitAmount,
                              Bookings = i.Bookings,
                              InvoiceId = i.InvoiceId,
                              InvoiceIssueDate = i.InvoiceIssueDate,
                              ReceivableDepositAmount = i.ReceivableDepositAmount,
                              ReceivableRemainingAmount = i.ReceivableRemainingAmount,
                              InvoiceCancellationFee = i.InvoiceCancellationFee,
                              ApplicationUser = i.ApplicationUser
                          }).FirstOrDefault();
            PaymentReceiptViewModel model = new PaymentReceiptViewModel()
            {

                //BookingId = result.BookingId,
                ////PaymentId = result.PaymentId,
                ////PaymentDateTime = result.PaymentDateTime,
                ////CreditAmount = result.CreditAmount,
                ////DebitAmount = result.DebitAmount,
                Payments = result.Payments,
                ApplicationUser = result.ApplicationUser,
                Bookings = result.Bookings,
                InvoiceId = result.InvoiceId,
                InvoiceIssueDate = result.InvoiceIssueDate,
                ReceivableDepositAmount = result.ReceivableDepositAmount,
                ReceivableRemainingAmount = result.ReceivableRemainingAmount,
                InvoiceCancellationFee = result.InvoiceCancellationFee
            };
            TempData["APaymentReceipt"] = model;

            var totalcredit = 0f;
            var totaldebt = 0f;
            var totalpaid = 0f;

            foreach (var item in model.Payments) //p 1 to 1 to invoice
            {
                totalcredit += item.CreditRemainingAmount + item.CreditDepositAmount + item.CreditCancellationFee;
                totaldebt += item.DebitRemainingAmount + item.DebitDepositAmount;
                totalpaid += (item.CreditRemainingAmount + item.CreditDepositAmount + item.CreditCancellationFee
                            + item.DebitRemainingAmount + item.DebitDepositAmount);
            }
            ViewBag.TotalCredit = totalcredit;
            ViewBag.TotalDebt = totaldebt;
            ViewBag.TotalPaid = totalpaid;
            return View(model);
        }
        /// <summary>
        /// Calculate total credit, debt and paid from the data passed APaymentReceipt as PaymentReceiptViewModel
        /// and pass this model data to the PaymentReceiptToPDFView 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> PaymentReceiptToPDF view with passed model data</returns>
        /// <includesource>yes</includesource>
        public ActionResult PaymentReceiptToPDF(int? id)
        {

            var model = TempData["APaymentReceipt"] as PaymentReceiptViewModel;
            var totalcredit = 0f;
            var totaldebt = 0f;
            var totalpaid = 0f;
            foreach (var item in model.Payments) //p 1 to 1 to invoice
            {
                totalcredit += item.CreditRemainingAmount + item.CreditDepositAmount + item.CreditCancellationFee;
                totaldebt += item.DebitRemainingAmount + item.DebitDepositAmount;
                totalpaid += (item.CreditRemainingAmount + item.CreditDepositAmount + item.CreditCancellationFee
                            + item.DebitRemainingAmount + item.DebitDepositAmount);
            }
            ViewBag.TotalCredit = totalcredit;
            ViewBag.TotalDebt = totaldebt;
            ViewBag.TotalPaid = totalpaid;
            //System.Web.HttpBrowserCapabilities browser = Request.Browser;
            //if (!HttpContext.Request.UserAgent.Contains("Chrome"))
            //{
            //    return Redirect("https://support.google.com/chrome/answer/95346?co=GENIE.Platform%3DDesktop&hl=en-GB");
            //}
            TempData["APaymentReceipt"] = model;//saving it in case backbutton get hit and reload is needed
            return new Rotativa.ViewAsPdf("PaymentReceiptToPDF", model/*new {id=id }*/) { FileName = "Reciept" + model.InvoiceId + ".pdf" };
        }
        ///<summary>
        /// Get Invoice record found by the passed id and create query with payemnt and Invoice data
        /// set PaymentReceiptViewModel model with the query data, Calculate total deposit pass is as a viewbag to the view along with the model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>PaymentDepositReceipt View with the PaymentReceiptViewmodel model</returns>
        /// <includesource>yes</includesource>
        public ActionResult PaymentDepositReceipt(int? id)
        {
            var userid = User.Identity.GetUserId();
            var result = (from i in db.Invoices
                              //from b in i.Bookings.DefaultIfEmpty()
                              //from p in i.Payments.DefaultIfEmpty()
                          where i.InvoiceId == id
                          select new
                          {
                              //BookingId = b.BookingId,
                              Payments = i.Payments,
                              //PaymentId = p.PaymentId,
                              PaymentDateTime = i.Payments.FirstOrDefault().PaymentDateTime,
                              //CreditAmount = p.CreditAmount,
                              //DebitAmount = p.DebitAmount,
                              Bookings = i.Bookings,
                              InvoiceId = i.InvoiceId,
                              InvoiceIssueDate = i.InvoiceIssueDate,
                              //InvoiceType = i.InvoiceType,
                              InvoiceCancellationFee = i.InvoiceCancellationFee,
                              ApplicationUser = i.ApplicationUser
                          }).FirstOrDefault();
            PaymentReceiptViewModel model = new PaymentReceiptViewModel()
            {

                //BookingId = result.BookingId,
                ////PaymentId = result.PaymentId,
                ////PaymentDateTime = result.PaymentDateTime,
                ////CreditAmount = result.CreditAmount,
                ////DebitAmount = result.DebitAmount,
                PaymentDateTime = result.PaymentDateTime,
                ApplicationUser = result.ApplicationUser,
                Bookings = result.Bookings,
                InvoiceId = result.InvoiceId,
                InvoiceIssueDate = result.InvoiceIssueDate,
                Payments = result.Payments,
                //InvoiceType = result.InvoiceType,
                InvoiceCancellationFee = result.InvoiceCancellationFee
            };
            TempData["APaymentReceiptViewModel"] = model;
            var totaldeposit = 0f;
            foreach (var item in model.Payments)
            {
                //totaldeposit += item.CreditAmount - item.DebitAmount;
                totaldeposit += item.CreditDepositAmount - item.DebitDepositAmount;
            }
            ViewBag.TotalDeposit = totaldeposit;
            return View(model);
        }
        /// <summary>
        /// Calculate total deposit from the data passed APaymentReceiptViewModel as PaymentReceiptViewModel
        /// and pass this model data to the PaymentDepositReceiptToPDFView 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> PaymentDepositReceiptToPDF view with passed model data</returns>
        /// <includesource>yes</includesource>
        public ActionResult PaymentDepositReceiptToPDF()
        {
            var pdfmodel = TempData["APaymentReceiptViewModel"] as PaymentReceiptViewModel;
            var totaldeposit = 0f;
            foreach (var item in pdfmodel.Payments)
            {
                //totaldeposit += item.CreditAmount - item.DebitAmount;
                totaldeposit += item.CreditDepositAmount - item.DebitDepositAmount;
            }
            ViewBag.TotalDeposit = totaldeposit;
            //System.Web.HttpBrowserCapabilities browser = Request.Browser;
            //if (!HttpContext.Request.UserAgent.Contains("Chrome"))
            //{ return Redirect("https://support.google.com/chrome/answer/95346?co=GENIE.Platform%3DDesktop&hl=en-GB");}
            return new Rotativa.ViewAsPdf("PaymentDepositReceiptToPDF", pdfmodel) { FileName = "DepositReceipt" + pdfmodel.InvoiceId + ".pdf" }; ;
        }


        /// <summary>
        /// Get BookingOriginal, BookingUpdated, temp data assign in to a list
        /// InvoiceOriginal, InvoiceUpdated tems data assign them in to a list
        /// put them into VisitRefundViewModel model and calculate the 
        /// subtotalAmountToBeDeducted,originalremainingAmount,refundamount 
        /// and pass them in a viewbag to display on the view
        /// saves  BookingOriginal, BookingUpdated, InvoiceOriginal, InvoiceUpdated Session 
        /// again to pass to the Refund action.
        /// </summary>
        /// <returns>VisitRerund confirmation View</returns>
        /// <includesource>yes</includesource>
        public ActionResult VisitRefundConfirmation()
        {
            var bookingOriginal = TempData["BookingOriginal"] as Booking;
            var bookingUpdated = TempData["BookingUpdated"] as Booking;
            var invoiceOriginal = TempData["InvoiceOriginal"] as Invoice;
            var invoiceUpdated = TempData["InvoiceUpdated"] as Invoice;

            var listBothBookings = new List<Booking>();
            listBothBookings.Add(bookingOriginal);
            listBothBookings.Add(bookingUpdated);

            var listBothInvoices = new List<Invoice>();
            listBothInvoices.Add(invoiceOriginal);
            listBothInvoices.Add(invoiceUpdated);
            //check if unit price has changed in model, if so set current unit price as model to calculate else use original one.
            var orginalBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == bookingOriginal.UnitPriceId).FirstOrDefault().UnitPriceValue;
            var thisBkgUnitPrice = (float)db.UnitPrices.Where(i => i.UnitPriceId == bookingUpdated.UnitPriceId).FirstOrDefault().UnitPriceValue;
            var currentUnitPrice = 0f;
            if (orginalBkgUnitPrice == thisBkgUnitPrice)
            { currentUnitPrice = orginalBkgUnitPrice; }
            else { currentUnitPrice = thisBkgUnitPrice; }
            ApplicationUser user = db.Users.Where(u => u.Id == bookingOriginal.Id).FirstOrDefault();
            List<PaymentHistory> listpayhis = db.PaymentHistories.Where(ph => ph.InvoiceId == bookingOriginal.InvoiceId).ToList();
            var subtotalAmountToBeDeducted = (float)bookingUpdated.BookingAdjustmentHrs * currentUnitPrice;
            var originalremainingAmount = (float)bookingOriginal.Subtotal - (float)bookingOriginal.BookingDeposit;
            var refundamount = 0f;
            if (bookingUpdated.BookingStatus == BookingStatus.Curtailed)
            {
                if (subtotalAmountToBeDeducted > originalremainingAmount)
                {
                    refundamount = (subtotalAmountToBeDeducted - originalremainingAmount);
                    //bookingUpdated.BookingCancellationFee = bookingOriginal.BookingCancellationFee;
                }
            }
            if (bookingUpdated.BookingStatus == BookingStatus.Cancelled)
            {
                if (bookingOriginal.BookingDeposit > 0) { refundamount = bookingOriginal.BookingDeposit; }
            }

            //float refundamount = (listBothBookings[1].BookingAdjustmentHrs * currentUnitPrice)-(bookingOriginal.Subtotal-bookingOriginal.BookingDeposit);
            VisitRefundViewModel model = new VisitRefundViewModel();

            model.ApplicationUser = user;
            model.Bookings = listBothBookings;
            model.Invoices = listBothInvoices;
            model.RefundAmount = refundamount;
            model.PaymentHistories = listpayhis;

            TempData["BookingUpdated"] = bookingUpdated;
            TempData["InvoiceUpdated"] = invoiceUpdated;
            return View(model);

        }
        /// <summary>
        /// Get accessto Braintree gateway, 
        /// get amount posted from the either confirm cancellation view or New view with get refund button
        /// if MyBookingCancellationRequest is not null, get payment transaction from it
        /// if InvoiceUpdated from AmendBooking or ConfirmVisit View is not null, get payment transaction from it,
        /// apply refund transaction with the transactionId found,
        /// if successful, if MyBookingCancellationRequest session is not null, save the change to the bookings, payments,invoices, paymentHistories database
        /// and save the updatedInvoice session as "updatedInvoiceCancelledBkg" to pass this to email fucntion
        /// if InvoiceUpdated from AmendBooking or ConfirmVisit view is not null, save change to payments, payemnt histories, invoices and bookings database
        /// and save TepData InvoiceUpdatedDebited to pass this to the email function.
        /// if successful in either way, retruns show view
        /// </summary>
        /// <returns>BookingViewModels/Show View</returns>
        /// <includesource>yes</includesource>
        public ActionResult Refund()
        {
            var gateway = config.GetGateway();
            Decimal amount;

            try
            {
                amount = Convert.ToDecimal(Request["amount"]);
            }
            catch (FormatException e)
            {
                TempData["Flash"] = "Error: 81503: Amount is an invalid format.";
                return RedirectToAction("MyBooking");
            }

            var nonce = Request["payment_method_nonce"];
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = nonce,
                //Options = new TransactionOptionsRequest
                //{
                //    SubmitForSettlement = true
                //}
            };
            string payTransactionId = "";
            var myBookingCancellationRequest = Session["MyBookingCancellationRequest"] as MyBookingCancellation;
            if (myBookingCancellationRequest != null)
            {
                var paymentid = myBookingCancellationRequest.Payments.FirstOrDefault().PaymentId;
                var payhistories = db.PaymentHistories.Where(ph => ph.PaymentId == paymentid && ph.CreditAmount >= myBookingCancellationRequest.BookingDeposit).ToList();
                //extract payment transactionId from the model
                foreach (var item in payhistories)
                {
                    payTransactionId = item.TransactionId;
                }
            }
            var bookingUpdated = TempData["BookingUpdated"] as Booking;
            var invoiceUpdated = TempData["InvoiceUpdated"] as Invoice;
            //extract payment that matches with the invoiceid from the invoiceUpdated
            if (invoiceUpdated != null)
            {
                Payment thisPayment = db.Payments.Where(p => p.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                var thispaymentid = thisPayment.PaymentId;
                //Payment thisPayment = db.Payments.Where(p => p.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                //PaymentHistory payhistransid = db.PaymentHistories.Where(i => i.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                if (thisPayment != null)
                {
                    //extract payment transactionId from the invoice
                    payTransactionId = thisPayment.PaymentHistories.Where(ph => ph.PaymentId == thispaymentid && ph.CreditAmount >= bookingUpdated.BookingDeposit).FirstOrDefault().TransactionId;
                }
            }



            Result<Transaction> result = gateway.Transaction.Refund(payTransactionId, (Decimal)amount);
            if (result.IsSuccess())
            {
                Transaction refund = result.Target;
                //refund.Type;
                //TransactionType.CREDIT;
                //refund.Amount;

                if (invoiceUpdated != null /*|| amendbkgInvoiceUpdated != null*/)
                {
                    Payment thisPayment = db.Payments.Where(p => p.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                    var thispaymentid = thisPayment.PaymentId;
                    // set selected payemnt's payment type to Deposit cancelled
                    //and put amount in the payment debit amount
                    if (thisPayment == null  /*&& amendbkgPayment == null*/)
                    {
                        return HttpNotFound();
                    }

                    //selectedPayment.DebitAmount = selectedPayment.DebitAmount + (float)amount;
                    thisPayment.DebitDepositAmount = thisPayment.DebitDepositAmount + (float)amount;
                    if (invoiceUpdated != null && bookingUpdated.BookingStatus == BookingStatus.Cancelled) { thisPayment.PaymentType = "DepositCancelled"; }
                    if (ModelState.IsValid)
                    {
                        db.Entry(thisPayment).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    string userid = User.Identity.GetUserId();
                    var newPaymentHistory = new PaymentHistory()
                    {
                        PaymentDateTime = DateTime.Now,
                        CreditAmount = 0f,
                        DebitAmount = (float)amount,
                        Id = userid,
                        PaymentId = thispaymentid,
                        InvoiceId = invoiceUpdated.InvoiceId,
                        PaymentType = PaymentType.Deposit,
                        TransactionId = refund.Id
                    };
                    db.PaymentHistories.Add(newPaymentHistory); ;
                    db.SaveChanges();


                    //save updated invoice on to database
                    Invoice thisInvoice = db.Invoices.Where(i => i.InvoiceId == invoiceUpdated.InvoiceId).FirstOrDefault();
                    thisInvoice.ReceivableDepositAmount = invoiceUpdated.ReceivableDepositAmount;
                    thisInvoice.ReceivableRemainingAmount = invoiceUpdated.ReceivableRemainingAmount;
                    thisInvoice.PayableDepositAmount = invoiceUpdated.PayableDepositAmount;
                    thisInvoice.PayableAmount = invoiceUpdated.PayableAmount;

                    if (ModelState.IsValid)
                    {
                        db.Entry(thisInvoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    if (bookingUpdated == null)
                    {
                        return HttpNotFound();
                    }
                    Booking thisbooking = db.Bookings.Where(b => b.BookingId == bookingUpdated.BookingId).FirstOrDefault();
                    thisbooking.BookingStartDateTime = bookingUpdated.BookingStartDateTime;
                    thisbooking.BookingEndDateTime = bookingUpdated.BookingEndDateTime;
                    thisbooking.ItemDescription = bookingUpdated.ItemDescription;
                    thisbooking.Subtotal = bookingUpdated.Subtotal;
                    thisbooking.BookingDeposit = bookingUpdated.BookingDeposit;
                    thisbooking.BookingAdjustmentHrs = bookingUpdated.BookingAdjustmentHrs;
                    thisbooking.BookingCancellationFee = bookingUpdated.BookingCancellationFee;
                    thisbooking.BookingStatus = bookingUpdated.BookingStatus;
                    thisbooking.UnitPriceId = bookingUpdated.UnitPriceId;
                    if (ModelState.IsValid)
                    {
                        db.Entry(thisbooking).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    TempData["InvoiceUpdatedDebited"] = invoiceUpdated;
                }
                if (myBookingCancellationRequest != null)
                {
                    //extract selected booking and chagne state to cancelled
                    Booking selectedBooking = db.Bookings.Find(myBookingCancellationRequest.BookingId);
                    if (selectedBooking == null)
                    {
                        return HttpNotFound();
                    }
                    selectedBooking.BookingStatus = (BookingStatus)Enum.Parse(typeof(BookingStatus), myBookingCancellationRequest.BookingStatus);
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedBooking).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //extract payment id from the model
                    var payid = 0;
                    foreach (var item in myBookingCancellationRequest.Payments)
                    {
                        if (item.PaymentType == "Deposit")
                        {
                            payid = item.PaymentId;
                        }
                    }
                    // get selected payemnt to chagne the payment type to Deposit cancelled
                    //and put amount in the payemnt debit amount
                    var selectedPayment = db.Payments.Find(payid);
                    if (selectedPayment == null)
                    {
                        return HttpNotFound();
                    }
                    //selectedPayment.DebitAmount = selectedPayment.DebitAmount + (float)amount;
                    selectedPayment.DebitDepositAmount = selectedPayment.DebitDepositAmount + (float)amount;
                    selectedPayment.PaymentType = "DepositCancelled";
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedPayment).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //find invoice id 
                    var invid = 0;
                    //foreach (var item in myBookingCancellationRequest.Invoices) b-i many to many
                    //{
                    //    invid = item.InvoiceId;
                    //}
                    invid = myBookingCancellationRequest.Invoice.InvoiceId;
                    //get selected invoice to save amount in payable deposit amount
                    Invoice selectedInvoice = db.Invoices.Find(invid);
                    if (selectedInvoice == null)
                    {
                        return HttpNotFound();
                    }
                    selectedInvoice.PayableDepositAmount = selectedInvoice.PayableDepositAmount + (float)amount;
                    if (ModelState.IsValid)
                    {
                        db.Entry(selectedInvoice).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //var userid = User.Identity.GetUserId();
                    //var newPayment = new Payment()
                    //{
                    //    PaymentDateTime = DateTime.Now,
                    //    CreditAmount = (float)amount,
                    //    DebitAmount = 0f,
                    //    Id = userid,
                    //    InvoiceId = selectedInvoice.InvoiceId,
                    //    PaymentType = "Deposit",
                    //    TransactionId = refund.Id
                    //};
                    //db.Payments.Add(newPayment); 
                    //db.SaveChanges();
                    string userid = User.Identity.GetUserId();
                    var newPaymentHistory = new PaymentHistory()
                    {
                        PaymentDateTime = DateTime.Now,
                        CreditAmount = 0f,
                        DebitAmount = (float)amount,
                        Id = userid,
                        PaymentId = selectedPayment.PaymentId,
                        InvoiceId = selectedInvoice.InvoiceId,
                        PaymentType = PaymentType.Deposit,
                        TransactionId = refund.Id
                    };
                    db.PaymentHistories.Add(newPaymentHistory); ;
                    db.SaveChanges();
                    Session["updatedInvoiceCancelledbkg"] = Session["updatedInvoice"];
                }
                return RedirectToAction("Show", new { id = refund.Id });
            }
            else if (result.Transaction != null)
            {
                return RedirectToAction("Show", new { id = result.Transaction.Id });
            }
            else
            {
                string errorMessages = "";
                foreach (ValidationError error in result.Errors.DeepAll())
                {
                    errorMessages += "Error: " + (int)error.Code + " - " + error.Message + "\n";
                }
                TempData["Flash"] = errorMessages;
                return RedirectToAction("New");
            }

        }



        /// <summary>
        /// Get Invoice record found from the passed id, 
        /// create payment history and assign passed id as paymentHistory Invoice Id
        /// assign the found invoice's user id to this payment hiostory userid
        /// assin the found invoice's payment id to this payment history paymentId
        /// calculate total remaining amount from data from the invoice data
        /// assign the total remaining into paymentHisotry's credit amount.
        /// save the paymenthistory as "NullPaymentHisotryWithInvPayIds" sesion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Return AddPayment view with the created paymentHistorydata</returns>
        /// <includesource>yes</includesource>
        public ActionResult AddPaymentHistory(int? id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Where(i => i.InvoiceId == id).FirstOrDefault();
            var paymentHistory = new PaymentHistory();
            {
                paymentHistory.InvoiceId = id;
            }
            paymentHistory.Id = invoice.Id;

            paymentHistory.PaymentId = invoice.Payments.FirstOrDefault().PaymentId;/*db.Invoices.Where(i => i.InvoiceId == id).FirstOrDefault().Payments.FirstOrDefault().PaymentId;*/
            var totalpaid = 0f;
            invoice.Payments.ToList().ForEach(p => totalpaid += (p.CreditDepositAmount - p.DebitDepositAmount + p.CreditRemainingAmount - p.DebitRemainingAmount));
            var totalremaining = (invoice.ReceivableDepositAmount - invoice.PayableDepositAmount + invoice.ReceivableRemainingAmount - invoice.PayableAmount) - totalpaid;
            paymentHistory.CreditAmount = totalremaining;
            Session["NullPaymentHistoryWithInvPayIds"] = paymentHistory;
            //ViewBag.Id = new SelectList(db.Users, "Id", "Companyname");
            //ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId");
            //if (Request.IsAjaxRequest())
            //{
            //    return PartialView("_AddPayHistory", paymentHistory);
            //}
            //else
            //{
            return View(paymentHistory);
            //}

        }


        /// <summary>
        /// returns validation error if input paymentDateTime is later than now,
        /// returns validation error if both credit and debit amount is filled by mistake
        /// returns validation error if Invoiced amount is more than remaining payment amount
        /// Assign NullPaymentHistoryWithInvPayIds session's 
        /// InvoiceId, bookingId and paymentId to the binded booking object as they are null at the moment 
        /// save the binged paymentHistory data in payment Histories, 
        /// and the update the payment data
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's CreditAmount > 0 , 
        ///   payments' CreditDepositAmount gets paymentHistory.CreditAmount and Payments' PaymentType get assigned of "Deposit" value
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's DebitAmount > 0
        ///   payments' DebitDepositAmount gets  paymentHistory.DebitAmount value and Payments' PaymentType get assigned of "Cancelled" value
        /// if paymentHistory's PaymentType is PaymentType.Remaining and paymentHistory's CreditAmount > 0 , 
        ///   payments' CreditRemainingAmount gets paymentHistory.CreditAmount and Payments' PaymentType get assigned of "Remaining" value
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's DebitAmount > 0
        ///   payments' DebitReaminingAmount gets  paymentHistory.DebitAmount value and Payments' PaymentType get assigned of "Cancelled" value
        /// and update payment database.
        /// </summary>
        /// <param name="paymentHistory"></param>
        /// <returns>Returns back to PaymentIndex view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPaymentHistory([Bind(Include = "PaymentHistoryId,PaymentDateTime,PaymentType,CreditAmount,DebitAmount,TransactionId,PaymentId,InvoiceId,Id")] PaymentHistory paymentHistory)
        {   //assign tempdata to local variable so that values can be assinged to the paymentHistory paramenter values
            var temppaymentHistory = Session["NullPaymentHistoryWithInvPayIds"] as PaymentHistory;
            if (paymentHistory.PaymentDateTime == null)
            {
                ViewBag.Error = "You must select payment date";
                return View();
            }
            if (paymentHistory.PaymentDateTime >= DateTime.Now)
            {
                ViewBag.Error = "Payment Date can't be of future date";
                return View();
            }
            if (paymentHistory.CreditAmount == null || paymentHistory.DebitAmount == null)
            {
                ViewBag.Error = "Either Credit amount and or  Debit amount can't be null.";
                return View();
            }
            if (paymentHistory.CreditAmount > 0 && paymentHistory.DebitAmount > 0)
            {
                ViewBag.Error = "Both Credit amount and Debit amount can't be filled at one record.";
                return View();
            }


            //get invoiced amount to check if input went over invoiced amount.
            var tempinv = db.Invoices.FirstOrDefault(i => i.InvoiceId == temppaymentHistory.InvoiceId);
            if (paymentHistory.CreditAmount > (tempinv.ReceivableRemainingAmount - tempinv.PayableAmount + tempinv.InvoiceCancellationFee))
            {
                ViewBag.Error = "Payment amount can't be more than invoiced amount";
                return View();
            }
            //assign invoice id and pay id to the paymet history
            paymentHistory.InvoiceId = temppaymentHistory.InvoiceId;
            paymentHistory.PaymentId = temppaymentHistory.PaymentId;
            paymentHistory.Id = temppaymentHistory.Id;

            //to save the data to the payment history, create new payment history
            if (ModelState.IsValid)
            {
                db.PaymentHistories.Add(paymentHistory);
                db.SaveChanges();
            }

            //next update the change in selected payment histories
            //find the payment record first and allocate the updated value.
            var selectedpay = db.Payments.Where(p => p.PaymentId == paymentHistory.PaymentId).FirstOrDefault();

            if (selectedpay != null)
            {   //foreach (var item in selectedpay)
                //{
                if (paymentHistory.PaymentType == PaymentType.Deposit)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {
                            selectedpay.CreditDepositAmount = paymentHistory.CreditAmount;
                            if (selectedpay.PaymentType == null) selectedpay.PaymentType = "Deposit";
                        }
                    }
                    if (paymentHistory.CreditAmount == 0)
                    {
                        if (paymentHistory.DebitAmount > 0)
                        {
                            selectedpay.DebitDepositAmount = paymentHistory.DebitAmount;
                            selectedpay.PaymentType = "Cancelled";
                        }
                    }
                }
                if (paymentHistory.PaymentType == PaymentType.Remaining)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {

                            selectedpay.CreditRemainingAmount = +paymentHistory.CreditAmount;
                            /*if (item.PaymentType == null)*/
                            selectedpay.PaymentType = "Remaining";
                        }
                    }

                    if (paymentHistory.CreditAmount == 0)
                    {
                        if (paymentHistory.DebitAmount > 0)
                        {
                            selectedpay.DebitRemainingAmount = +paymentHistory.DebitAmount;
                            selectedpay.PaymentType = "Cancelled";
                        }
                    }
                }
                if (paymentHistory.PaymentType == PaymentType.CancellationFee)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {
                            selectedpay.CreditCancellationFee = paymentHistory.CreditAmount;
                            /*if (item.PaymentType == null) */
                            selectedpay.PaymentType = "CancellationFee";
                        }
                    }
                }
                if (ModelState.IsValid)
                {
                    db.Entry(selectedpay).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("PaymentIndex");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", paymentHistory.Id);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "Id", paymentHistory.InvoiceId);
            //ViewBag.PaymentId = new SelectList(db.Payments, "PaymentId", "PaymentType", paymentHistory.PaymentId);
            if (Request.IsAjaxRequest())
                return PartialView("_AddPayHistory");
            else
                return View();
        }


        // GET: PaymentHistories/EditPaymentHistory
        /// <summary>
        /// Gets the paymentHistory found with the passed id 
        /// saves it as Session "PaymentHistoryWithIds" and pass it to the view
        /// </summary>
        /// <param name="id"></param>
        /// <returns>EditPaymentHistory view with paymenthistory data</returns>
        /// <includesource>yes</includesource>
        public ActionResult EditPaymentHistory(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                PaymentHistory paymentHistory = db.PaymentHistories.Find(id);
                if (paymentHistory == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Id = new SelectList(db.Users, "Id", "Companyname");
                ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId");
                Session["PaymentHistoryWithIds"] = paymentHistory;
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_EditPayHistory", paymentHistory);
                }
                else
                {
                    return View(paymentHistory);
                }
            }
        }

        /// <summary>
        /// Gets the Session data PayemntHistoryWithIds
        /// returns validation error if input paymentDateTime is later than now,
        /// returns validation error if both credit and debit amount is filled by mistake
        /// returns validation error if Invoiced amount is more than remaining payment amount
        /// Create payment history from the temppayentHistory's paymenthistory id
        /// and assing the posted PayemntHistoryViewModel model data to the created paymemnHistory
        /// save to paymentHistoryr database,
        /// save the binged paymentHistory data in payment Histories, 
        /// and the update the payment data
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's CreditAmount > 0 , 
        ///   payments' CreditDepositAmount gets paymentHistory.CreditAmount and Payments' PaymentType get assigned of "Deposit" value
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's DebitAmount > 0
        ///   payments' DebitDepositAmount gets  paymentHistory.DebitAmount value and Payments' PaymentType get assigned of "Cancelled" value
        /// if paymentHistory's PaymentType is PaymentType.Remaining and paymentHistory's CreditAmount > 0 , 
        ///   payments' CreditRemainingAmount gets paymentHistory.CreditAmount and Payments' PaymentType get assigned of "Remaining" value
        /// if paymentHistory's PaymentType is PaymentType.Deposit and paymentHistory's DebitAmount > 0
        ///   payments' DebitReaminingAmount gets  paymentHistory.DebitAmount value and Payments' PaymentType get assigned of "Cancelled" value
        /// and update payment database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns back to PaymentIndex view</returns>
        /// <includesource>yes</includesource>
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult EditPaymentHistory([Bind(Include = "PaymentHistoryId,PaymentDateTime,PaymentType,CreditAmount,DebitAmount,TransactionId,PaymentId,InvoiceId,Id")] PaymentHistory paymentHistory)
        public ActionResult EditPaymentHistory(PaymentHistoryViewModel model)
        { //assign tempdata to local variable so that values can be assinged to the paymentHistory paramenter values
            var temppaymentHistory = Session["PaymentHistoryWithIds"] as PaymentHistory;
            var paydateDT = Convert.ToDateTime(model.PaymentDateTime);
            if (paydateDT >= DateTime.Now)
            {
                ViewBag.Error = "Payment Date can't be of future date";
                return View();
            }
            ////get original payment history record
            //PaymentHistory originalph = db.PaymentHistories.FirstOrDefault(ph => ph.PaymentHistoryId == temppaymentHistory.PaymentHistoryId);
            //get invoiced amount to check if input went over invoiced amount.
            var tempinv = db.Invoices.FirstOrDefault(i => i.InvoiceId == temppaymentHistory.InvoiceId);
            if (model.CreditAmount > (tempinv.ReceivableRemainingAmount - tempinv.PayableAmount + tempinv.InvoiceCancellationFee))
            {
                ViewBag.Error = "Payment amount can't be more than invoiced amount";
                return View();
            }
            if (model.CreditAmount > 0 && model.DebitAmount > 0)
            {
                ViewBag.Error = "Both Credit amount and Debit amount can't be filled at one record.";
                return View();
            }
            PaymentHistory paymentHistory = db.PaymentHistories.Where(ph => ph.PaymentHistoryId == temppaymentHistory.PaymentHistoryId).FirstOrDefault();
            paymentHistory.PaymentDateTime = paydateDT;
            paymentHistory.PaymentType = model.PaymentType;
            paymentHistory.CreditAmount = model.CreditAmount;
            paymentHistory.DebitAmount = model.DebitAmount;

            if (ModelState.IsValid)
            {
                db.Entry(paymentHistory).State = EntityState.Modified;
                db.SaveChanges();
            }

            //update the change in the payment history into payment table
            //find the related payment record first and allocate the updated value.
            var selectedpay = db.Payments.Where(p => p.PaymentId == paymentHistory.PaymentId).FirstOrDefault();

            if (selectedpay != null)
            {
                //using (db)
                //{
                //foreach (var item in selectedpay.ToList())
                //{
                if (paymentHistory.PaymentType == PaymentType.Deposit)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {
                            selectedpay.CreditDepositAmount = paymentHistory.CreditAmount;
                            if (selectedpay.PaymentType == null) selectedpay.PaymentType = "Deposit";
                        }
                    }
                    if (paymentHistory.CreditAmount == 0)
                    {
                        if (paymentHistory.DebitAmount > 0)
                        {
                            selectedpay.DebitDepositAmount = paymentHistory.DebitAmount;
                            selectedpay.PaymentType = "Cancelled";
                        }
                    }
                }
                if (paymentHistory.PaymentType == PaymentType.Remaining)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {

                            selectedpay.CreditRemainingAmount = +paymentHistory.CreditAmount;
                            selectedpay.PaymentType = "Remaining";
                        }
                    }

                    if (paymentHistory.CreditAmount == 0)
                    {
                        if (paymentHistory.DebitAmount > 0)
                        {
                            selectedpay.DebitRemainingAmount = +paymentHistory.DebitAmount;
                            selectedpay.PaymentType = "Cancelled";
                        }
                    }
                }
                if (paymentHistory.PaymentType == PaymentType.CancellationFee)
                {
                    if (paymentHistory.CreditAmount > 0)
                    {
                        if (paymentHistory.DebitAmount == 0)
                        {
                            selectedpay.CreditCancellationFee = paymentHistory.CreditAmount;
                            selectedpay.PaymentType = "CancellationFee";
                        }
                    }
                } //}

                if (ModelState.IsValid)
                {
                    db.Entry(selectedpay).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexPaymentHistory");
                }
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", paymentHistory.Id);
            ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "Id", paymentHistory.InvoiceId);
            //ViewBag.PaymentId = new SelectList(db.Payments, "PaymentId", "PaymentType", paymentHistory.PaymentId);
            if (Request.IsAjaxRequest())
                return PartialView("_EditPayHistory");
            else
                return View(paymentHistory);
        }
        //// GET: PaymentHistories/Create
        //public ActionResult NewPaymentHistory()
        //{
        //    ViewBag.Id = new SelectList(db.Users, "Id", "Companyname");
        //    ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "InvoiceId");

        //    return View();
        //    //return this.PartialView("_NewPayHistory");
        //}
        //// POST: PaymentHistories/newPaymentHistory
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult NewPaymentHistory([Bind(Include = "PaymentHistoryId,PaymentDateTime,PaymentType,CreditAmount,DebitAmount,TransactionId,PaymentId,InvoiceId,Id")] PaymentHistory paymentHistory)
        //{

        //    if (paymentHistory.InvoiceId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //find user id  from invoice id and allocate it to payment history and 
        //    //find payment through the invoice Id and allocate the appropriate payment attribute to the payment
        //    //var selectedInv = db.Invoices.Where(i => i.InvoiceId == paymentHistory.InvoiceId).FirstOrDefault().Id.ToString();
        //    //find the payment Id from the invoce
        //    int paymentid = (int)db.Invoices.Where(i => i.InvoiceId == paymentHistory.InvoiceId).FirstOrDefault().Payments.FirstOrDefault().PaymentId;
        //    var userid = db.Invoices.Where(i => i.InvoiceId == paymentHistory.InvoiceId).FirstOrDefault().Id.ToString();

        //    paymentHistory.Id = userid;
        //    paymentHistory.PaymentId = paymentid;

        //    // save the updates if model state is valid

        //    if (ModelState.IsValid)
        //    {
        //        db.PaymentHistories.Add(paymentHistory);
        //        db.SaveChanges();
        //    }
        //    //update the change in the payment history into payment table
        //    //find the related payment record first and allocate the updated value.
        //    var selectedpay = db.Payments.Where(p => p.PaymentId == paymentid).FirstOrDefault();
        //    if (selectedpay != null)
        //    {
        //        //using (db)
        //        //{
        //        //foreach (var item in selectedpay.ToList())
        //        //{
        //        if (paymentHistory.PaymentType == PaymentType.Deposit)
        //        {
        //            if (paymentHistory.CreditAmount > 0)
        //            {
        //                if (paymentHistory.DebitAmount == 0)
        //                {
        //                    selectedpay.CreditDepositAmount = paymentHistory.CreditAmount;
        //                    if (selectedpay.PaymentType == null) selectedpay.PaymentType = "Deposit";
        //                }
        //            }
        //            if (paymentHistory.CreditAmount == 0)
        //            {
        //                if (paymentHistory.DebitAmount > 0)
        //                {
        //                    selectedpay.DebitDepositAmount = paymentHistory.DebitAmount;
        //                    selectedpay.PaymentType = "Cancelled";
        //                }
        //            }
        //        }
        //        if (paymentHistory.PaymentType == PaymentType.Remaining)
        //        {
        //            if (paymentHistory.CreditAmount > 0)
        //            {
        //                if (paymentHistory.DebitAmount == 0)
        //                {

        //                    selectedpay.CreditRemainingAmount = +paymentHistory.CreditAmount;
        //                    selectedpay.PaymentType = "Remaining";
        //                }
        //            }

        //            if (paymentHistory.CreditAmount == 0)
        //            {
        //                if (paymentHistory.DebitAmount > 0)
        //                {
        //                    selectedpay.DebitRemainingAmount = paymentHistory.DebitAmount;
        //                    selectedpay.PaymentType = "Cancelled";
        //                }
        //            }
        //        }
        //        if (paymentHistory.PaymentType == PaymentType.CancellationFee)
        //        {
        //            if (paymentHistory.CreditAmount > 0)
        //            {
        //                if (paymentHistory.DebitAmount == 0)
        //                {
        //                    selectedpay.CreditCancellationFee = paymentHistory.CreditAmount;
        //                    selectedpay.PaymentType = "CancellationFee";
        //                }
        //            }
        //        }
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(selectedpay).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("PaymentIndex");
        //        }
        //    }
        //    ViewBag.Id = new SelectList(db.Users, "Id", "Companyname", paymentHistory.Id);
        //    ViewBag.InvoiceId = new SelectList(db.Invoices, "InvoiceId", "Id", paymentHistory.InvoiceId);
        //    //ViewBag.PaymentId = new SelectList(db.Payments, "PaymentId", "PaymentType", paymentHistory.PaymentId);
        //    return View(paymentHistory);
        //    //return View("_newPaymentHistory", paymentHistory);
        //}
        /// <summary>
        /// Gets all the paymentHistory from database
        /// if startDate and endDate is selected filters the data if payment date is btw those date
        /// pass the list of payment to the view
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns PaymentHistoryView with list of payment history data</returns>
        /// <includesource>yes</includesource>
        public ActionResult IndexPaymentHistory(string startDate, string endDate)
        {
            var paymentHistories = db.PaymentHistories.Include(p => p.ApplicationUser).Include(p => p.Invoice).Include(p => p.Payment);
            if (startDate != null && endDate != null)
            {
                if (startDate != "" && endDate != "")
                {
                    DateTime startDateT = Convert.ToDateTime(startDate);
                    DateTime endDateTemp = Convert.ToDateTime(endDate);
                    DateTime endDateT = endDateTemp.AddDays(1);
                    if (endDateT > startDateT)
                    {
                        paymentHistories = paymentHistories.Where(p => p.PaymentDateTime >= startDateT && p.PaymentDateTime <= endDateT);
                    }
                }
            }
            return View(paymentHistories.ToList());
        }
        /// <summary>
        /// Get Payemnt History found with the passed id from database
        /// and Returns the record of PaymentHistory view that will be deleted once confirmed
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retrun DeletePaymentHistory with paymentHistory data</returns>
        // GET: PaymentHistories/
        /// <includesource>yes</includesource>
        public ActionResult DeletePaymentHistory(int? id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentHistory paymentHistory = db.PaymentHistories.Find(id);
            if (paymentHistory == null)
            {
                return HttpNotFound();
            }
            return View(paymentHistory);
            //return this.PartialView("_EditPayHistory");
        }
        /// <summary>
        /// Get Payemnt History found with the passed id from database
        /// remove the found record from the database
        /// Change the related payments attributes.
        /// saves it to the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns IndexPaymentHisotry</returns>
        /// <includesource>yes</includesource>
        [HttpPost, ActionName("DeletePaymentHistory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentHistory paymentHistory = db.PaymentHistories.Find(id);
            var paymentId = paymentHistory.PaymentId;
            db.PaymentHistories.Remove(paymentHistory);
            db.SaveChanges();
            //find paymennt record relats to this payment history and make amendment 
            var selectedPay = db.Payments.Find(paymentId);
            if (paymentHistory.PaymentType == PaymentType.Deposit)
            {
                if (paymentHistory.CreditAmount > 0)
                {
                    if (paymentHistory.DebitAmount == 0)
                    {
                        selectedPay.CreditDepositAmount = selectedPay.CreditDepositAmount - paymentHistory.CreditAmount;
                    }
                    if (paymentHistory.DebitAmount > 0)
                    {
                        selectedPay.CreditDepositAmount = selectedPay.CreditDepositAmount - paymentHistory.CreditAmount;
                        selectedPay.DebitDepositAmount = selectedPay.DebitDepositAmount - paymentHistory.DebitAmount;
                    }
                }
                if (paymentHistory.CreditAmount == 0)
                {
                    if (paymentHistory.DebitAmount > 0)
                    {
                        selectedPay.DebitDepositAmount = selectedPay.DebitDepositAmount - paymentHistory.DebitAmount;
                    }
                }
            }
            if (paymentHistory.PaymentType == PaymentType.Remaining)
            {
                if (paymentHistory.CreditAmount > 0)
                {
                    if (paymentHistory.DebitAmount == 0)
                    {
                        selectedPay.CreditRemainingAmount = selectedPay.CreditRemainingAmount - paymentHistory.CreditAmount;
                        //selectedPay.PaymentType = "Remaining";
                    }
                    if (paymentHistory.DebitAmount > 0)
                    {
                        selectedPay.CreditRemainingAmount = selectedPay.CreditRemainingAmount - paymentHistory.CreditAmount;
                        selectedPay.DebitRemainingAmount = selectedPay.DebitRemainingAmount - paymentHistory.DebitAmount;
                    }
                }

                if (paymentHistory.CreditAmount == 0)
                {
                    if (paymentHistory.DebitAmount > 0)
                    {
                        selectedPay.DebitRemainingAmount = selectedPay.DebitRemainingAmount - paymentHistory.DebitAmount;
                        //selectedPay.PaymentType = "Cancelled";
                    }
                }
            }
            if (paymentHistory.PaymentType == PaymentType.CancellationFee)
            {
                selectedPay.CreditCancellationFee = selectedPay.CreditCancellationFee - paymentHistory.CreditAmount;
                //selectedPay.PaymentType = "CancellationFee";              
            }
            if (ModelState.IsValid)
            {
                db.Entry(selectedPay).State = EntityState.Modified;
                db.SaveChanges();
            }
            //} end of foreach

            return RedirectToAction("IndexPaymentHistory");
            //return View("_EditPayHistory", paymentHistory);
        }

        //[AllowAnonymous]
        //[ChildActionOnly]
        //public ActionResult BookingUpdateSummary()
        //{
        //    var bkgToBeUpdated = GetBookedStateTobeUpdatedCount();

        //    ViewData["BookingCount"] = bkgToBeUpdated;

        //    return PartialView("BookingUpdateSummary");
        //}
        //public int GetBookedStateTobeUpdatedCount()
        //{
        //    // Get the count of each item in the cart and sum them up
        //    int? count = (from b in db.Bookings
        //                  where b.BookingStatus == BookingStatus.Booked && b.BookingEndDateTime < DateTime.Now
        //                  select (int?)b.BookingId).Count();

        //    // Return 0 if all entries are null
        //    return count ?? 0;
        //}

        /// <summary>
        /// dispose database related garbege after use 
        /// so that entitiy frame work can work properly
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