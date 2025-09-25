using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Notes
{
    public class NoteCreateDto
    {
        public int? GuestId { get; set; }
        public string Notes { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
