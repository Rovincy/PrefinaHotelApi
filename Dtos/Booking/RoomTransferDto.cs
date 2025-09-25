using System;

namespace HotelWebApi.Dtos.Booking
{
    public class RoomTransferDto
    {
            public int? RoomId { get; set; }
            public int? GuestId { get; set; }
            public DateTime? BookStart { get; set; }
            public DateTime? CheckInTime { get; set; }
            public DateTime? BookEnd { get; set; }
            public DateTime? Timestamp { get; set; }
    }
}
