using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AlfaAccounting.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using AlfaAccounting;
using Twilio;
using static ApplicationSignInManager;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace AlfaAccounting
{

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
            // return Task.FromResult(0);
        }

        private async Task configSendGridasync(IdentityMessage message)
        { //  var myMessage = new SendGrid.SendGridMessage();
            //   var apiKey = Environment.GetEnvironmentVariable("mailapi");
            var apiKey = ConfigurationManager.AppSettings["mailApi"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Alfa Accounting");
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
//var response = await client.SendEmailAsync(msg);

//  var credentials = new NetworkCredential(
//  ConfigurationManager.AppSettings["mailAccount"],
//  ConfigurationManager.AppSettings["mailPassword"]
//  );

////  Create a Web transport for sending email.

// var transportWeb = new Web(credentials);

//  // Send the email.
//  if (transportWeb != null)
//  {
//      await transportWeb.DeliverAsync(myMessage);
//  }
//  else
//  {
//      Trace.TraceError("Failed to create Web transport.");
//      await Task.FromResult(0);
//  }
//       }
// }

public class SmsService : IIdentityMessageService
{
    public Task SendAsync(IdentityMessage message)
    {
        // Twilio Begin
        var Twilio = new TwilioRestClient(
          System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
          System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"]);
        var result = Twilio.SendMessage(
          System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
          message.Destination, message.Body
        );
        //    Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
        Trace.TraceInformation(result.Status);
        //    Twilio doesn't currently have an async API, so return success.
        return Task.FromResult(0);
        // Twilio End
        //return Task.FromResult(0);
    }
}

// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
        // Configure validation logic for usernames
        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
        {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
        };

        // Configure validation logic for passwords
        manager.PasswordValidator = new PasswordValidator
        {
            RequiredLength = 6,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
        };

        // Configure user lockout defaults
        manager.UserLockoutEnabledByDefault = true;
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        // You can write your own provider and plug it in here.
        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
        {
            MessageFormat = "Your security code is {0}"
        });
        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
        {
            Subject = "Security Code",
            BodyFormat = "Your security code is {0}"
        });

        manager.EmailService = new EmailService();
        manager.SmsService = new SmsService();

        var dataProtectionProvider = options.DataProtectionProvider;
        if (dataProtectionProvider != null)
        {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }
        return manager;
    }

}

// Configure the application sign-in manager which is used in this application.
public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
{
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
    {
    }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
        return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    }

    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    {
        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }

    // Add ApplicationRoleManager to allow the management of Roles
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        {
        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }


    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check if session is supported
            if (ctx.Session["ID"] != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
                        string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                        string loginUrl = FormsAuthentication.LoginUrl + redirectUrl;
                        if (ctx.Request.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        RedirectResult rr = new RedirectResult(loginUrl);
                        filterContext.Result = rr;
                        //ctx.Response.Redirect("~/Home/Logon");
                        //filterContext.Result = new RedirectResult("~/Account/Login");
                        //return;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
