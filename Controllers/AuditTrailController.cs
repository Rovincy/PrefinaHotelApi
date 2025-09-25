using HotelWebApi.Dtos.AuditTrail;
using HotelWebApi.Dtos.Currency;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTrailController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;
        public static IWebHostEnvironment _environment;
        public AuditTrailController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpPost]
        public async Task<ActionResult<Currency>> AddAuditTrail(AuditTrailCreateDto auditTrailDto)
        {
            auditTrailDto.Timestamp = DateTime.Now;
            var auditTrail = mapper.Map<AuditTrail>(auditTrailDto);
            await _context.AuditTrails.AddAsync(auditTrail);
            await _context.SaveChangesAsync();

            return Ok(auditTrail.Id);
        }
    }
}
