using AlfaAccounting.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AlfaAccounting.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UnitPrice> UnitPrices { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<InvoiceToBeSentDate> InvoiceToBeSentDates { get; set; }
        public DbSet<DepositRate> DepositRates { get; set; }
        //       public DbSet<BookingInvoiceLine> BookingInvoiceLines { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

       

        /*       protected override void OnModelCreating(DbModelBuilder modelBuilder)
               {

                   modelBuilder.Configurations.Add(new BlogTypeConfiguration());
                   modelBuilder.Configurations.Add(new CommentTypeConfiguration());

                   base.OnModelCreating(modelBuilder);
               }

               public class BlogTypeConfiguration : EntityTypeConfiguration<Blog>
               {
                   public BlogTypeConfiguration()
                   {//one-to-many
                    //               HasOptional(b => b.Category)
                    //                                         .WithMany(c => c.Blogs).HasForeignKey(b => b.CategoryId);
                       HasOptional(b => b.ApplicationUser)
                                          .WithMany(u => u.Blogs).HasForeignKey(b => b.Id);
                       //HasMany(b => b.Comments).WithRequired(c => c.Blog)
                       //.HasForeignKey(c => c.BlogId).WillCascadeOnDelete(true);
                   }
               }

               public class CommentTypeConfiguration : EntityTypeConfiguration<Comment>
               {
                   public CommentTypeConfiguration()
                   {//one-to-many
                    //               HasOptional<Blog>(C => C.Blog)
                    //                                         .WithMany(b => b.Comments).HasForeignKey(c => c.BlogId);
                       HasOptional(b => b.ApplicationUser)
                                          .WithMany(u => u.Comments).HasForeignKey(c => c.Id);
                       // HasMany(b => b.Comments).WithRequired(c => c.Blog)
                       // .HasForeignKey(c => c.BlogId).WillCascadeOnDelete(true);
                   }
               }

                       protected override void OnModelCreating(DbModelBuilder modelBuilder)
                       {
                           //configure default schema
                           //            modelBuilder.HasDefaultSchema("Admin");
                           //Map entity to table
                           //         modelBuilder.Entity<Category>().ToTable("Category");
                           //         modelBuilder.Entity<Blog>().ToTable("Blogs");
                           //        modelBuilder.Entity<Comment>().ToTable("Comments");
               //                       modelBuilder.Entity<Category>().HasMany(c => c.Blogs).WithOptional(b => b.Category)
               //                                  .HasForeignKey(b => b.CategoryId).WillCascadeOnDelete(false);
               //                       modelBuilder.Entity<Blog>().HasMany(b => b.Comments).WithRequired(c => c.Blog)
               //                                  .HasForeignKey(c => c.BlogId).WillCascadeOnDelete(true);
               //                       modelBuilder.Entity<ApplicationUser>().HasMany(ApplicationUser => ApplicationUser.Blogs).WithOptional(b => b.ApplicationUser)
               //                                  .HasForeignKey(b => b.Id).WillCascadeOnDelete(false);
               //                       modelBuilder.Entity<ApplicationUser>().HasMany(ApplicationUser => ApplicationUser.Comments).WithOptional(c => c.ApplicationUser)
               //                                  .HasForeignKey(c => c.Id).WillCascadeOnDelete(false);


                                       modelBuilder.Entity<Blog>().HasRequired<Category>(b => b.Category)
                                                .WithMany(c => c.Blogs).HasForeignKey(b => b.CategoryId).WillCascadeOnDelete(false);

                                       modelBuilder.Entity<Blog>().HasRequired<ApplicationUser>(b => b.ApplicationUser)
                                                    .WithMany(u => u.Blogs).HasForeignKey(b => b.Id).WillCascadeOnDelete(false);

                                       modelBuilder.Entity<Comment>().HasRequired<Blog>(c => c.Blog)
                                                   .WithMany(b => b.Comments).HasForeignKey(c => c.BlogId).WillCascadeOnDelete(true);

                                       modelBuilder.Entity<Comment>().HasRequired<ApplicationUser>(c => c.ApplicationUser)
                                                .WithMany(u => u.Comments).HasForeignKey(c => c.Id).WillCascadeOnDelete(false);

                       }      
                       /*
                       */



    }


    public partial class ApplicationDbContext
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }


    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {

            //Initialize Iendity(context);
            if (!context.Users.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var userStore = new UserStore<ApplicationUser>(context);
                //_________________________________________________________________________
                //populating the role table with the data in side of RoleName class using RoleManager.
                //Role Name class does not need to be created if simplly add "Administrator", "Dog_Owner", "Dog_Walker" string
                if (!roleManager.RoleExists(RoleName.ROLE_ADMINSTRATOR))
                {
                    var roleresult = roleManager.Create(new IdentityRole(RoleName.ROLE_ADMINSTRATOR));
                }
                if (!roleManager.RoleExists(RoleName.ROLE_USER))
                {
                    var roleresult = roleManager.Create(new IdentityRole(RoleName.ROLE_USER));
                }
                if (!roleManager.RoleExists(RoleName.ROLE_STAFF))
                {
                    var roleresult = roleManager.Create(new IdentityRole(RoleName.ROLE_STAFF));
                }

                //pupulating category table
                List<Category> categories = new List<Category>();

                categories.Add(new Category { CategoryName = "Tax" });
                categories.Add(new Category { CategoryName = "Audit" });
                categories.Add(new Category { CategoryName = "General" });
                foreach (Category ctgy in categories) context.Categories.Add(ctgy);
                context.SaveChanges();

                string userName1 = "alfaacc00unting2017@gmail.com"; /*admin @admin2.com*/
                string password1 = "Password#2";
                // var passworddHash = new PasswordHasher(); if migration enable methods this code needs used
                //password = passwordHash.HashPassword(password);
                // Create Admin user and role

                var user = userManager.FindByName(userName1);
                if (user == null)
                {
                    var newUser = new ApplicationUser()
                    { Companyname = "Alfa Accounting", Forename = "Administrator", Surname = "Admin", Street = "190 Catheral Street", Town = "Glasgow", Postcode = "2G1 3DF", PhoneNumber = "0141  43455", UserName = userName1, Email = userName1, EmailConfirmed = true, BacsApproved = false };
                    userManager.Create(newUser, password1);
                    userManager.AddToRole(newUser.Id, RoleName.ROLE_ADMINSTRATOR);

                    var objBlog1 = new Blog()
                    {BlogDate = Convert.ToDateTime("27/10/2016"),BlogApproved = true,CategoryId = 1,Id = newUser.Id,BlogTitle = "Trump tax reforms - what they mean for the UK",
                     BlogContent = "It's no surprise Donald Trump's tax plans are generating international interest. The focal point is the cut in the rate of tax on business' profits from 35% to 15%. The repercussions will undoubtedly be felt in the UK. Tax changes by a leading economy often have a ripple effect.We have seen successive UK governments steadily cut the rate of corporation tax,which will fall to 17 % by 2020 exactly because a competitive tax rate helps signal an economy that is 'Open for Business'.But what happens when every country wants to be open for business? There are concerns about a 'race to the bottom' and particularly the impact this could have on developing countries who rely on profit tax receipts.According to the Oxford University Centre for Business Taxation, since the nineties, the average corporate statutory tax rate in the G20 has declined by 14.35 percentage points, from 41.48 % to 27.08 %.That said, whilst we have seen the UK rate come down we have also seen in a succession of Budgets introduce measures to widen the base upon which the tax is calculated.This results in a more complex tax regime.It's not just the tax rate that matters it's also how easy it is to meet the compliance burden that can make a big difference.Further, corporation tax, paid on the profits of a business. is just one type of tax that businesses bear.While profit taxes have been coming down, other taxes on business have been going up.PwC and WorldBank analysis of tax systems in 190 economies shows more countries are increasing the Total Tax Rate for business than cutting it.Likewise profit taxes and labour related taxes are increasing globally.Whether this trend continues remains to be seen.The UK currently has the lowest profit tax rate in the G20, but that won't be the case if the US rate comes down to 15%.In the meantime, the US has sent a signal worldwide that it will be more tax competitive than it has in the past.This could mark a change in the cost to businesses based or operating there - depending on how the rest of Trump's tax reforms play out. Will this trigger further cuts by the UK Government post Brexit, or indeed other countries world wide? Only time will tell."};


                 var objCommnet1 = new Comment()
                    {CommentedDate = Convert.ToDateTime("15/01/2017"),CommentTitle = "so be it",CommentBody = "Thanks to Mr Trump!!!!!",BlogId = 1,Id = newUser.Id};
                    context.Blogs.Add(objBlog1);
                    context.Comments.Add(objCommnet1);
                    //userManager.Create(newUser, password);
                    //userManager.AddToRole(newUser,Role);

                    //userManager.SetLockourtEnabled(newUser.Id,false)

                    var newUser2 = new ApplicationUser()
                    { Companyname = "Alfa Accounting", Forename = "Mie", Surname = "Tanaka", Street = "1 Glasgow Street", Town = "Glasgow", Postcode = "G1 5AA", PhoneNumber = "014112345", UserName = "mietta25@gmail.com", Email = "mietta25@gmail.com", EmailConfirmed = true, BacsApproved = false };
                    userManager.Create(newUser2, "Password#2");
                    userManager.AddToRole(newUser2.Id, RoleName.ROLE_STAFF);

                    var objBlog2 = new Blog()
                    {BlogDate = Convert.ToDateTime("20/11/2016"),BlogApproved = true,CategoryId = 3,Id = newUser2.Id,BlogTitle = "The acceleration of the Finance Bill - more uncertainty for Business, not less",
                     BlogContent = "The Finance Bill often passes with relatively little fanfare.  It's when new tax measures often announced in a Budget become law.  The legislation has usually been in draft form for a good few months, and niggles hopefully ironed out.But while the Bill is typically passed in July, the Government has accelerated things - as is usually the case before a General Election.  The Bill will receive Royal Assent on Thursday.So as not to rush legislation through without due care and scrutiny, many measures have been left out. This is good news and bad news. On the plus side, there's more time to ensure the legislation is effective.  On the downside, people and businesses won't be sure if the measures left out will ever come into force.Take the Substantial Shareholding Exemption rules. These exempt the disposal of certain shares in subsidiaries from corporation tax on any capital gain. The rules theoretically came into effect on 1 April, so a business making a relevant disposal after that date would have thought they'd benefit from the exemption.  Now they can no longer be sure. While the key driver of the uncertainty is the impending Election, even if the same Government remains in power, there's more likelihood that the rules will evolve given the time lag. It will be important the Government post 8th June sets out its stall quickly on the measures held in limbo - both in terms of scope and timing. Businesses loathe uncertainty (especially in already uncertain times) but it would definitely be worse if poor legislation was rushed through.  Taxpayers could end up able to apply the rules or could even find themselves the wrong side of the law.UK tax code needs a good spring clean - less is definitely more at present.Let's use the time wisely to get rules that work and not create a period of wasted hiatus."};
                    context.Blogs.Add(objBlog2);

                    var objBlog3 = new Blog()
                    {BlogDate = Convert.ToDateTime("20/02/2017"),BlogTitle = "Spring Budget 2017 – Is the Chancellor building for the future?",
                        BlogApproved = false,CategoryId = 3,Id = newUser2.Id,
                     BlogContent = "The forthcoming Budget should give more flavour of the Government’s vision for post Brexit Britain. The recent industrial strategy paper, emphasised a broad approach - creating conditions for businesses of all sizes and across all sectors to thrive. But the language gave clues as to priorities - with an emphasis on industries and services of the future. Whilst the Chancellor has indicated a fairly quiet Budget is on the cards we will at least get more indication of the role tax policy will play in our future.There was almost no mention of tax in the Industrial Strategy paper but tax policy will be crucial in helping achieve the stated objectives,such as helping businesses to scale up.So, in this the first of two Budgets this year we can expect to hear more on the public spending and the tax measures to underpin his plan...and indeed we will see what kind of Chancellor Mr Hammond wants to be."};
                    context.Blogs.Add(objBlog3);

                    var objComment2 = new Comment()
                    {CommentedDate = Convert.ToDateTime("26/01/2017"),CommentTitle = "tough",CommentBody = " Thanks to brexit!!!",BlogId = 3,Id = newUser2.Id};
                    context.Comments.Add(objComment2);

                    var objComment3 = new Comment()
                    {CommentedDate = Convert.ToDateTime("27/01/2017"),CommentTitle = "ok",CommentBody = " lets see what happens",BlogId = 2,Id = newUser2.Id};
                    context.Comments.Add(objComment2);

                    //populating Unit Price table
                    //List<UnitPrice> UnitPrices = new List<UnitPrice>();
                    UnitPrice objUnitPrice1 = new UnitPrice() { UnitPriceId = 1, UnitPriceValue = 100, UnitPriceDescription = "Standard" };
                    context.UnitPrices.Add(objUnitPrice1);
                    UnitPrice objUnitPrice2 = new UnitPrice() { UnitPriceId = 2, UnitPriceValue = 90, UnitPriceDescription = "Discount90" };
                    context.UnitPrices.Add(objUnitPrice2);
                    UnitPrice objUnitPrice3 = new UnitPrice() { UnitPriceId = 3, UnitPriceValue = 80, UnitPriceDescription = "Discount80" };
                    context.UnitPrices.Add(objUnitPrice3);
                    UnitPrice objUnitPrice4 = new UnitPrice() { UnitPriceId = 4, UnitPriceValue = 70, UnitPriceDescription = "Discount70" };
                    context.UnitPrices.Add(objUnitPrice4);
                    //UnitPrices.Add(objUnitPrice4);
                    context.SaveChanges();

                    DepositRate objDepositRate = new DepositRate() { DepositRateValue = 0.10f };
                    context.DepositRates.Add(objDepositRate);
                    context.SaveChanges();

                    InvoiceToBeSentDate objIInvoiceToBeSentDate = new InvoiceToBeSentDate()
                    {
                     DateInvoiceToBeSentId =1, DateOftheMonth = new DateTime(2017,01,28)
                    };
                    context.InvoiceToBeSentDates.Add(objIInvoiceToBeSentDate);
                    context.SaveChanges();

                    var newUser3 = new ApplicationUser()/*Jovanni.bianchi@PizzaItalia.com*/
                    { Companyname = "Pizza Italia", Forename = "Jovanni", Surname = "Bianchi", Street = "1 St Georges Street", Town = "Glasgow", Postcode = "G1 5AB", PhoneNumber = "07810197579", UserName = "30218926@cityofglacol.ac.uk", Email = "30218926@cityofglacol.ac.uk", EmailConfirmed = true, BacsApproved = true };
                    userManager.Create(newUser3, "Password#2");
                    userManager.AddToRole(newUser3.Id, RoleName.ROLE_USER);
                    userManager.SetLockoutEnabled(newUser3.Id, true);

                    ///1 booking with 1 invoice already both deposit and remaininging paid data set
                    var objBooking1 = new Booking()
                    { BookedDate = new DateTime(2017, 04, 30, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 5, 9, 30, 0), BookingEndDateTime = new DateTime(2017, 05, 5, 12, 30, 0), BookingStatus = BookingStatus.Visited, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBooking1.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).TotalMinutes / 60;
                    objBooking1.BookingDeposit = objBooking1.Subtotal * objDepositRate.DepositRateValue;
                    objBooking1.ItemDescription = objBooking1.BookingStartDateTime.Date.ToString("d")+ " From " + objBooking1.BookingStartDateTime.Hour + ":" + objBooking1.BookingStartDateTime.Minute+ " to " + objBooking1.BookingEndDateTime.Hour + ":" + objBooking1.BookingEndDateTime.Minute+ " " + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Hours + "hr" + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Minutes + "min";
                    objBooking1.VisitConfirmationSent = true;
                    context.Bookings.Add(objBooking1);

                    var objInvoice1 = new Invoice()
                    {InvoiceIssueDate = objBooking1.BookedDate, ReceivableRemainingAmount = 0f, ReceivableDepositAmount = objBooking1.Subtotal * objDepositRate.DepositRateValue, PayableAmount = 0f, PayableDepositAmount = 0f, Id = newUser3.Id};
                    objInvoice1.Bookings.Add(objBooking1);
                    objBooking1.InvoiceId = objInvoice1.InvoiceId;
                    context.Invoices.Add(objInvoice1);

                    objInvoice1.ReceivableRemainingAmount = objBooking1.Subtotal - objInvoice1.ReceivableDepositAmount;

                    var objPayment1 = new Payment()
                    { PaymentDateTime = objBooking1.BookedDate, CreditDepositAmount = objInvoice1.ReceivableDepositAmount, DebitDepositAmount = objInvoice1.PayableDepositAmount, InvoiceId = objInvoice1.InvoiceId, Id = newUser3.Id, PaymentType = "Deposit", TransactionId = "byfzff5m" };
                    context.Payments.Add(objPayment1);
                    context.SaveChanges();

                    var objPaymentHistory1 = new PaymentHistory()
                    { PaymentDateTime = objBooking1.BookingStartDateTime, CreditAmount = objPayment1.CreditDepositAmount, DebitAmount = objPayment1.DebitDepositAmount, PaymentId = objPayment1.PaymentId, InvoiceId = objInvoice1.InvoiceId, Id = newUser3.Id, PaymentType = PaymentType.Deposit, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistory1);
                    objPayment1.PaymentHistories.Add(objPaymentHistory1);
                    context.SaveChanges();

                    objPayment1.CreditRemainingAmount = objInvoice1.ReceivableRemainingAmount;
                    objPayment1.PaymentType = "Remaining";
                    context.SaveChanges();

                    var objPaymentHistory2 = new PaymentHistory()
                    { PaymentDateTime = objBooking1.BookingStartDateTime, CreditAmount = objPayment1.CreditRemainingAmount, DebitAmount = objPayment1.DebitRemainingAmount, PaymentId = objPayment1.PaymentId, InvoiceId = objInvoice1.InvoiceId, Id = newUser3.Id, PaymentType = PaymentType.Remaining, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistory2);
                    objPayment1.PaymentHistories.Add(objPaymentHistory2);
                    context.SaveChanges();

                    ///another set of 3bookings only deposit paid data set one of the day is in the past and visit status needs updated
                    var objBookingJB2 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 14, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 15, 15, 0, 0), BookingEndDateTime = new DateTime(2017, 05, 15, 16, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBookingJB2.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBookingJB2.BookingEndDateTime - objBookingJB2.BookingStartDateTime).TotalMinutes / 60;
                    objBookingJB2.BookingDeposit = objBookingJB2.Subtotal * objDepositRate.DepositRateValue;
                    objBookingJB2.ItemDescription = objBookingJB2.BookingStartDateTime.Date.ToString("d") + " From " + objBookingJB2.BookingStartDateTime.Hour + ":" + objBookingJB2.BookingStartDateTime.Minute + " to " + objBookingJB2.BookingEndDateTime.Hour + ":" + objBookingJB2.BookingEndDateTime.Minute + " " + (objBookingJB2.BookingEndDateTime - objBookingJB2.BookingStartDateTime).Hours + "hr" + (objBookingJB2.BookingEndDateTime - objBookingJB2.BookingStartDateTime).Minutes + "min";
                    context.Bookings.Add(objBookingJB2);

                    var objBookingJB3 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 14, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 26, 15, 0, 0), BookingEndDateTime = new DateTime(2017, 05, 26, 16, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBookingJB3.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBookingJB3.BookingEndDateTime - objBookingJB3.BookingStartDateTime).TotalMinutes / 60;
                    objBookingJB3.BookingDeposit = objBookingJB3.Subtotal * objDepositRate.DepositRateValue;
                    objBookingJB3.ItemDescription = objBookingJB3.BookingStartDateTime.Date.ToString("d") + " From " + objBookingJB3.BookingStartDateTime.Hour + ":" + objBookingJB3.BookingStartDateTime.Minute + " to " + objBookingJB3.BookingEndDateTime.Hour + ":" + objBookingJB3.BookingEndDateTime.Minute + " " + (objBookingJB3.BookingEndDateTime - objBookingJB3.BookingStartDateTime).Hours + "hr" + (objBookingJB3.BookingEndDateTime - objBookingJB3.BookingStartDateTime).Minutes + "min";
                    context.Bookings.Add(objBookingJB3);

                    var objBookingJB4 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 14, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 06, 02, 15, 0, 0), BookingEndDateTime = new DateTime(2017, 06, 02, 16, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBookingJB4.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBookingJB4.BookingEndDateTime - objBookingJB4.BookingStartDateTime).TotalMinutes / 60;
                    objBookingJB4.BookingDeposit = objBookingJB4.Subtotal * objDepositRate.DepositRateValue;
                    objBookingJB4.ItemDescription = objBookingJB4.BookingStartDateTime.Date.ToString("d") + " From " + objBookingJB4.BookingStartDateTime.Hour + ":" + objBookingJB4.BookingStartDateTime.Minute + " to " + objBookingJB4.BookingEndDateTime.Hour + ":" + objBookingJB4.BookingEndDateTime.Minute + " " + (objBookingJB4.BookingEndDateTime - objBookingJB4.BookingStartDateTime).Hours + "hr" + (objBookingJB4.BookingEndDateTime - objBookingJB4.BookingStartDateTime).Minutes + "min";
                    context.Bookings.Add(objBookingJB4);

                    var objInvoiceJB2 = new Invoice()
                    { InvoiceIssueDate = objBookingJB2.BookedDate, ReceivableRemainingAmount = 0f, ReceivableDepositAmount = (objBookingJB2.Subtotal + objBookingJB3.Subtotal + objBookingJB4.Subtotal) * objDepositRate.DepositRateValue, PayableAmount = 0f, PayableDepositAmount = 0f, Id = newUser3.Id };
                    objInvoiceJB2.Bookings.Add(objBookingJB2);
                    objInvoiceJB2.Bookings.Add(objBookingJB3);
                    objInvoiceJB2.Bookings.Add(objBookingJB4);
                    objBookingJB2.InvoiceId = objInvoiceJB2.InvoiceId;
                    objBookingJB3.InvoiceId = objInvoiceJB2.InvoiceId;
                    objBookingJB4.InvoiceId = objInvoiceJB2.InvoiceId;
                    context.Invoices.Add(objInvoiceJB2);

                    //remaining invoice not issued

                    var objPaymentJB2 = new Payment()
                    { PaymentDateTime = objBookingJB2.BookedDate, CreditDepositAmount = objInvoiceJB2.ReceivableDepositAmount, DebitDepositAmount = objInvoiceJB2.PayableDepositAmount, InvoiceId = objInvoiceJB2.InvoiceId, Id = newUser3.Id, PaymentType = "Deposit", TransactionId = "byfzff5m" };
                    context.Payments.Add(objPaymentJB2);
                    context.SaveChanges();
                    //remaining invoice payment not issued

                    var objPaymentHistoryJB2 = new PaymentHistory()
                    { PaymentDateTime = objBookingJB2.BookingStartDateTime, CreditAmount = objPaymentJB2.CreditDepositAmount, DebitAmount = objPaymentJB2.DebitDepositAmount, PaymentId = objPaymentJB2.PaymentId, InvoiceId = objInvoiceJB2.InvoiceId, Id = newUser3.Id, PaymentType = PaymentType.Deposit, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistoryJB2);
                    objPaymentJB2.PaymentHistories.Add(objPaymentHistoryJB2);
                    context.SaveChanges();

                    //Remaining Invoice's pay history not issued

                    ///another 1 booking with 1 invoice records only deposit paid it is past records visit status needs updated to issue invoice
                    var objBookingJB5 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 1, 15, 0, 0), BookingStartDateTime = new DateTime(2017, 05, 10, 13, 0, 0), BookingEndDateTime = new DateTime(2017, 05, 10, 17, 0, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBookingJB5.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBookingJB5.BookingEndDateTime - objBookingJB5.BookingStartDateTime).TotalMinutes / 60;
                    objBookingJB5.BookingDeposit = objBookingJB5.Subtotal * objDepositRate.DepositRateValue;
                    objBookingJB5.ItemDescription = objBookingJB5.BookingStartDateTime.Date.ToString("d") + " From " + objBookingJB5.BookingStartDateTime.Hour + ":" + objBookingJB5.BookingStartDateTime.Minute + " to " + objBookingJB5.BookingEndDateTime.Hour + ":" + objBookingJB5.BookingEndDateTime.Minute + " " + (objBookingJB5.BookingEndDateTime - objBookingJB5.BookingStartDateTime).Hours + "hr" + (objBookingJB5.BookingEndDateTime - objBookingJB5.BookingStartDateTime).Minutes + "min";
                    objBookingJB5.VisitConfirmationSent = true;
                    context.Bookings.Add(objBookingJB5);

                    var objBookingJB6 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 1, 10, 0, 0), BookingStartDateTime = new DateTime(2017, 05, 16, 13, 0, 0), BookingEndDateTime = new DateTime(2017, 05, 16, 17, 0, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser3.Id, UnitPriceId = objUnitPrice1.UnitPriceId, };
                    objBookingJB6.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBookingJB6.BookingEndDateTime - objBookingJB6.BookingStartDateTime).TotalMinutes / 60;
                    objBookingJB6.BookingDeposit = objBookingJB6.Subtotal * objDepositRate.DepositRateValue;
                    objBookingJB6.ItemDescription = objBookingJB6.BookingStartDateTime.Date.ToString("d") + " From " + objBookingJB6.BookingStartDateTime.Hour + ":" + objBookingJB6.BookingStartDateTime.Minute + " to " + objBookingJB6.BookingEndDateTime.Hour + ":" + objBookingJB6.BookingEndDateTime.Minute + " " + (objBookingJB6.BookingEndDateTime - objBookingJB6.BookingStartDateTime).Hours + "hr" + (objBookingJB6.BookingEndDateTime - objBookingJB6.BookingStartDateTime).Minutes + "min";
                    objBookingJB6.VisitConfirmationSent = true;
                    context.Bookings.Add(objBookingJB6);

                    var objInvoiceJB3 = new Invoice()
                    { InvoiceIssueDate = objBookingJB5.BookedDate, ReceivableRemainingAmount = 0f, ReceivableDepositAmount = (objBookingJB5.Subtotal+ objBookingJB6.Subtotal) * objDepositRate.DepositRateValue, PayableAmount = 0f, PayableDepositAmount = 0f, Id = newUser3.Id };
                    objInvoiceJB3.Bookings.Add(objBookingJB5);
                    objInvoiceJB3.Bookings.Add(objBookingJB6);
                    objBookingJB5.InvoiceId = objInvoiceJB3.InvoiceId;
                    objBookingJB6.InvoiceId = objInvoiceJB3.InvoiceId;
                    context.Invoices.Add(objInvoiceJB3);
                    

                    //remaining invoice not issued

                    var objPaymentJB3 = new Payment()
                    { PaymentDateTime = objBookingJB5.BookedDate, CreditDepositAmount = objInvoiceJB3.ReceivableDepositAmount, DebitDepositAmount = objInvoiceJB3.PayableDepositAmount, InvoiceId = objInvoiceJB3.InvoiceId, Id = newUser3.Id, PaymentType = "Deposit", TransactionId = "byfzff5m" };
                    context.Payments.Add(objPaymentJB3);
                    context.SaveChanges();

                    //remaining invoice payment not issued

                    var objPaymentHistoryJB3 = new PaymentHistory()
                    { PaymentDateTime = objBookingJB5.BookingStartDateTime, CreditAmount = objPaymentJB3.CreditDepositAmount, DebitAmount = objPaymentJB3.DebitDepositAmount, PaymentId = objPaymentJB3.PaymentId, InvoiceId = objInvoiceJB3.InvoiceId, Id = newUser3.Id, PaymentType = PaymentType.Deposit, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistoryJB3);
                    objPaymentJB3.PaymentHistories.Add(objPaymentHistoryJB3);
                    context.SaveChanges();

                    //Remaining Invoice's payment history notissued
                    var newUser4 = new ApplicationUser() /*JoannaGoodie @AppleCafe.com*/
                    { Companyname = "Apple Cafe", Forename = "Joanna", Surname = "Goodie", Street = "32 Angus Road", Town = "Glasgow", Postcode = "G2 5CC", PhoneNumber = "07810197579", UserName = "JoannaGoodie@AppleCafe.com", Email = "JoannaGoodie@AppleCafe.com", EmailConfirmed = true, BacsApproved = true };
                    userManager.Create(newUser4, "Password#2");
                    userManager.AddToRole(newUser4.Id, RoleName.ROLE_USER);
                    userManager.SetLockoutEnabled(newUser4.Id, true);
                    context.SaveChanges();

                    var objBooking2 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 10, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 11, 9, 30, 0), BookingEndDateTime = new DateTime(2017, 05, 11, 12, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser4.Id, UnitPriceId = 1, };
                    //objBooking2.BookingDuration = objBooking2.BookingEndDateTime - objBooking2.BookingStartDateTime;
                    objBooking2.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking2.BookingEndDateTime - objBooking2.BookingStartDateTime).TotalMinutes / 60;
                    objBooking2.BookingDeposit = objBooking2.Subtotal * objDepositRate.DepositRateValue;
                    objBooking2.ItemDescription = objBooking2.BookingStartDateTime.Date.ToString("d") + " From " + objBooking2.BookingStartDateTime.Hour + ":" + objBooking2.BookingStartDateTime.Minute + " to " + objBooking2.BookingEndDateTime.Hour + ":" + objBooking2.BookingEndDateTime.Minute + " " + (objBooking2.BookingEndDateTime - objBooking2.BookingStartDateTime).Hours + "hr" + (objBooking2.BookingEndDateTime - objBooking2.BookingStartDateTime).Minutes + "min";
                    objBooking2.VisitConfirmationSent = true;
                    context.Bookings.Add(objBooking2);
                    //mark as visited
                    objBooking2.BookingStatus = BookingStatus.Visited;
                    context.SaveChanges();

                    var objInvoice3 = new Invoice()
                    { InvoiceIssueDate = objBooking2.BookedDate, ReceivableRemainingAmount = 0f, ReceivableDepositAmount = objBooking2.Subtotal * objDepositRate.DepositRateValue, PayableAmount = 0f, PayableDepositAmount = 0f, Id = newUser4.Id/*,.ReceivableDepositAmount = objBookingInvoiceLine1.Subtotal * objDepositRate.DepositRateValue,*/};
                    objInvoice3.Bookings.Add(objBooking2);
                    objBooking2.InvoiceId = objInvoice3.InvoiceId;
                    context.Invoices.Add(objInvoice3);
                    //issue remaining invoice
                    objInvoice3.ReceivableRemainingAmount = objBooking2.Subtotal - objInvoice3.ReceivableDepositAmount;
                    context.SaveChanges();

                    //Remaining not paid
                    //objInvoice3.ReceivableRemainingAmount = objBooking2.Subtotal - objInvoice3.ReceivableDepositAmount;
                    //objBooking2.BookingStatus = BookingStatus.Visited;
                    //context.SaveChanges();
                    
                    var objPayment3 = new Payment()
                    { PaymentDateTime = objBooking2.BookedDate, CreditDepositAmount = objInvoice3.ReceivableDepositAmount, DebitDepositAmount = 0f, InvoiceId = objInvoice3.InvoiceId, Id = newUser4.Id, PaymentType = "Deposit", TransactionId = "8mh62qqc" };
                    context.Payments.Add(objPayment3);
                    context.SaveChanges();

                    var objPaymentHistory3 = new PaymentHistory()
                    { PaymentDateTime = objBooking2.BookingStartDateTime, CreditAmount = objPayment3.CreditDepositAmount, DebitAmount = objPayment3.DebitDepositAmount, PaymentId = objPayment3.PaymentId, InvoiceId = objInvoice3.InvoiceId, Id = newUser2.Id, PaymentType = PaymentType.Deposit, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistory3);
                    context.SaveChanges();

                    //Remaining not paied
                    //objPayment3.CreditRemainingAmount = objInvoice3.ReceivableRemainingAmount;
                    //context.SaveChanges();

                    var objBooking3 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 1, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 27, 9, 30, 0), BookingEndDateTime = new DateTime(2017, 05, 27, 12, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser4.Id, UnitPriceId = 1, };
                    objBooking3.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking3.BookingEndDateTime - objBooking3.BookingStartDateTime).TotalMinutes / 60;
                    objBooking3.BookingDeposit = objBooking3.Subtotal * objDepositRate.DepositRateValue;
                    objBooking3.ItemDescription = objBooking3.BookingStartDateTime.Date.ToString("d") + " From " + objBooking3.BookingStartDateTime.Hour + ":" + objBooking3.BookingStartDateTime.Minute + " to " + objBooking3.BookingEndDateTime.Hour + ":" + objBooking3.BookingEndDateTime.Minute + " " + (objBooking3.BookingEndDateTime - objBooking3.BookingStartDateTime).Hours + "hr" + (objBooking3.BookingEndDateTime - objBooking3.BookingStartDateTime).Minutes + "min";
                    context.Bookings.Add(objBooking3);

                    var objBooking4 = new Booking()
                    { BookedDate = new DateTime(2017, 05, 1, 9, 30, 0), BookingStartDateTime = new DateTime(2017, 05, 26, 9, 30, 0), BookingEndDateTime = new DateTime(2017, 05, 26, 10, 30, 0), BookingStatus = BookingStatus.Booked, BookingAdjustmentHrs = 0f, Id = newUser4.Id, UnitPriceId = 1, };
                    objBooking4.Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking4.BookingEndDateTime - objBooking4.BookingStartDateTime).TotalMinutes / 60;
                    objBooking4.BookingDeposit = objBooking4.Subtotal * objDepositRate.DepositRateValue;
                    objBooking4.ItemDescription = objBooking4.BookingStartDateTime.Date.ToString("d") + " From " + objBooking4.BookingStartDateTime.Hour + ":" + objBooking4.BookingStartDateTime.Minute + " to " + objBooking4.BookingEndDateTime.Hour + ":" + objBooking4.BookingEndDateTime.Minute + " " + (objBooking4.BookingEndDateTime - objBooking4.BookingStartDateTime).Hours + "hr" + (objBooking4.BookingEndDateTime - objBooking4.BookingStartDateTime).Minutes + "min";
                    context.Bookings.Add(objBooking4);

                    var objInvoice5 = new Invoice()
                    { InvoiceIssueDate = objBooking3.BookedDate, ReceivableRemainingAmount = 0f, ReceivableDepositAmount = objBooking3.Subtotal * objDepositRate.DepositRateValue + objBooking4.Subtotal * objDepositRate.DepositRateValue, PayableAmount = 0f, PayableDepositAmount = 0f, Id = newUser4.Id/*,ReceivableDepositAmount = objBookingInvoiceLine1.Subtotal * objDepositRate.DepositRateValue,*/};
                    objInvoice5.Bookings.Add(objBooking3);
                    objInvoice5.Bookings.Add(objBooking4);
                    objBooking3.InvoiceId = objInvoice5.InvoiceId;
                    objBooking4.InvoiceId = objInvoice5.InvoiceId;
                    context.Invoices.Add(objInvoice5);

                    var objPayment5 = new Payment()
                    { PaymentDateTime = objBooking3.BookedDate, CreditDepositAmount = objInvoice5.ReceivableRemainingAmount + objInvoice5.ReceivableDepositAmount, DebitDepositAmount = objInvoice5.PayableAmount + objInvoice5.PayableDepositAmount, InvoiceId = objInvoice5.InvoiceId, Id = newUser4.Id, PaymentType = "Deposit", TransactionId = "byfzff5m" };
                    context.Payments.Add(objPayment5);

                    var objPaymentHistory5 = new PaymentHistory()
                    { PaymentDateTime = objBooking1.BookingStartDateTime, CreditAmount = objPayment5.CreditDepositAmount, DebitAmount = objPayment5.DebitDepositAmount, PaymentId = objPayment5.PaymentId, InvoiceId = objInvoice5.InvoiceId, Id = newUser4.Id, PaymentType = PaymentType.Deposit, TransactionId = "byfzff5m" };
                    context.PaymentHistories.Add(objPaymentHistory5);
                    context.SaveChanges();

                }
            }
            base.Seed(context);
            context.SaveChanges();
        }
    }
}
