using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class BillingsHistory
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? RoomId { get; set; }
        public string? Description { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public int? CustomerBookingId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Currency { get; set; }
        public bool? IsPayment { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsAccomodation { get; set; }
        public int? CompanyId { get; set; }
        public int? CustomerIdTransferedFrom { get; set; }
        public string? Notes { get; set; }
        public string? ReceiptNumber { get; set; }
        public decimal? RoomRate { get; set; }
        public decimal? ActualRoomRate { get; set; }
        public int? CreatedBy { get; set; }
        public decimal? CurrencyRate { get; set; }

        public virtual Company Company { get; set; }
        public virtual Guest Customer { get; set; }
        public virtual Guest CustomerIdTransferedFromNavigation { get; set; }
        public virtual Room Room { get; set; }
    }
}
