using HotelWebApi.Dtos.PaymentNote;
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
    public class PaymentNoteController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public PaymentNoteController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentNote>>> GetPaymentMethod()
        {

            var paymentMehod = await _context.PaymentNotes.ToListAsync();

            return Ok(paymentMehod);

        }
        //[HttpGet("ActivePaymentMethod")]
        //public async Task<ActionResult<IEnumerable<PaymentCategory>>> GetActivePaymentMethod()
        //{

        //    var paymentMehod = await _context.PaymentCategories.Where(x => x.IsActive == true).ToListAsync();

        //    return Ok(paymentMehod);

        //}

        [HttpPost]
        public async Task<ActionResult<PaymentNote>> AddPaymentMethod(PaymentNoteCreateDto paymentMethoDto)
        {

            var paymentMethod = mapper.Map<PaymentNote>(paymentMethoDto);
            //paymentMethod.CreatedAt = System.DateTime.Now;
            await _context.PaymentNotes.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod.Id);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<PaymentNote>>> PutPaymentCategory(PaymentNotePutDto paymentMethodPut)
        {
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();
            var paymentMethod = mapper.Map<PaymentNote>(paymentMethodPut);

            //var mapTaxes = mapper.Map
            _context.PaymentNotes.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod.Id);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<PaymentNote>> DeletePaymentMethod(int id)
        {
            var paymentCategory = await _context.PaymentNotes.FindAsync(id);
            _context.PaymentNotes.Remove(paymentCategory);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
