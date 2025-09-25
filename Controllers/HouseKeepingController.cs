using HotelWebApi.Dtos.HouseKeeping;
using HotelWebApi.Dtos.ServiceDetails;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseKeepingController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public HouseKeepingController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HouseKeepingItem>>> GetHouseKeepingDetails()
        {
            var response = await _context.HouseKeepingItems.ToListAsync();
            return Ok(response);
        }
         [HttpPost]
        public async Task<ActionResult<HouseKeepingItem>> AddHouseKeepingItem(HouseKeepingItemDto houseKeepingDto)
        {

            var houseKeepingData = mapper.Map<HouseKeepingItem>(houseKeepingDto);
            await _context.HouseKeepingItems.AddAsync(houseKeepingData);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(RoomType), new { id = roomType.Id }, roomType);
            return Ok(houseKeepingData);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteHouseKeeping(int id)
        {
            var item = await _context.HouseKeepingItems.FindAsync(id);

            if (item == null)
            {
                return BadRequest("Could not delete house keeping item");
            }
            if (item != null)
            {
                _context.HouseKeepingItems.Remove(item);
            }
            await _context.SaveChangesAsync();
            return Ok();

        }

    }
}
