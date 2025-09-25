using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class Company
    {
        public Company()
        {
            Billings = new HashSet<Billing>();
            BillingsHistories = new HashSet<BillingsHistory>();
            Bookings = new HashSet<Booking>();
            Guests = new HashSet<Guest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string TinNumber { get; set; }
        public bool? NonTaxable { get; set; }
        public decimal? FixRate { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }

        public virtual ICollection<Billing> Billings { get; set; }
        public virtual ICollection<BillingsHistory> BillingsHistories { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Guest> Guests { get; set; }
    }
}
