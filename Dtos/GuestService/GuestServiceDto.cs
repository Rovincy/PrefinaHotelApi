using System;

namespace HotelWebApi.Dtos.GuestService
{
    public class GuestServiceDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int guestId { get; set; }
        public int? companyId { get; set; }
        public string Service { get; set; }
        public int Quantity { get; set; }
        public int? RoomId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? IsPaid { get; set; }
        public DateTime timestamp { get; set; }
    }
}
