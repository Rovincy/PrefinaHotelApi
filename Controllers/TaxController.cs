using HotelWebApi.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using HotelWebApi.Dtos.Billing;
using HotelWebApi.Dtos.Tax;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public TaxController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxTable>>> GetTax()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Find bookings with null CheckInTime and BookEnd date before today
            var taxes = await _context.TaxTables.ToListAsync();

            return Ok(taxes);
        }

        [HttpGet("id")]
        public async Task<ActionResult<IEnumerable<TaxTable>>> GetSingleTax(int id)
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Find bookings with null CheckInTime and BookEnd date before today
            var taxes = await _context.TaxTables.Where(x=>x.Id==id).ToListAsync();

            return Ok(taxes);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<TaxTable>>> PostTaxes(TaxCreateDto taxCreate)
        {
            var tax = mapper.Map<TaxTable>(taxCreate);

            await _context.TaxTables.AddAsync(tax);
            await _context.SaveChangesAsync();

            return Ok(tax.Id);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<TaxTable>>> PutTaxes(TaxPutDto taxPut)
        {
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();
            var newTax = mapper.Map<TaxTable>(taxPut);

            //var mapTaxes = mapper.Map
            _context.TaxTables.Update(newTax);
            await _context.SaveChangesAsync();

            return Ok(newTax.Id);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<IEnumerable<TaxTable>>> DeleteTaxes(int id)
        {
            var tax = await _context.TaxTables.FindAsync(id);

            //var mapTaxes = mapper.Map
            _context.TaxTables.Remove(tax);
            await _context.SaveChangesAsync();

            return Ok(tax.Id);
        }
    }
}
