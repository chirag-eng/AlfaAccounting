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

namespace AlfaAccounting.Controllers
{
    public class BookingViewModelsController : Controller
    {
        private ApplicationUserManager _userManager;
        public IBraintreeConfiguration config = new BraintreeConfiguration();
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookingViewModels
        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult BookDates()
        {
            return View();
        }
        /*        public ActionResult BookDates()
                {
                    return View();
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult BookDates([Bind(Include = "BookingId, BookedDate, BookingStartDateTime, BookingEndDateTime, BookingDuration, BookingStatus, BookingDurationAdjustedHrs, ItemDescription, Subtotal, UnitPriceId, Id")] Booking booking)
                {
                    if (ModelState.IsValid)
                    {
                        booking.Id = User.Identity.GetUserId();
                        booking.BookedDate = DateTime.Now.Date;
                        db.Bookings.Add(booking);
                        db.SaveChanges();
                    }
                    return View(booking);
                }
                */



        public ActionResult ConfirmBooking()
        {
            //            if (ModelState.IsValid)
            //            {  


            //            CultureInfo gb = new CultureInfo("en-GB");

            var listNewBookingToBeSavedToDb = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
            //var listNewBookingInvoiceLineToBeSavedToDb = Session["ListNewBookingInvoiceLineToBeSavedToDb"] as List<BookingInvoiceLine>;

            var model = new ConfirmBookingViewModel()
            {
                InvoiceReference = "Deposit",
                InvoiceIssueDate = DateTime.Now,
                Id = User.Identity.GetUserId(),
                //                Bookings = db.Bookings.Where(b => b.Invoice.ToString().Contains(null)).Where(b => b.Id.Equals(loggedinId)).ToList(),               
                //Bookings = db.Bookings.Where(b => !b.Invoices.Any()).Where(b => b.ApplicationUser.Id.Equals(loggedinIlistNewBookingToBeSavedToDbd)).ToList(),
                Bookings = listNewBookingToBeSavedToDb,
                ReceivableRemainingAmount = 0f,
                PayableAmount = 0f,
                PayableDepositAmount = 0f
            };
            var newInvoice = new Invoice()
            {   InvoiceId = model.InvoiceId,
                InvoiceReference = model.InvoiceReference,
                InvoiceIssueDate = model.InvoiceIssueDate,
                ReceivableRemainingAmount = model.ReceivableRemainingAmount,
 //               ReceivableDepoisitAmount = model.ReceivableDepoisitAmount,
                PayableAmount = model.PayableAmount,
                PayableDepositAmount = model.ReceivableDepoisitAmount,
                Id = model.Id,
//                Bookings = model.Bookings
             };
            //foreach(BookingInvoiceLine bkgInvLn in listNewBookingInvoiceLineToBeSavedToDb)
            //{
            //    model.ReceivableDepoisitAmount += bkgInvLn.Subtotal * 0.1f;
            //    bkgInvLn.InvoiceId = newInvoice.InvoiceId;
            //}

            //This Invoices bookings collections get this invoice object added to thier Invoice collection
            foreach (Booking bk in model.Bookings)
            {
                model.ReceivableDepoisitAmount += bk.Subtotal * 0.1f;
                bk.Invoices.Add(newInvoice);

             };
                newInvoice.ReceivableDepoisitAmount = model.ReceivableDepoisitAmount;           
                newInvoice.Bookings = model.Bookings;
            
            //           db.SaveChanges();
            Session["ListNewBookingToBeSavedToDb"] = listNewBookingToBeSavedToDb;
            Session["NewInvoice"] = newInvoice;
            return View(model);

        }


       /*        public ActionResult ProceedToPayment()
               {
                   var gateway = config.GetGateway();
                   var clientToken = gateway.ClientToken.generate();
                   ViewBag.ClientToken = clientToken;
                   return View();
               }

               public async Task<ActionResult> ProceedToPayment(ProceedPaymentViewModel model)
               {
                   if (ModelState.IsValid)
                   {
                       var payviewModel = new ProceedPaymentViewModel()
                       {
                           PaymentDate = DateTime.Now,
                           CreditAmount = model.CreditAmount,
                           DebitAmount = model.DebitAmount,
                           PaymentMethod = model.PaymentMethod,
                           CardNumber = model.CardNumber,
                           ExpiryDate = model.ExpiryDate,
                           CVVNumber = model.CVVNumber
                       };
                       var userId = User.Identity.GetUserId();
                       db.SaveChanges();
       //                string callbackUrl = await SendEmailPaymentConfirmationTokenAsync(userId, "Payment Confirmation email has been sent");
                       ViewBag.Message = "Check your email and confirm you may need to register again";

                       return View("Info");
                   }
                   return View(model);
               }
               */
        // to send email confirmation again to the user who deleted email or email never arrived

        public static readonly TransactionStatus[] transactionSuccessStatuses = {
                                                                                    TransactionStatus.AUTHORIZED,
                                                                                    TransactionStatus.AUTHORIZING,
                                                                             TransactionStatus.SETTLED,
                                                                                    TransactionStatus.SETTLING,
                                                                                    TransactionStatus.SETTLEMENT_CONFIRMED,
                                                                                    TransactionStatus.SETTLEMENT_PENDING,
                                                                                    TransactionStatus.SUBMITTED_FOR_SETTLEMENT
                                                                                };

        public ActionResult New()
        {
            var gateway = config.GetGateway();
            var clientToken = gateway.ClientToken.generate();
            ViewBag.ClientToken = clientToken;

            var newInvoice = Session["NewInvoice"] as Invoice;

            if (newInvoice.InvoiceReference.Equals("Deposit"))
            {
                ViewBag.amountValue = newInvoice.ReceivableDepoisitAmount.ToString();
            }
            else if (newInvoice.InvoiceReference.Equals("Remainder"))
            {
                ViewBag.amountValue = newInvoice.ReceivableRemainingAmount.ToString();
            }


            return View();
        }

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


                var listNewBookingToBeSavedToDb = Session["ListNewBookingToBeSavedToDb"] as List<Booking>;
                var newInvoice = Session["NewInvoice"] as Invoice;

                db.Invoices.Add(newInvoice);
                db.SaveChanges();

                foreach (Booking bkg in listNewBookingToBeSavedToDb)
                {
                    db.Bookings.Add(bkg);
                }
                db.SaveChanges();

                //save payemnt detail to the database 
                var newPayment = new Payment()
                {
                    PaymentDateTime = DateTime.Now,
                    CreditAmount = (float)newInvoice.ReceivableDepoisitAmount + (float)newInvoice.ReceivableRemainingAmount,
                    DebitAmount = 0f,
                    Id = User.Identity.GetUserId(),
                    InvoiceId = newInvoice.InvoiceId
                };
                db.Payments.Add(newPayment);
                db.SaveChanges();

                //clear both booking and invoice sessions
                Session["ListNewBookingToBeSavedToDb"] = null;
                Session["NewInvoice"] = null;

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

        public async Task<ActionResult> Show(String id)
        {
            var gateway = config.GetGateway();
            Transaction transaction = gateway.Transaction.Find(id);

            if (transactionSuccessStatuses.Contains(transaction.Status))
            {
                TempData["header"] = "Sweet Success!";
                TempData["icon"] = "success";
                TempData["message"] = "Your test transaction has been successfully processed. See the Braintree API response and try again.";
            }
            else
            {
                TempData["header"] = "Transaction Failed";
                TempData["icon"] = "fail";
                TempData["message"] = "Your test transaction has a status of " + transaction.Status + ". See the Braintree API response and try again.";
            };

            ViewBag.Transaction = transaction;
            TempData["Transaction"] = transaction;
            
            //send payemnt confirmation email
            string callbackUrl = await SendPaymentConfirmationEmailTokenAsync(User.Identity.GetUserId(), "Your payment confirmation from AlfaAccounting");
 

            return View();
        }

        private async Task<string> SendPaymentConfirmationEmailTokenAsync(string userID, string subject)
        {
            var trans = TempData["Transaction"] as Transaction;
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            //            await UserManager.SendEmailAsync(userID, subject, "Please confirm your accout by clicking <a href=\"" + callbackUrl + "\">here</a>");
            await UserManager.SendEmailAsync(userID, subject, "Thank you very much for your payment of " + trans.Amount + ". your payment process number is " + trans.CreditCard.Token);
            return callbackUrl;
        }
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

        public ActionResult Invoices()
        {
            return View();
        }

        public ActionResult Payments()
        {
            return View();
        }



        private void saveBookingsToDb(List<Booking> newbookings)
        {

        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}