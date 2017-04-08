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

namespace AlfaAccounting.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //identity's id is string so it is not included, phone number is alreaded included to 
        [Required(ErrorMessage = "Companyname required")]
        [Display(Name = "Company Name")]
        public string Companyname { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        [Required(ErrorMessage = "Street name required")]
        [Display(Name = "Stree name")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Town/City required")]
        [Display(Name = "Town/City")]
        public string Town { get; set; }
        [Required(ErrorMessage = "Postcode required")]
        public string Postcode { get; set; }
        //navigational properties

        public virtual ICollection<Comment> Comments { get; set; }
        //       public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        public ApplicationUser()
        {
            Comments = new List<Comment>();
            Blogs = new List<Blog>();
            Bookings = new List<Booking>();
            Invoices = new List<Invoice>();
            Payments = new List<Payment>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
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
        [DataType(DataType.Date)]
        public DateTime BookedDate { get; set; }
        public DateTime BookingStartDateTime { get; set; }
        public DateTime BookingEndDateTime { get; set; }

        [Display(Name = "Booking Status")]
        public string BookingStatus { get; set; }
        public TimeSpan BookingDuration { get; set; }
        [Display(Name = "Adjustment Hrs i.e. 1.5, 1.25, 1.75 etc")]
        public float BookingAdjustmentHrs { get; set; }
        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }
        [DataType(DataType.Currency)]
        public float Subtotal { get; set; }
        //navigational properties
        [ForeignKey("UnitPrice")]
        public int? UnitPriceId { get; set; }
        public virtual UnitPrice UnitPrice { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        //[ForeignKey("Invoice")]
        //public int? InvoiceId { get; set; }
        //public virtual Invoice Invoice { get; set; }
        public virtual ICollection<Invoice> Invoices{get;set;}
       //public virtual ICollection<BookingInvoiceLine> BookingInvoiceLines { get; set; }
        //construtor
        public Booking()
        {
            BookedDate = DateTime.Now;
            BookingStartDateTime = DateTime.Now;
            BookingEndDateTime = DateTime.Now;
            Invoices = new List<Invoice>();
            //BookingInvoiceLines = new List<BookingInvoiceLine>();

        }

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
    public string InvoiceReference { get; set; }
    public DateTime InvoiceIssueDate { get; set; }
    [Display(Name = "Payable")]
    [DataType(DataType.Currency)]
    public float ReceivableRemainingAmount { get; set; }
    [Display(Name = "Deposit Payable")]
    [DataType(DataType.Currency)]
    public float ReceivableDepoisitAmount { get; set; }
    [Display(Name = "In credit")]
    [DataType(DataType.Currency)]
    public float PayableAmount { get; set; }
    [Display(Name = "Deposit in credit")]
    [DataType(DataType.Currency)]
    public float PayableDepositAmount { get; set; }
   //navigational properties
    [ForeignKey("ApplicationUser")]
    public string Id { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; }
    public virtual ICollection<Payment> Payments { get; set; }
//    public virtual ICollection<BookingInvoiceLine> InvoiceLines { get; set; }
    //constructor
    public Invoice()
        {
            InvoiceIssueDate = DateTime.Now;
            Bookings = new List<Booking>();
            Payments = new List<Payment>();
//            InvoiceLines = new List<BookingInvoiceLine>();
        }
    }

    public enum InvoiceReference
    {
        Deposit,
        Remaining,
        Credit,
        Extra
    }
    public class Payment
    {   [Key]
        public int PaymentId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string PaymentMethod { get; set; }
        [DataType(DataType.Currency)]
        public float CreditAmount { get; set; }
        [DataType(DataType.Currency)]
        public float DebitAmount { get; set; }
        //navigational properties
        [ForeignKey("Invoice")]
        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public enum PaymentMethod
    {
        BACS,
        DD,
        CD,
        Paypal
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
//        [Required]
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
        //        [Required]
        //        [RegularExpression(@"^(?:.*[a-z]){30,}$", ErrorMessage = "String length must be greater than or equal 30 characters.")]
        [RegularExpression(@"^.{30,}$", ErrorMessage = "Minimum 30 characters required")]
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
}








