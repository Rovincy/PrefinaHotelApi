using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Guest
{
    public class GuestDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        //public string Account { get; set; }
        public int CompanyId { get; set; }
        public DateTime? Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public int NationalityId { get; set; }
        public string Idtype { get; set; }
        public string Idnumber { get; set; }
        public string DocUrl { get; set; }

    }
}
