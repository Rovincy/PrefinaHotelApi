using HotelWebApi.Dtos.Room;
using HotelWebApi.Dtos.ServiceCategory.cs;
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
    public class ServiceController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public ServiceController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceCategory>>> GetServices()
        {
            var response = await _context.ServiceCategories.ToListAsync();
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceCategory>> AddServiceCategory(ServiceCreateDto serviceCreateDto)
        {

            var serviceCategory = mapper.Map<ServiceCategory>(serviceCreateDto);
            await _context.ServiceCategories.AddAsync(serviceCategory);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(RoomType), new { id = roomType.Id }, roomType);
            return Ok(serviceCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Service(int id)
        {
            var services = await _context.ServiceCategories.FindAsync(id);
            var relatedServiceDetails = _context.ServiceDetails.Where(te => te.ServiceCategoryId == id).ToList();
            var relatedHouseKeepingItems = _context.HouseKeepingItems.Where(x=>x.ServiceCategoryId == id).ToList();

            if (services == null)
            {
                return BadRequest("Could not delete service Category");
            }
            if (relatedServiceDetails != null)
            {
                foreach (var data in relatedServiceDetails)
                {
                    _context.ServiceDetails.Remove(data);

                }
            }
            if (relatedHouseKeepingItems != null)
            {
                foreach (var data in relatedHouseKeepingItems)
                {
                    _context.HouseKeepingItems.Remove(data);

                }
            }

            _context.ServiceCategories.Remove(services);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
