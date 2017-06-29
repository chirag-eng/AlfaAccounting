using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlfaAccounting.Models
{
    public class ExpandedUserDTO
    {
        [Required(ErrorMessage = "Companyname required")]
        public string Companyname { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        [Required(ErrorMessage = "Street name required")]
        [Display(Name = "Stree name")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Town/City required")]
        [Display(Name = "Town/City")]
        public string Town { get; set; }
        [Display(Name = "Post code")]
        [Required(ErrorMessage = "Postcode required")]
        [StringLength(12, ErrorMessage = "The {0} must be at least {2}, maximum 12 characters long.", MinimumLength = 2)]
        public string Postcode { get; set; }

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
        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        public bool BacsApproved { get; set; }
        public IEnumerable<UserRolesDTO> Roles { get; set; }

    }

    public class UserRolesDTO
    {
        [Key]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class UserRoleDTO
    {
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class RoleDTO
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class UserAndRolesDTO
    {
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public List<UserRoleDTO> colUserRoleDTO { get; set; }
    }
}