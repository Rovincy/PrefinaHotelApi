using System;

namespace HotelWebApi.Dtos.Billing
{
    public class PostCorporateBillingDto
    {

        public int? customerId { get; set; }
        public string? description { get; set; }
        public string? currency { get; set; }
        public decimal? debit { get; set; }
        public decimal? credit { get; set; }
        public bool? isPayment { get; set; }
        public string PaymentMethod { get; set; }
        public int? CompanyId { get; set; }
        public int? CustomerIdTransferedFrom { get; set; }
        public string Notes { get; set; }
        public string ReceiptNumber { get; set; }
        public DateTime? timestamp { get; set; }
        public decimal? ActualRoomRate { get; set; }
        public int? CreatedBy { get; set; }
        public int? CustomerBookingId { get; set; }
        public int[] Bills { get; set; }


    }
}
