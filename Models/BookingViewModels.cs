using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AlfaAccounting.Models
{
    public class AddBookingViewModel
    {
        public string BookingStartDateTime { get; set; }
        public string BookingEndDateTime { get; set; }
    }
    //public class AmendBookdatesViewModel
    //{
    //    public int BookingId { get; set; }
    //    [DataType(DataType.Date)]
    //    public DateTime BookingDate { get; set; }
    //    [Display(Name = "Start Date Time")]
    //    public DateTime? BookingStartDateTime { get; set; }
    //    [Display(Name = "End Date Time")]
    //    public DateTime? BookingEndDateTime { get; set; }
    //    [Display(Name = "Booking Status")]
    //    public string BookingStatus { get; set; }
    //    [Display(Name = "Adjustment Hrs i.e. 1.5, 1.25, 1.75 etc")]
    //    public float BookingAdjustmentHrs { get; set; }
    //    public float BookingCancellationFee { get; set; }

    //}



    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public float DepositTotal { get; set; }
        public float CartTotal { get; set; }
    }

    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public float DepositTotal { get; set; }
        public float CartTotal { get; set; }
        public int SCartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }


    public class AdminBookingsViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; }   
    }
    public class MyBookingsViewModelBase: InvoiceIndexViewModel
    {   
           public DateTime? PaymentDateTime { get; set; }
        [Display(Name = "Paid Amout")]
        [DataType(DataType.Currency)]
        public float PaidAmount { get; set; }
        [Display(Name = "Payment Type")]
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
        public float BookingCancellationFee { get; set; }
    }

    public class MyBookingCancellation
    {
        public int BookingId { get; set; }
        public DateTime BookedDate{ get; set; }
        public DateTime BookingStartDate { get; set; }
        public float BookingCancellationFee { get; set; }
        [Display(Name = "Booking Detail")]
        public string ItemDescription { get; set; }
        public float Subtotal { get; set; }
        public float BookingDeposit { get; set; }
        public string BookingStatus { get; set; }
        public float UnitPriceValue { get; set; }
        public string TransactionId { get; set; }
        //public ICollection<Invoice> Invoices { get; set; } b-i many to many
        public virtual Invoice Invoice { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class ConfirmBookingViewModel : InvoiceIndexViewModel
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
 
    public class InvoiceIndexViewModel
    {   [Display(Name ="InvId")]
        public int InvoiceId { get; set; }
        [Display(Name = "BookingId")]
        public int BookingId { get; set; }
        [Display(Name = "Booking Detail")]
        public string ItemDescription { get; set; }
        public DateTime BookingStartDateTime { get; set; }
        [Display(Name = "Booking Status")]
        public string BookingStatus { get; set; }
        [Display(Name ="Company Name")]
        public string Companyname { get; set; }
        [Display(Name = "Subtotal")]
        public float Subtotal { get; set; }
        [Display(Name = "Invoiced Date")]
        public DateTime InvoiceIssueDate { get; set; }
        [Display(Name = "Invoiced Deposit")]
        [DataType(DataType.Currency)]
        public float InvoicedDeposit { get; set; }
        [Display(Name = "Invoiced Remaining")]
        [DataType(DataType.Currency)]
        public float InvoicedRemaining { get; set; }
        public float ReceivableRemainingAmount { get; set; }
        public float ReceivableDepositAmount { get; set; }
        public float PayableAmount { get; set; }
        public float PayableDepositAmount { get; set; }
        public float BookingDeposit { get; set; }
        public float InvoiceCancellationFee { get; set; }
        public string Id { get; set; }
        public bool Statuscheck { get; set; }

    }

    public class InvoiceDetailViewModel : InvoiceIndexViewModel
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }

    public class VisitRefundViewModel /*: InvoiceIndexViewModel*/
    {
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        [Display(Name="Refund Amount")]
        public float RefundAmount { get; set; }
        public float AdditionalPaymentAmount { get; set; }
    }

    public class PaymentIndexViewModel:InvoiceIndexViewModel
    {
        [Display(Name = "PaymentId")]
        public int PaymentId { get; set; }
        [Display(Name = "Payment Date/Time")]
        public DateTime? PaymentDateTime { get; set; }
        [Display(Name = "Payment Amout")]
        [DataType(DataType.Currency)]
        public float PaidAmount { get; set; }
        [Display(Name = "Payment Type")]
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

    }

    public class PaymentReceiptViewModel 
    {
        public int BookingId { get; set; }
        public int InvoiceId { get; set; }
        public DateTime InvoiceIssueDate { get; set; }
        public float InvoiceCancellationFee { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public DateTime? PaymentDateTime { get; set; }
        public float ReceivableDepositAmount { get; set; }
        public float ReceivableRemainingAmount { get; set; }
        public float PayableDepositAmount { get; set; }
        public float PayableAmount { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }

    public class AmendBookDatesViewModel
    {
        public int BookingId { get; set; }
        public DateTime BookedDate { get; set; }
        [DataType(DataType.DateTime)]
        public string BookingStartDateTime { get; set; }
        [DataType(DataType.DateTime)]
        public string BookingEndDateTime { get; set; }
        public int UnitPriceId { get; set; }
        public int InvoiceId { get; set; }
    }

    public class PaymentHistoryViewModel
    {
        public int PaymentHistoryId { get; set; }
        [Required(ErrorMessage = "Payment Date required")]
        //[DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Paid Date")]
        //[DataType(DataType.Date)]
        public String PaymentDateTime { get; set; }
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

        [ForeignKey("Invoice")]
        public int? InvoiceId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
    }

}