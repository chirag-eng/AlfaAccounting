using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using AlfaAccounting.Controllers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace AlfaAccounting.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //identity's id is string so it is not included, phone number is alreaded included to 
        [Required(ErrorMessage = "Company Name required")]
        [Display(Name = "Company Name")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Company Name Minimum 2 characters required")]
        public string Companyname { get; set; }
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Forename Minimum 2 characters required")]
        public string Forename { get; set; }
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Surname Minimum 2 characters required")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Street name required")]
        [Display(Name = "Stree name")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Street Name Minimum 2 characters required")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Town/City required")]
        [Display(Name = "Town/City")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Town/City Minimum 2 characters required")]
        public string Town { get; set; }
        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "Postcode required")]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2}, maximum 12 characters long.", MinimumLength = 2)]
        public string Postcode { get; set; }
        public bool BacsApproved { get; set; }
        //navigational properties

        public virtual ICollection<Comment> Comments { get; set; }
        //       public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        public ApplicationUser()
        {
            Comments = new List<Comment>();
            Blogs = new List<Blog>();
            Bookings = new List<Booking>();
            Invoices = new List<Invoice>();
            Payments = new List<Payment>();
            PaymentHistories = new List<PaymentHistory>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class InvoiceToBeSentDate
    {
        [Key]
        public int DateInvoiceToBeSentId { get; set; }
        public DateTime DateOftheMonth { get; set; }
    }

    public class DepositRate
    {
        [Key]
        public int DepositRateId { get; set; }
        [Required(ErrorMessage = "Deposit Rate Required")]
        public float DepositRateValue { get; set; }
    }
    public class UnitPrice
    {
        [Key]
        public int UnitPriceId { get; set; }
        [Required(ErrorMessage ="Unit Price Value Required")]
        public float UnitPriceValue { get; set; }
        public string UnitPriceDescription { get; set; }
    }

    public class Booking
    {
        [Key/*, DatabaseGenerat‌​ed(DatabaseGeneratedOp‌​tion.None)*/]
        public int BookingId { get; set; }
        [Display(Name = "Booked Date")]
        public DateTime BookedDate { get; set; }
        [Display(Name = "Booked Start Date and time")]
        public DateTime BookingStartDateTime { get; set; }
        [Display(Name = "Booked End Date and time")]
      //  [DisplayFormat(DataFormatString =
      //"{0:yyyy-MM-dd}",
      // ApplyFormatInEditMode = true)]
        public DateTime BookingEndDateTime { get; set; }
        [Display(Name = "Booking Status")]
        public BookingStatus BookingStatus { get; set; }
        //public TimeSpan BookingDuration { get; set; }
        [Display(Name = "Adjustment Hrs i.e. 1.5, 1.25, 1.75 etc")]
        public float BookingAdjustmentHrs { get; set; }
        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }
        [DataType(DataType.Currency)]
        public float Subtotal { get; set; }
        [DataType(DataType.Currency)]
        public float BookingDeposit { get; set; }
        public float BookingCancellationFee { get; set; }
        public bool VisitConfirmationSent { get; set; }
        //navigational properties
        [Display(Name ="Unit Price")]
        [ForeignKey("UnitPrice")]
        public int? UnitPriceId { get; set; }
        public virtual UnitPrice UnitPrice { get; set; }
        [Display(Name = "User")]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name ="Invoice No")]
        [ForeignKey("Invoice")]
        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        //public virtual ICollection<Invoice> Invoices{get;set;}
        //public virtual ICollection<BookingInvoiceLine> BookingInvoiceLines { get; set; }
        //construtor
        public Booking()
        {
            BookedDate = DateTime.Now;
            BookingStartDateTime = DateTime.Now;
            BookingEndDateTime = DateTime.Now;
            //Invoices = new List<Invoice>();
            Subtotal = 0f;
            BookingCancellationFee = 0f;
            BookingDeposit = 0f;
            //BookingInvoiceLines = new List<BookingInvoiceLine>();
        }

    }

    public enum BookingStatus
    {
        Booked,
        Visited,
        Extended,
        Curtailed,
        Cancelled
    }

    //public class BookingInvoiceLine
    //{
    //    [Key]
    //    public int BookingInvoiceLineId { get; set; }
    //    [DataType(DataType.Currency)]
    //    public float Subtotal { get; set; }
    //    [Display(Name = "Adjustment Hrs i.e. 1.5, 1.25, 1.75 etc")]
    //    public TimeSpan BookingDuration { get; set; }
    //    [DataType(DataType.Currency)]
    //    public float BookingAdjustmentHrs { get; set; }
    //    [Display(Name = "Item Description")]
    //    public string ItemDescription { get; set; }
    //    //navigational properties
    //    [ForeignKey("UnitPrice")]
    //    public int? UnitPriceId { get; set; }
    //    public virtual UnitPrice UnitPrice { get; set; }
    //    [ForeignKey("Invoice")]
    //    public int InvoiceId { get; set; }
    //    public virtual Invoice Invoice { get; set; }
    //    [ForeignKey("Booking")]
    //    public int BookingId { get; set; }
    //    public virtual Booking Booking { get; set; }
    //}

    public class Invoice {
        [Key/*, DatabaseGenerat‌​ed(DatabaseGeneratedOp‌​tion.None)*/]
        public int InvoiceId { get; set; }
    //public string InvoiceType { get; set; }
    public DateTime InvoiceIssueDate { get; set; }
    [Display(Name = "Payable")]
    [DataType(DataType.Currency)]
    public float ReceivableRemainingAmount { get; set; }
    [Display(Name = "Deposit Payable")]
    [DataType(DataType.Currency)]
    public float ReceivableDepositAmount { get; set; }
    [Display(Name = "In credit")]
    [DataType(DataType.Currency)]
    public float PayableAmount { get; set; }
    [Display(Name = "Deposit in credit")]
    [DataType(DataType.Currency)]
    public float PayableDepositAmount { get; set; }
    public float InvoiceCancellationFee { get; set; }
        //navigational properties
        [ForeignKey("ApplicationUser")]
    public string Id { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
   public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        //    public virtual ICollection<BookingInvoiceLine> InvoiceLines { get; set; }
        //constructor
        public Invoice()
        {
            InvoiceIssueDate = DateTime.Now;
            Bookings = new List<Booking>();
            Payments = new List<Payment>();
            PaymentHistories = new List<PaymentHistory>();
            //            InvoiceLines = new List<BookingInvoiceLine>();
            ReceivableRemainingAmount = 0f;
            ReceivableDepositAmount = 0f;
            PayableAmount = 0f;
            PayableDepositAmount = 0f;
            InvoiceCancellationFee = 0f;
        }
    }

   
    public class Payment
    {   [Key]
        public int PaymentId { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public string PaymentType { get; set; }
        //[DataType(DataType.Currency)]
        //public float CreditAmount { get; set; }
        //[DataType(DataType.Currency)]
        //public float DebitAmount { get; set; }
        [DataType(DataType.Currency)]
        public float CreditDepositAmount { get; set; }
        [DataType(DataType.Currency)]
        public float DebitDepositAmount { get; set; }
        public float CreditRemainingAmount { get; set; }
        [DataType(DataType.Currency)]
        public float DebitRemainingAmount { get; set; }
        [DataType(DataType.Currency)]
        public float CreditCancellationFee { get; set; }
        public string TransactionId { get; set; }
        //navigational properties
        [ForeignKey("Invoice")]
        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        //default constructor
        public Payment()
        {
            PaymentDateTime = null;
            CreditDepositAmount = 0f;
            DebitDepositAmount = 0f;
            CreditRemainingAmount = 0f;
            DebitRemainingAmount = 0f;
            CreditCancellationFee = 0f;
            PaymentHistories = new List<PaymentHistory>();
        }
    }

    public class PaymentHistory
    {
        [Key]
        public int PaymentHistoryId { get; set; }
        [Required(ErrorMessage = "Payment Date required")]
        //[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Paid Date")]
        //[DataType(DataType.Date)]
        public DateTime? PaymentDateTime { get; set; }
        [Required(ErrorMessage = "Payment Type required")]
        [Display(Name = "Payment Type")]
        public PaymentType PaymentType { get; set; }
        [Display(Name = "Credit Amount")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Credit Amount Required")]
        [Range(0.0, float.MaxValue, ErrorMessage = "must be positive numbers")]
        public float CreditAmount { get; set; }
        [Display(Name = "Debit Amount")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Debit Amount Required")]
        [Range(0.0, float.MaxValue, ErrorMessage = "must be positive numbers")]
        public float DebitAmount { get; set; }
        public string TransactionId { get; set; }
       // navigational properties
        [ForeignKey("Payment")]
        public int? PaymentId { get; set; }
        public virtual Payment Payment { get; set; }
        [ForeignKey("Invoice")]
        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //default constructor
        public PaymentHistory()
        {
            PaymentDateTime = null;
            CreditAmount = 0f;
            DebitAmount = 0f;
        }
    }

    public enum PaymentType
    {
        Deposit =1,
        Remaining = 0,
        CancellationFee =2,
        DepositCancelled =3

    }
    
    public class Category
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
//        [StringLength(25)]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        //navigational properties
        public virtual ICollection<Blog> Blogs { get; set; }
        public Category()
        {
            Blogs = new List<Blog>();
        }
    }

    public class Comment
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CommentId { get; set; }

        //       [DefaultValue(typeof(DateTime), "")]
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}",ApplyFormatInEditMode = true)]

        [DisplayName("Commented date")]
        [DataType(DataType.Date)]
        public DateTime CommentedDate { get; set; }
//        [Required]
//        [StringLength(100)]
        [DisplayName("Coment title")]
        public string CommentTitle { get; set; }
        [Required(ErrorMessage ="Comment Context is required")]
        [DisplayName("Coment Body")]
        public string CommentBody { get; set; }

        //Navigational properties
        [DisplayName("Blog")]
        [ForeignKey("Blog")]
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [DisplayName("User")]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public Comment()
        {
            CommentedDate = DateTime.Now.Date;
        }

    }

    public class Blog
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BlogId { get; set; }
//        [Required]
//          [StringLength(50, ErrorMessage = "The {0} must be at least {2}, maximum 50characters long.", MinimumLength = 6)]
          [DisplayName("Blog title")]
        public string BlogTitle { get; set; }



        //[RegularExpression(@"^(?:.*[a-z]){300,}$", ErrorMessage = "String length must be greater than or equal 300 characters")]
        [Required(ErrorMessage ="Blox context required")]
        [RegularExpression(@"^.{300,}$", ErrorMessage = "Minimum 300 characters required")]
        [DisplayName("Blog content")]
        public string BlogContent { get; set; }
        //        [DisplayFormat(DataFormatString = "{0:dd-mmm-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime BlogDate { get; set; }
        [DisplayName("Approved")]
        public bool BlogApproved { get; set; }

        //navigational properties
        [DisplayName("Category")]
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [DisplayName("User")]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public Blog()
        {
            Comments = new List<Comment>();
            BlogDate = DateTime.Now.Date;
        }
    }


    public class Cart
    {   [Key]
        public int CartRecordId { get; set; }
        public string CartId { get; set; }
        public int CartCount { get; set; }
        public DateTime CartDateCreated { get; set; }
        //navigational properties
        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }

    }

    public partial class ShoppingCart
    {
        ApplicationDbContext db = new ApplicationDbContext();

        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public void AddToCart(Booking booking)
        {
            // Get the matching cart and booking instances
            var cartItem = db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.BookingId == booking.BookingId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    BookingId = booking.BookingId,
                    CartId = ShoppingCartId,
                    CartCount = 1,
                    CartDateCreated = DateTime.Now
                };

                db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.CartCount++;
            }

            // Save changes
            db.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart that get removed
            var cartItem = db.Carts.Single(cart => cart.CartId == ShoppingCartId
                           && cart.CartRecordId == id);
            // Get the booking that get removed
            var bkg = db.Bookings.Single(b => b.BookingId == cartItem.BookingId);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.CartCount > 1)
                {
                    cartItem.CartCount--;
                    itemCount = cartItem.CartCount;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                    db.Bookings.Remove(bkg);
                }

                // Save changes
                db.SaveChanges();
            }

            return itemCount;
        }


        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }


            // Save changes
            db.SaveChanges();
        }

        public void EmptyBooking()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                var bookingItems = db.Bookings.Where(b => b.BookingId == cartItem.BookingId);

                foreach (var bookingItem in bookingItems)
                {
                    db.Bookings.Remove(bookingItem);
                }
            }

            // Save changes
            db.SaveChanges();

        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.CartCount).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }

        public float GetTotal()
        {
            // Multiply Booking price by unit price
            // the current price for each of those bookings in the cart
            // sum all booking price totals to get the cart total
            float? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Booking.Subtotal).Sum();
            return total ?? 0f;
        }

        public float GetDeposit()
        {
            
                // Multiply Booking price by unit price mutiply by depositrate
                // the current deposit for each of those bookings in the cart
                // sum all booking deposit totals to get the cart total
                float? total = (from cartItems in db.Carts
                                  where cartItems.CartId == ShoppingCartId
                                  select (int?)cartItems.Booking.Subtotal * db.DepositRates.Select(d => d.DepositRateValue).FirstOrDefault()).Sum();
                return total ?? 0f;
        }

        //public int CreateInvoice(Invoice Invoice)
        //{
        //    decimal orderTotal = 0m;

        //    var cartItems = GetCartItems();

        //    // Iterate over the items in the cart, adding the order details for each
        //    var newInvoice = new Invoice()
        //    {

        //        InvoiceType = "Deposit",
        //        InvoiceIssueDate = DateTime.Now,
        //        ReceivableRemainingAmount = 0f,
        //        PayableAmount = 0f,
        //        PayableDepositAmount = 0f,

        //    };
        //    foreach (var item in cartItems)
        //    {
        //        newInvoice.Bookings.Add(db.Bookings.SingleOrDefault(b => b.BookingId == item.BookingId));
        //    }

        //    foreach (Booking bk in newInvoice.Bookings)
        //    {
        //        var bookingDuration = bk.BookingEndDateTime - bk.BookingStartDateTime;
        //        var unitP = db.UnitPrices.SingleOrDefault(u => u.UnitPriceDescription == "Standard").UnitPriceValue;

        //        if (bk.BookingAdjustmentHrs < 0) { newInvoice.InvoiceType = "Credit"; }

        //        if (newInvoice.ReceivableRemainingAmount > 0) { newInvoice.InvoiceType = "Remining"; }
        //        if (bk.BookingAdjustmentHrs > 0) { newInvoice.InvoiceType = "Extra"; }

        //        if (newInvoice.InvoiceType == "Deposit")
        //        {
        //            newInvoice.ReceivableDepositAmount += (float)bk.BookingDuration.TotalMinutes/60* unitP * db.DepositRates.Select(d=>d.DepositRateValue).FirstOrDefault();
        //        }
        //        else if (newInvoice.InvoiceType == "Remaining")
        //        {
        //            newInvoice.ReceivableRemainingAmount += (float)bk.BookingDuration.TotalMinutes / 60 * unitP * 0.9f;
        //        }
        //        else if (newInvoice.InvoiceType == "Credit") { }
        //        if (bk.BookingAdjustmentHrs < 0)
        //        {
        //            newInvoice.PayableAmount += bk.BookingAdjustmentHrs * unitP;
        //        }
        //        else if (newInvoice.InvoiceType == "Extra")
        //        {
        //            newInvoice.ReceivableRemainingAmount += (float)(bk.BookingEndDateTime - bk.BookingStartDateTime).TotalMinutes/60 * unitP * 0.9f + (bk.BookingAdjustmentHrs * unitP * 1);
        //        }
        //        bk.Invoices.Add(newInvoice);

        //    };
        //    //     newInvoice.ReceivableDepositAmount = model.ReceivableDepositAmount;           
        //    //     newInvoice.Bookings = model.Bookings;

        //    db.Invoices.Add(newInvoice);


        //    // Save the order
        //    db.SaveChanges();

        //    // Empty the shopping cart
        //    EmptyCart();

        //    // Return the InvoiceId as the confirmation number
        //    return Invoice.InvoiceId;
        //}

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userid)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userid;
                db.Bookings.SingleOrDefault(b => b.BookingId == item.BookingId).Id = userid;

            }

            
            db.SaveChanges();
        }
    }
}








