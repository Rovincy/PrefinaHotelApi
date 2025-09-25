using System;

namespace HotelWebApi.Dtos.Billing
{
    public class BillingTransferCreateDto
    {
        public int? customerId { get; set; }
        public string? description { get; set; }
        public string? currency { get; set; }
        public decimal? amount { get; set; }
        public int? receiverId { get; set; }
        public DateTime? timestamp { get; set; }
    }
}
