using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Dtos.RoomType
{
    public class RoomTypeCreateDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        //public bool IsActive { get; set; }
        public string? Image { get; set; }
        public IFormFile File { get; set; }

    }
}
