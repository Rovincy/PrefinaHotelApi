using HotelWebApi.Dtos.Booking;
using HotelWebApi.Dtos.GuestService;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestServiceController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public GuestServiceController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestService>>> GetGuestService()
        {
            var response = await _context.GuestServices.ToListAsync();
            return Ok(response);
        }
        [HttpPost]
        public async Task<ActionResult<GuestService>> AddGuestService(GuestServiceDto guestServiceDto)
        {
            var guestData = await _context.Guests.Where(x=>x.Id == guestServiceDto.guestId).FirstOrDefaultAsync();
            var bookingData= await _context.Bookings.Where(x=> x.Id== guestServiceDto.BookingId).FirstOrDefaultAsync();
            guestServiceDto.companyId = guestData.CompanyId;
            var guestservice = mapper.Map<GuestService>(guestServiceDto);
            guestservice.Timestamp = DateTime.Now;
            guestservice.TotalPrice = guestservice.Quantity * guestservice.UnitPrice;

            //var guestServices = _context.GuestServices.Where(x => (x.IsNightAudited == false || x.IsNightAudited == null) && x.GuestId == guestId).ToList();
            //if (guestServices.Any())
            //{
            //    //n_auditCount = n_auditCount + guestServices.Count;
            //    foreach (var guestService in guestServices)
            //    {
            var cedisRate = await _context.Currencies.Where(x=>x.Id==1).ToListAsync();

                    Billing billing_services = new Billing();
                    billing_services.Debit = guestservice.UnitPrice;
                    billing_services.CustomerId = guestData.Id;
                    billing_services.CompanyId = guestData.CompanyId;
                    billing_services.RoomId = guestservice.RoomId;
                    billing_services.Description = guestservice.Service;
                    billing_services.Currency = "GHS";
                    billing_services.Timestamp = DateTime.Now;
                    billing_services.CustomerBookingId = guestservice.BookingId;
                    billing_services.ReceiptNumber = bookingData.CustomerReceiptNumber;
                    billing_services.CurrencyRate = cedisRate[0].Rate;

                    BillingsHistory billingsHistory_services = new BillingsHistory();
                    billingsHistory_services.Debit = guestservice.UnitPrice;
                    billingsHistory_services.CustomerId = guestData.Id;
                    billingsHistory_services.CompanyId = guestData.CompanyId;
                    billingsHistory_services.RoomId = guestservice.RoomId;
                    billingsHistory_services.Description = guestservice.Service;
                    billingsHistory_services.Currency = "GHS";
                    billingsHistory_services.Timestamp = DateTime.Now;
                    billingsHistory_services.CustomerBookingId = guestservice.BookingId;
                    billingsHistory_services.ReceiptNumber= bookingData.CustomerReceiptNumber;
                    billingsHistory_services.CurrencyRate = cedisRate[0].Rate;

                    guestservice.IsNightAudited = true;

                    _context.Billings.Add(billing_services);
                    _context.BillingsHistories.Add(billingsHistory_services);
                    //_context.SaveChanges();
            //    }

            //    //GuestService g_services = new GuestService();
            //    //n_audit.Amount = amount;
            //    //n_audit.Timestamp = DateTime.Now;
            //    //n_audit.NumberOfTransaction = n_auditCount;
            //    //_context.NightAudits.Add(n_audit);
            //    //_context.GuestServices.UpdateRange(guestServices);
            //    //_context.SaveChanges();
            //}


            await _context.GuestServices.AddAsync(guestservice);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(guestservice.Id);
        }
        [HttpPut("serviceId")]
        public async Task<ActionResult<GuestService>> updateGuestService(long serviceId)
        {
            var serviceToUpdate = await _context.GuestServices.Where(te=>te.Id == serviceId).ToListAsync();
            if (serviceToUpdate != null)
            {
                serviceToUpdate.ForEach(element =>
                {
                    element.IsPaid = 1;
                });
            }
            _context.GuestServices.UpdateRange(serviceToUpdate);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(serviceId);
          
        }
    }
}
