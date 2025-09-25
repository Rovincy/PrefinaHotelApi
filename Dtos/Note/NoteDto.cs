using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Notes
{
    public class NoteDto
    {
        public int id;
        public int? GuestId { get; set; }
        public string Notes { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
