using System;

namespace HotelWebApi.Dtos.Booking
{
    public class NewGuestBookingCreateDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Idtype { get; set; }
        public string Idnumber { get; set; }
        public int? RoomId { get; set; }
        public DateTime? BookStart { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? BookEnd { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
