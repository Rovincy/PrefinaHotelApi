using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class Booking
    {
        public Booking()
        {
            GuestServices = new HashSet<GuestService>();
        }

        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? GuestId { get; set; }
        public int? CompanyId { get; set; }
        public DateTime? BookStart { get; set; }
        public DateTime? BookEnd { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public DateTime? Timestamp { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsNightAudited { get; set; }
        public string? CustomerReceiptNumber { get; set; }
        public decimal? Price { get; set; }

        public virtual Company Company { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual Room Room { get; set; }
        public virtual ICollection<GuestService> GuestServices { get; set; }
    }
}
