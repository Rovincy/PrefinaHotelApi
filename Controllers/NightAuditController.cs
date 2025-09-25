using HotelWebApi.Dtos.Billing;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NightAuditController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public NightAuditController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Billing>>> PostBillings(int guestId)
        {
            DateTime currentDate = DateTime.Now.Date; // Get the current date without time
            decimal? amount = 0;
            NightAudit n_audit = new NightAudit();
            int? n_auditCount = 0;

                    var guest = _context.Guests.Where(x => x.Id == guestId).FirstOrDefault();
            var activeBookings = _context.Bookings
                .Where(x => x.CheckInTime != null && x.BookStart.HasValue && currentDate >= x.BookStart.Value.Date && x.BookEnd.HasValue && currentDate <= x.BookEnd.Value.Date && x.GuestId == guestId)
                .ToList();
            foreach (var booking in activeBookings)
            {
                var reservedRooms = _context.Rooms.Where(x => x.Id == booking.RoomId).ToList();
                foreach (var room in reservedRooms)
                {
                    // Check if billing data already exists  for this room
                    bool billingExists = _context.Billings.Any(x => x.RoomId == room.Id && x.Currency=="USD"&&x.CustomerId==booking.GuestId&&x.IsAccomodation==true);
                    if (billingExists)
                    {
                        // Billing data already exists for this room, skip it
                        continue;
                    }
                    //Console.WriteLine("This does not exists",booking.Id);

                    //var roomType = _context.RoomTypes.Where(x => x.Id == room.TypeId).ToList();

                    var daysInterval = (int)((TimeSpan)(booking.BookEnd - booking.BookStart)).TotalDays;
                    // Accommodation
                    Billing billing = new Billing();
                    billing.Debit = booking.Price * daysInterval;
                    //billing.Debit = roomType[0].Price;
                    billing.CustomerId = booking.GuestId;
                    billing.CompanyId = guest.CompanyId;
                    billing.RoomId = room.Id;
                    billing.Description = "Accommodation for " + room.Name;
                    billing.Currency = "USD";
                    billing.Timestamp = DateTime.Now;
                    billing.IsAccomodation = true;

                    BillingsHistory billingsHistory = new BillingsHistory();
                    billingsHistory.Debit = booking.Price * daysInterval;
                    //billingsHistory.Debit = roomType[0].Price;
                    billingsHistory.CustomerId = booking.GuestId;
                    billingsHistory.CompanyId = guest.CompanyId;
                    billingsHistory.RoomId = room.Id;
                    billingsHistory.Description = "Accommodation for " + room.Name;
                    billingsHistory.Currency = "USD";
                    billingsHistory.Timestamp = DateTime.Now;
                    billingsHistory.IsAccomodation = true;

                    _context.Billings.Add(billing);
                    _context.BillingsHistories.Add(billingsHistory);

                    //booking.IsNightAudited = true;
                    _context.Bookings.UpdateRange(booking);
                    _context.SaveChanges();


                }
                    // Guest Services if any
            }
                    var guestServices = _context.GuestServices.Where(x => (x.IsNightAudited == false || x.IsNightAudited == null) && x.GuestId == guestId).ToList();
                    if (guestServices.Any())
                    {
                        n_auditCount = n_auditCount + guestServices.Count;
                        foreach (var guestService in guestServices)
                        {
                            Billing billing_services = new Billing();
                            billing_services.Debit = guestService.UnitPrice;
                            billing_services.CustomerId = guestId;
                            billing_services.CompanyId = guest.CompanyId;
                            billing_services.RoomId = guestService.RoomId;
                            billing_services.Description = guestService.Service;
                            billing_services.Currency = "GHS";
                            billing_services.Timestamp = DateTime.Now;

                            BillingsHistory billingsHistory_services = new BillingsHistory();
                            billingsHistory_services.Debit = guestService.UnitPrice;
                            billingsHistory_services.CustomerId = guestId;
                            billingsHistory_services.CompanyId = guest.CompanyId;
                            billingsHistory_services.RoomId = guestService.RoomId;
                            billingsHistory_services.Description = guestService.Service;
                            billingsHistory_services.Currency = "GHS";
                            billingsHistory_services.Timestamp = DateTime.Now;

                            amount = amount + guestService.UnitPrice;
                            guestService.IsNightAudited = true;

                            _context.Billings.Add(billing_services);
                            _context.BillingsHistories.Add(billingsHistory_services);
                            _context.SaveChanges();
                        }

                        GuestService g_services = new GuestService();
                        n_audit.Amount = amount;
                        n_audit.Timestamp = DateTime.Now;
                        n_audit.NumberOfTransaction = n_auditCount;
                        _context.NightAudits.Add(n_audit);
                        _context.GuestServices.UpdateRange(guestServices);
                        _context.SaveChanges();
                    }
                

            return Ok();
            //var billing = mapper.Map<Billing>(billingCreate);
            //var billingHistory = mapper.Map<BillingsHistory>(billingCreate);

            //await _context.Billings.AddAsync(billing);
            //await _context.BillingsHistories.AddAsync(billingHistory);
            //await _context.SaveChangesAsync();

            //return Ok(billing.Id);
        }
   
    }
}
