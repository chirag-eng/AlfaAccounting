using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlfaAccounting.Models
{
        public class BookdatesViewModel
        {
            [DataType(DataType.Date)]
            public DateTime BookingDate { get; set; }
            public DateTime? BookingStartDateTime { get; set; }
            public DateTime? BookingEndDateTime { get; set; }
            public string BookingStatus { get; set; }
            [DisplayName("User")]
            [ForeignKey("ApplicationUser")]
            public string Id { get; set; }
            public virtual ApplicationUser ApplicationUser { get; set; }
//        [ForeignKey("Invoice")]
//        public int? InvoiceId { get; set; }
//        public virtual Invoice Invoice { get; set; }
        //        public TimeSpan BookingDuration { get; set; }


    }

        public class ConfirmBookingViewModel
        {

            [Key]
            public int InvoiceId { get; set; }
            public string InvoiceReference { get; set; }
            public DateTime InvoiceIssueDate { get; set; }
        [Display(Name = "Payable")]
        [DataType(DataType.Currency)]
        public float ReceivableAmount { get; set; }
        [Display(Name = "Deposit Payable")]
        [DataType(DataType.Currency)]
            public float ReceivableDepoisitAmount { get; set; }
        [Display(Name = "In credit")]
        [DataType(DataType.Currency)]
        public float PayableAmount { get; set; }
        [Display(Name = "Deposit in credit")]
        [DataType(DataType.Currency)]
        public float PayableDepositAmount { get; set; }
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
            public ICollection<Booking> Bookings { get; set; }

        }

 /*       public class ProceedPaymentViewModel
        {
            [Key]
            public int PaymentId { get; set; }
            [DataType(DataType.Date)]
            public DateTime PaymentDate { get; set; }
            public float CreditAmount { get; set; }
            public float DebitAmount { get; set; }
            public string PaymentMethod { get; set; }
            public virtual ApplicationUser ApplicationUser { get; set; }

        }
    */
}