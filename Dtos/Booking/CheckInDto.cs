using System;

namespace HotelWebApi.Dtos.Booking
{
    public class CheckInOutDto
    {
        public int Id { get; set; }
        public bool IsCorporate { get; set; }
        public DateTime? CheckInOutTime { get; set; }
    }
}
