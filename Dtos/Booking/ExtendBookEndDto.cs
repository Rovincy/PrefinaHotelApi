using System;

namespace HotelWebApi.Dtos.Booking
{
    public class ExtendBookEndDto
    {
        public int Id { get; set; }
        public DateTime? BookEnd { get; set; }
        public DateTime? BookStart { get; set; }


    }
}
