//using System;
//using System.Collections.Generic;

//namespace HotelWebApi.Data
//{
//    public partial class Room
//    {
//        public Room()
//        {
//            Bookings = new HashSet<Booking>();
//            RoomPrices = new HashSet<RoomPrice>();
//        }

//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Type { get; set; }
//        public int? CategoryId { get; set; }
//        public bool? IsActive { get; set; }

//        public virtual RoomCategory Category { get; set; }
//        public virtual ICollection<Booking> Bookings { get; set; }
//        public virtual ICollection<RoomPrice> RoomPrices { get; set; }
//    }
//}
