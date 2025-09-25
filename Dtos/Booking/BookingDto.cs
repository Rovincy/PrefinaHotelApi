using System;

namespace HotelWebApi.Dtos.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
        public DateTime? BookStart { get; set; }
        public DateTime? BookEnd { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public bool? IsCancelled { get; set; }
        public decimal? Price { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
