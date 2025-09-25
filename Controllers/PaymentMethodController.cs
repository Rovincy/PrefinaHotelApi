using HotelWebApi.Dtos.Currency;
using HotelWebApi.Dtos.PaymentMethod;
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
    public class PaymentMethodController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public PaymentMethodController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethod()
        {

            var paymentMehod = await _context.PaymentMethods.ToListAsync();

            return Ok(paymentMehod);

        }
        [HttpGet("ActivePaymentMethod")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetActivePaymentMethod()
        {

            var paymentMehod = await _context.PaymentMethods.Where(x=>x.IsActive==true).ToListAsync();

            return Ok(paymentMehod);

        }

        [HttpPost]
        public async Task<ActionResult<PaymentMethod>> AddPaymentMethod(PaymentMethodCreateDto paymentMethoDto)
        {

            var paymentMethod = mapper.Map<PaymentMethod>(paymentMethoDto);
            paymentMethod.CreatedAt = System.DateTime.Now;
            await _context.PaymentMethods.AddAsync(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod.Id);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> PutPaymentMethod(PaymentMethodPutDto paymentMethodPut)
        {
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();
            var paymentMethod = mapper.Map<PaymentMethod>(paymentMethodPut);

            //var mapTaxes = mapper.Map
            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(paymentMethod.Id);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<PaymentMethod>> DeletePaymentMethod(int id)
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
