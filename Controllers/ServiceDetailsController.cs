using HotelWebApi.Dtos.ServiceCategory.cs;
using HotelWebApi.Dtos.ServiceDetails;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceDetailsController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public ServiceDetailsController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDetail>>> GetServiceDetails()
        {
            var response = await _context.ServiceDetails.ToListAsync();
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<ServiceDetail>> AddService(ServiceDetailDto serviceDetailDto)
        {

            var serviceDetailData = mapper.Map<ServiceDetail>(serviceDetailDto);
            await _context.ServiceDetails.AddAsync(serviceDetailData);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(RoomType), new { id = roomType.Id }, roomType);
            return Ok(serviceDetailData);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteServiceDetails(int id)
        {
            var item = await _context.ServiceDetails.FindAsync(id);

            if (item == null)
            {
                return BadRequest("Could not delete service detail");
            }
            if (item != null)
            {
                _context.ServiceDetails.Remove(item);
            }
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
