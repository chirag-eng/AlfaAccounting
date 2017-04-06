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
        [Display(Name = "Post code")]
        public string Town { get; set; }
        [Display(Name = "Post code")]
        [Required(ErrorMessage = "Postcode required")]
        public string Postcode { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
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