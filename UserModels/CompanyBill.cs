using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class CompanyBill
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public int? RoomId { get; set; }
        public string? Description { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? CustomerBookingId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Currency { get; set; }
        public bool? IsPayment { get; set; }
        public int? CompanyId { get; set; }
        public int? CreatedBy { get; set; }
        public string? ReceiptNumber { get; set; }
        public decimal? RoomRate { get; set; }
        public int? BillingId { get; set; }
        public string? CheckNumber { get; set; }
    }
}
