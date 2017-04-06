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
 //       public DbSet<BookingInvoiceLine> BookingInvoiceLines { get; set; }

        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



 //       public System.Data.Entity.DbSet<AlfaAccounting.Models.ConfirmBookingViewModel> ConfirmBookingViewModels { get; set; }

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


    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext> {

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

                categories.Add(new Category { CategoryName = "Play" });
                categories.Add(new Category { CategoryName = "Comedy" });
                categories.Add(new Category { CategoryName = "Gig" });
                foreach (Category ctgy in categories) context.Categories.Add(ctgy);
                context.SaveChanges();



                string userName1 = "admin@admin2.com";
                string password1 = "Password#2";
                // var passworddHash = new PasswordHasher(); if migration enable methods this code needs used
                //password = passwordHash.HashPassword(password);
                // Create Admin user and role

                var user = userManager.FindByName(userName1);
                if (user == null)
                {
                    var newUser = new ApplicationUser()
                    { Companyname = "Alfa Accounting",
                        Forename = "Administrator",
                        Surname = "Admin",
                        Street = "190 Catheral Street",
                        Town = "Glasgow",
                        Postcode = "2G1 3DF",
                        PhoneNumber = "0141  43455",
                        UserName = userName1,
                        Email = userName1,
                        EmailConfirmed = true,
                    };
                    userManager.Create(newUser, password1);
                    userManager.AddToRole(newUser.Id, RoleName.ROLE_ADMINSTRATOR);
                    var objBlog1 = new Blog()
                    { BlogDate = Convert.ToDateTime("27/10/2016"),
                        BlogTitle = "The Full Monty Overview",
                        BlogContent = "The classic film about six out of work steel workers with nothing to lose took the world by storm! Based on his smash hit film and adapted for the stage by Oscar-winning writer Simon Beaufoy, this hilarious and heartfelt production stars Gary Lucy, Andrew Dunn, Louis Emerick, Chris Fountain, Anthony Lewis, Kai Owen and a cast of fourteen and is directed by Jack Ryder. Not only has the play been getting standing ovations every night, but it also won the prestigious UK Theatre Award for best touring production. Featuring great songs by Donna Summer, Hot Chocolate and Tom Jones you really should...drop absolutely everything and book today!",
                        BlogApproved = true,
                        CategoryId = 1, Id = newUser.Id };
                    var objCommnet1 = new Comment()
                    { CommentedDate = Convert.ToDateTime("15/01/2017"),
                        CommentTitle = "good show",
                        CommentBody = " must go, but there was not car park near the theater give good 40 min before arriving the theater",
                        BlogId = 1, Id = newUser.Id };
                    context.Blogs.Add(objBlog1);
                    context.Comments.Add(objCommnet1);
                    //userManager.Create(newUser, password);
                    //userManager.AddToRole(newUser,Role);

                    //userManager.SetLockourtEnabled(newUser.Id,false)

                var newUser2 = new ApplicationUser()
                {
                    Companyname = "Design Space",
                    Forename = "Mie",
                    Surname = "Tanaka",
                    Street = "1 Glasgow Street",
                    Town = "Glasgow",
                    Postcode = "G1 5AA",
                    PhoneNumber = "014112345",
                    UserName = "mietta25@gmail.com",
                    Email = "mietta25@gmail.com",
                    EmailConfirmed = true,
                };
                userManager.Create(newUser2, "Password#2");
                userManager.AddToRole(newUser2.Id, RoleName.ROLE_STAFF);

                var objBlog2 = new Blog() { BlogDate = Convert.ToDateTime("20/11/2016"),
                    BlogTitle = "Frankie Boyle and Friends Overview",
                    BlogContent = "Fresh from hosting his critically-acclaimed American Autopsy for the BBC, join Frankie Boyle and some of his favourite comics for a run of shows in his native Glasgow as part of the Glasgow Live International Comedy Festival. Expect ‘black-hearted brilliance’ The Guardian from one of UK comedy’s fiercest talents, as well as a selection of the brightest and best comedians currently working on the circuit. Thursday night’s show proceeds go to the charity Help Refugees UK. Latecomers may not be admitted. There will be no re-admittance during the second half of the show.",
                    BlogApproved = true,
                    CategoryId = 2, Id = newUser2.Id };
                context.Blogs.Add(objBlog2);
                var objBlog3 = new Blog()
                {
                    BlogDate = Convert.ToDateTime("20/02/2017"),
                    BlogTitle = "Roy Wood & his Band",
                    BlogContent = "Featuring Roy Wood & his Rock ‘N Roll band performing all his classic hits, I Can Hear The Grass Grow, Flowers In The Rain, Blackberry Way, California Man, See My Baby Jive, I Wish It Could Be Christmas Everyday….and much more.Born in Birmingham, His first instrument was drums, which is the only instrument he has ever had any tuition on! Influenced by Hank Marvin's sound, he took up playing guitar, and formed a group called The Falcons at the age of forteen In 1970, Roy teamed up with fellow Birmingham songwriter Jeff Lynne, who joined The Move for their final two albums. The final single recorded by The Move during this period was California Man. Roy had an ambition over a number of years, to form a classically based band featuring live strings instead of the conventional guitar line up. Together with Jeff, they formed The Electric Light Orchestra. Support Act: Nik Lowe, an acoustic soul/pop/rock/blues/crooner like no other. It’s hard to put Nik Lowe into a box labelled with a particular genre of music becase Nik's song-writing style covers most genres. One minute his music is rocky and raw, the next smooth and schmaltzy, then on to pure retro-pop - and all with a voice to match. Don’t miss this exclusive show!",
                    BlogApproved = false,
                    CategoryId = 3,
                    Id = newUser2.Id
                };
                context.Blogs.Add(objBlog3);
                var objComment2 = new Comment()
                { CommentedDate = Convert.ToDateTime("26/01/2017"),
                    CommentTitle = "so so",
                    CommentBody = " it was boring i wish i stayed at home, i should have booked comedy show instead",
                    BlogId = 3, Id = newUser2.Id };
                context.Comments.Add(objComment2);
                var objComment3 = new Comment()
                { CommentedDate = Convert.ToDateTime("27/01/2017"),
                    CommentTitle = "really goog",
                    CommentBody = " I had a  black joke giggles the whole time, he could use less swearing words",
                    BlogId = 2, Id = newUser2.Id
                };
                context.Comments.Add(objComment2);

                    //populating Unit Price table
                    List<UnitPrice> UnitPrices = new List<UnitPrice>();
                    UnitPrice objUnitPrice1 = new UnitPrice() { UnitPriceId = 1, UnitPriceValue = 100, UnitPriceDescription = "Standard" };
                    UnitPrices.Add(objUnitPrice1);
                    UnitPrice objUnitPrice2 = new UnitPrice() { UnitPriceId = 2, UnitPriceValue = 90, UnitPriceDescription = "Discount90" };
                    UnitPrices.Add(objUnitPrice2);
                    UnitPrice objUnitPrice3 = new UnitPrice() { UnitPriceId = 3, UnitPriceValue = 80, UnitPriceDescription = "Discount80" };
                    UnitPrices.Add(objUnitPrice3);
                    UnitPrice objUnitPrice4 = new UnitPrice() { UnitPriceId = 4, UnitPriceValue = 70, UnitPriceDescription = "Discount70" };
                    UnitPrices.Add(objUnitPrice4);

                    context.SaveChanges();

                    var newUser3 = new ApplicationUser()
                {
                    Companyname = "Pizza Italia",
                    Forename = "Jovanni",
                    Surname = "Bianchi",
                    Street = "1 St Georges Street",
                    Town = "Glasgow",
                    Postcode = "G1 5AB",
                    PhoneNumber = "01413334444",
                    UserName = "Jovanni.bianchi@PizzaItalia.com",
                    Email = "Jovanni.bianchi@PizzaItalia.com",
                    EmailConfirmed = true,
                };
                userManager.Create(newUser3, "Password#2");
                userManager.AddToRole(newUser3.Id, RoleName.ROLE_STAFF);

                var objBooking1 = new Booking()
                {

                    BookedDate = DateTime.Now,
                    BookingStartDateTime = new DateTime(2017, 03, 29, 9, 30, 0),
                    BookingEndDateTime = new DateTime(2017, 03, 29, 12, 30, 0),
                    BookingStatus = "Booked",
                    BookingAdjustmentHrs = 0f,
                    ApplicationUser = newUser3,
                    Id = newUser3.Id,
                    UnitPriceId = objUnitPrice1.UnitPriceId,
                            
                };
                    //var objBookingInvoiceLine1 = new BookingInvoiceLine()
                    //{
                    //    BookingAdjustmentHrs = 0f,
                    //    UnitPriceId = objUnitPrice1.UnitPriceId,
                    //    BookingDuration = objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime,
                    //    Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).TotalMinutes / 60,
                    //    ItemDescription = objBooking1.BookingStartDateTime.Date.ToString("d")
                    //+ " From " + objBooking1.BookingStartDateTime.Hour + ":" + objBooking1.BookingStartDateTime.Minute
                    //+ " to " + objBooking1.BookingEndDateTime.Minute + ":" + objBooking1.BookingEndDateTime.Minute
                    //+ " " + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Hours + "hr" + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Minutes + "min",
                    //BookingId = objBooking1.BookingId
                    //};

                    objBooking1.BookingDuration = objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime;
                    objBooking1.UnitPrice = objUnitPrice1;
                    objBooking1.Subtotal = objBooking1.UnitPrice.UnitPriceValue * (float)objBooking1.BookingDuration.TotalMinutes / 60;
                    objBooking1.ItemDescription = objBooking1.BookingStartDateTime.Date.ToString("d")
                            + " From " + objBooking1.BookingStartDateTime.Hour + ":" + objBooking1.BookingStartDateTime.Minute
                            + " to " + objBooking1.BookingEndDateTime.Minute + ":" + objBooking1.BookingEndDateTime.Minute
                            + objBooking1.BookingDuration.Hours + "hr" + objBooking1.BookingDuration.Minutes + "min";
                    context.Bookings.Add(objBooking1);
 //                   context.BookingInvoiceLines.Add(objBookingInvoiceLine1);

                    var objInvoice1 = new Invoice()
                    {
//                        InvoiceId = 1,
                    InvoiceReference = "Deposit",
                    InvoiceIssueDate = objBooking1.BookedDate,
                    ReceivableAmount = 0f,
                    ReceivableDepoisitAmount = objBooking1.Subtotal * 0.1f,
//                    ReceivableDepoisitAmount = objBookingInvoiceLine1.Subtotal * 0.1f,
                    PayableAmount = 0f,
                    PayableDepositAmount = 0f,
                    Id = newUser3.Id,
                    };

                     objInvoice1.Bookings.Add(objBooking1);
                     objBooking1.Invoices.Add(objInvoice1);
                    //                    objBookingInvoiceLine1.InvoiceId = objInvoice1.InvoiceId;
                    //                    objBooking1.Invoice = objInvoice1;
                    //                    objBooking1.InvoiceId = objInvoice1.InvoiceId;
                    context.Invoices.Add(objInvoice1);

                    var objInvoice2 = new Invoice()
                    {
 //                       InvoiceId = 2,
                        InvoiceReference = "Remaining",
                        InvoiceIssueDate = objBooking1.BookedDate,
                        ReceivableAmount = objBooking1.Subtotal*0.9f,
//                        ReceivableAmount = objBookingInvoiceLine1.Subtotal * 0.9f,
                        ReceivableDepoisitAmount = 0f,
                        PayableAmount = 0f,
                        PayableDepositAmount = 0f,
//                        ApplicationUser = newUser3,
                        Id = newUser3.Id,
                    };
 //                   var objBookingInvoiceLine2 = new BookingInvoiceLine()
 //                   {
 //                       BookingAdjustmentHrs = 0f,
 //                       UnitPriceId = objUnitPrice1.UnitPriceId,
 //                       BookingDuration = objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime,
 //                       Subtotal = objUnitPrice1.UnitPriceValue * (float)(objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).TotalMinutes / 60,
 //                       ItemDescription = objBooking1.BookingStartDateTime.Date.ToString("d")
 //+ " From " + objBooking1.BookingStartDateTime.Hour + ":" + objBooking1.BookingStartDateTime.Minute
 //+ " to " + objBooking1.BookingEndDateTime.Minute + ":" + objBooking1.BookingEndDateTime.Minute
 //+ " " + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Hours + "hr" + (objBooking1.BookingEndDateTime - objBooking1.BookingStartDateTime).Minutes + "min",
 //                       BookingId = objBooking1.BookingId
 //                   };

                    objInvoice2.Bookings.Add(objBooking1);
                    objBooking1.Invoices.Add(objInvoice2);
                    //                    objBookingInvoiceLine2.InvoiceId = objInvoice2.InvoiceId;
                    //                    objBooking1.Invoice = objInvoice1;
                    //                   objBooking1.InvoiceId = objInvoice1.InvoiceId;
                    context.Invoices.Add(objInvoice2);

                    var objPayment1 = new Payment()
                {
                    PaymentDateTime = objBooking1.BookedDate,
                    CreditAmount = objInvoice1.ReceivableAmount + objInvoice1.ReceivableDepoisitAmount,
                    DebitAmount = objInvoice1.PayableAmount + objInvoice1.PayableDepositAmount,
                    InvoiceId = objInvoice1.InvoiceId,
                        ApplicationUser = newUser3,
                        Id = newUser3.Id,
                    };
                    objPayment1.Invoice = objInvoice1;
                context.Payments.Add(objPayment1);

                    var objPayment2 = new Payment()
                    {
                        PaymentDateTime = objBooking1.BookingStartDateTime,
                        CreditAmount = objInvoice2.ReceivableAmount + objInvoice2.ReceivableDepoisitAmount,
                        DebitAmount = objInvoice2.PayableAmount + objInvoice2.PayableDepositAmount,
                        InvoiceId = objInvoice2.InvoiceId,
                        Id = newUser3.Id
                    };
                    objPayment2.Invoice = objInvoice2;
                    context.Payments.Add(objPayment2);

            }
            }
            base.Seed(context);
            context.SaveChanges();
        }
    }
}
