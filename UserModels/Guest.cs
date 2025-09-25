using System;
using System.Collections.Generic;

namespace HotelWebApi.UserModels
{
    public partial class Guest
    {
        public Guest()
        {
            BillingCustomerIdTransferedFromNavigations = new HashSet<Billing>();
            BillingCustomers = new HashSet<Billing>();
            BillingsHistoryCustomerIdTransferedFromNavigations = new HashSet<BillingsHistory>();
            BillingsHistoryCustomers = new HashSet<BillingsHistory>();
            Bookings = new HashSet<Booking>();
            GuestServices = new HashSet<GuestService>();
            Notes = new HashSet<Note>();
        }

        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Account { get; set; }
        public DateTime? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public int? NationalityId { get; set; }
        public string? Idtype { get; set; }
        public string? Idnumber { get; set; }
        public string? DocUrl { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Nationality Nationality { get; set; }
        public virtual ICollection<Billing> BillingCustomerIdTransferedFromNavigations { get; set; }
        public virtual ICollection<Billing> BillingCustomers { get; set; }
        public virtual ICollection<BillingsHistory> BillingsHistoryCustomerIdTransferedFromNavigations { get; set; }
        public virtual ICollection<BillingsHistory> BillingsHistoryCustomers { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<GuestService> GuestServices { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
