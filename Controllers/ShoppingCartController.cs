using AlfaAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlfaAccounting.Controllers
{
    public class ShoppingCartController : Controller
    {
        /// <summary>
        /// Mie Tanaka
        /// Version 0.0
        /// 26/05/2017
        /// </summary>
        ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Get shopping cart items that takes parameter of http context
        /// Create ShoppingCartViewModel and pass that model to View
        /// </summary>
        /// <returns>Returns shopping cart index view</returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                DepositTotal = cart.GetDeposit(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        // GET: /Store/AddToCart/5

        //public ActionResult AddToCart(int id)
        //{

        //    // Retrieve the album from the database
        //    var addedBooking = db.Bookings
        //        .Single(b => b.BookingId == id);

        //    // Add it to the shopping cart
        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    cart.AddToCart(addedBooking);

        //    // Go back to the main store page for more shopping
        //    return RedirectToAction("Index");
        //}
        //public void AddToCart(int id)
        //{

        //    // Retrieve the album from the database
        //    var addedBooking = db.Bookings
        //        .Single(b => b.BookingId == id);

        //    // Add it to the shopping cart
        //    var cart = ShoppingCart.GetCart(this.HttpContext);

        //    cart.AddToCart(addedBooking);

        //}

        //
        /// <summary>
        /// helper method
        /// Remove passed bookingid 's booking from the shopping cart and database
        /// reflect the change on the view with ajax post
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ShoppingcartIndex view</returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // get item to Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the Booking ItemDescription to display confirmation
            var bkg = db.Bookings.Single(item => item.BookingId == id);
            if (bkg != null)
            {
                var itemDescription = bkg.ItemDescription;

                // Remove the from cart
                int itemCount = cart.RemoveFromCart(id);
                db.Bookings.Remove(bkg);

                // Display the confirmation message
                var results = new ShoppingCartRemoveViewModel
                {
                    Message = Server.HtmlEncode(itemDescription) +
                        " has been removed from your shopping cart.",
                    CartTotal = cart.GetTotal(),
                    DepositTotal = cart.GetDeposit(),
                    SCartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };

                return Json(results);
            }else
            {
                return null;
            }
        }


        // GET: /ShoppingCart/CartSummary
        /// <summary>
        /// count number of item in shopping cart and pass that to a partial view 
        /// </summary>
        /// <returns>partial view CartSummary</returns>
        /// <includesource>yes</includesource>
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }
    }

}
