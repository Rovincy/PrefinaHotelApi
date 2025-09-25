using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Booking
{
    public class BookingCreateDto
    {
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? BookStart { get; set; }
        public string? CustomerReceiptNumber { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? BookEnd { get; set; }
        public DateTime? Timestamp { get; set; }
        public decimal? Price { get; set; }
    }
}
