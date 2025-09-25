using HotelWebApi.UserModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using HotelWebApi.Dtos.Booking;
using System.Linq;
using HotelWebApi.Dtos.Currency;
using HotelWebApi.Dtos.Tax;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public CurrencyController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrency()
        {

            var currency = await _context.Currencies.ToListAsync();

            return Ok(currency);

        }
        [HttpPost]
        public async Task<ActionResult<Currency>> AddCurrency(CurrencyCreateDto currencyDto)
        {
                
             var currency = mapper.Map<Currency>(currencyDto);
             await _context.Currencies.AddAsync(currency);
             await _context.SaveChangesAsync();

             return Ok(currency.Id);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Currency>>> PutCurrency(CurrencyPutDto currencyPut)
        {
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();
            var currency = mapper.Map<Currency>(currencyPut);

            //var mapTaxes = mapper.Map
            _context.Currencies.Update(currency);
            await _context.SaveChangesAsync();

            return Ok(currency.Id);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<Currency>> DeleteCurrency(int id)
        {
            var billing = await _context.Currencies.FindAsync(id);
            _context.Currencies.Remove(billing);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
