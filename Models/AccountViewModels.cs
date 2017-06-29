using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaAccounting.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

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
        [Required(ErrorMessage = "Phone Number Required")]
        [Display(Name ="Phone Number")]
        [RegularExpression(@"^.{11,}$", ErrorMessage = "PhoneNumber Minimum 11 characters required")]
        public string PhoneNumber { get; set; }
        public bool BacsApproved { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passowrd is required")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long. inculding a number, a upper letter, a lower letter, a symbol", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Passowrd is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
