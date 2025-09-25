using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    public class NationalityController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;
        public NationalityController(FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nationality>>> GetNotes()
        {
            var response = await _context.Nationalities.ToListAsync();
            return Ok(response);
        }
    }
}
