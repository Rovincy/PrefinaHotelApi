using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.Room
{
    public class RoomDto
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
        public string? Image { get; set; }
        public IFormFile File { get; set; }

    }
}
