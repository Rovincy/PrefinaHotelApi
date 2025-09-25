using System;

namespace HotelWebApi.Dtos.Booking
{
    public class ShortTimeExtensionDto
    {
        public int bookingId { get; set; }
        public DateTime? BookEnd { get; set; }
        public string Hours {  get; set; }
        public decimal? Price { get; set; }
       
    }
}
