using HotelWebApi.Dtos.Booking;
using HotelWebApi.Dtos.Currency;
using HotelWebApi.UserModels;
using AutoMapper;
using MessagePack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Resend;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public BookingController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<ActionResult<Booking>> AddBooking(BookingCreateDto bookingDto)
        {
            DateTime currentTime = DateTime.Now;
            var receiptNumber = "";
            // forcing booking end time to have 12pm as booking time
            //if (bookingDto.BookEnd.HasValue)
            //{
            //    bookingDto.BookEnd = new DateTime(
            //        bookingDto.BookEnd.Value.Year,
            //        bookingDto.BookEnd.Value.Month,
            //        bookingDto.BookEnd.Value.Day,
            //        12,
            //        0,
            //        0
            //    );
            //}
            //if (bookingDto.BookStart.HasValue)
            //{
            //    bookingDto.BookStart = new DateTime(
            //        bookingDto.BookStart.Value.Year,
            //        bookingDto.BookStart.Value.Month,
            //        bookingDto.BookStart.Value.Day,
            //        13,
            //        0,
            //        0
            //    );
            //}


            // Check if the booking period overlaps with existing book
            var daysInterval = 0;
            var overlappingBooking = await _context.Bookings
                .Where(b =>
                    b.RoomId == bookingDto.RoomId &&
                        (b.IsCancelled != true || b.IsCancelled == null) && b.CheckInTime != null && b.CheckOutTime == null &&
                    //b.Id != bookingDto.Id && // Exclude the current booking if editing
                    (
                        (bookingDto.BookStart >= b.BookStart && bookingDto.BookStart < b.BookEnd) ||
                        (bookingDto.BookEnd > b.BookStart && bookingDto.BookEnd <= b.BookEnd) ||
                        (bookingDto.BookStart <= b.BookStart && bookingDto.BookEnd >= b.BookEnd)
                    )
                )
                .FirstOrDefaultAsync();

            if (overlappingBooking != null)
            {
                return BadRequest("Booking period overlaps with an existing booking.");
            }
            //bookingDto.CheckInTime = System.DateTime.Now;
            var guestData = await _context.Guests.Where(x => x.Id == bookingDto.GuestId).FirstOrDefaultAsync();
            //bookingDto.CompanyId = guestData?.CompanyId==null?0: guestData.CompanyId;
            receiptNumber = $"{guestData.Firstname.Substring(0, 3)}{DateTime.Now.ToString("MMddHHmmss")}";
            bookingDto.CustomerReceiptNumber = receiptNumber;
            bookingDto.Timestamp = DateTime.Now;
            var book = mapper.Map<Booking>(bookingDto);
            await _context.Bookings.AddAsync(book);
            await _context.SaveChangesAsync();

            var reservedRooms = _context.Rooms.Where(x => x.Id == bookingDto.RoomId).ToList();
            foreach (var room in reservedRooms)
            {
                // Check if billing data already exists  for this room
                bool billingExists = _context.Billings.Any(x => x.RoomId == room.Id && x.Currency == "USD" && x.CustomerId == bookingDto.GuestId && x.IsAccomodation == true);
                if (billingExists)
                {
                    // Billing data already exists for this room, skip it
                    continue;
                }
                //Console.WriteLine("This does not exists",booking.Id);

                //var roomType = _context.RoomTypes.Where(x => x.Id == room.TypeId).ToList();

                daysInterval = (int)((TimeSpan)(bookingDto.BookEnd - bookingDto.BookStart)).TotalDays;
                // Calculate remaining hours
                var timeDifference = (TimeSpan)(bookingDto.BookEnd - bookingDto.BookStart);
                var remainingHours = (decimal)(timeDifference.TotalHours % 24);
                if (remainingHours < 24 && remainingHours > 0)
                {
                    daysInterval++;
                }
                var cedisRate = await _context.Currencies.Where(x=>x.Id==1).ToListAsync();
                // Accommodation
                Billing billing = new Billing();
                billing.Debit = bookingDto.Price * daysInterval;
                //billing.Debit = roomType[0].Price;
                billing.CustomerId = bookingDto.GuestId;
                billing.CompanyId = bookingDto.CompanyId;
                billing.RoomId = room.Id;
                billing.Description = "Accommodation for " + room.Name;
                billing.Currency = "USD";
                billing.Timestamp = DateTime.Now;
                billing.IsAccomodation = true;
                billing.RoomRate = bookingDto.Price;
                billing.CustomerBookingId = book.Id;
                billing.ReceiptNumber = receiptNumber;
                billing.CurrencyRate = cedisRate[0].Rate;

                BillingsHistory billingsHistory = new BillingsHistory();
                billingsHistory.Debit = bookingDto.Price * daysInterval;
                //billingsHistory.Debit = roomType[0].Price;
                billingsHistory.CustomerId = bookingDto.GuestId;
                billingsHistory.CompanyId = bookingDto.CompanyId;
                billingsHistory.RoomId = room.Id;
                billingsHistory.Description = "Accommodation for " + room.Name;
                billingsHistory.Currency = "USD";
                billingsHistory.Timestamp = DateTime.Now;
                billingsHistory.IsAccomodation = true;
                billingsHistory.RoomRate = bookingDto.Price;
                billingsHistory.CustomerBookingId = book.Id;
                billingsHistory.ReceiptNumber = receiptNumber;
                billingsHistory.CurrencyRate = cedisRate[0].Rate;
                _context.Billings.Add(billing);
                _context.BillingsHistories.Add(billingsHistory);

                //booking.IsNightAudited = true;
                //_context.Bookings.UpdateRange(booking);
                //_context.SaveChanges();


            }

            //booking.IsNightAudited = true;
            //_context.Bookings.UpdateRange(booking);
            //_context.SaveChanges();

            await _context.SaveChangesAsync();


            IResend resend = ResendClient.Create("re_LcU7kitj_97EX4ffEPSo5iQsnfrpzSWE1");

            try
            {
            var resp = await resend.EmailSendAsync(new EmailMessage()
            {
                From = "onboarding@resend.dev",
                //From = "cobrafc225@gmail.com",
                To = "destaeldosso@gmail.com",
                Subject = "Room Reservation",
                HtmlBody = @"
<!DOCTYPE html>
<html lang=""""en"""">
<head>
    <meta charset=""""UTF-8"""">
    <meta name=""""viewport"""" content=""""width=device-width, initial-scale=1.0"""">
    <title>New Room Reservation</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        .header {
            background-color: #1a3c34;
            color: #ffffff;
            text-align: center;
            padding: 20px;
        }
        .header h1 {
            margin: 0;
            font-size: 24px;
        }
        .content {
            padding: 20px;
            color: #333333;
        }
        .content h2 {
            color: #1a3c34;
            font-size: 20px;
            margin-bottom: 15px;
        }
        .content p {
            font-size: 16px;
            line-height: 1.5;
            margin: 10px 0;
        }
        .footer {
            background-color: #1a3c34;
            color: #ffffff;
            text-align: center;
            padding: 10px;
            font-size: 12px;
        }
        @media only screen and (max-width: 600px) {
            .container {
                width: 100%;
                margin: 10px;
            }
            .content h2 {
                font-size: 18px;
            }
            .content p {
                font-size: 14px;
            }
        }
    </style>
</head>
<body>
    <div class=""""container"""">
        <div class=""""header"""">
            <h1>New Room Reservation</h1>
        </div>
        <div class=""""content"""">
            <h2>Booking Confirmation</h2>
            <p>A new room reservation has been successfully made at this Hotel. Please check the system for full booking details and ensure all necessary arrangements are made for the guest's arrival.</p>
        </div>
        <div class=""""footer"""">
            <p>Test's Hotel | Internal Notification</p>
        </div>
    </div>
</body>
</html>""
",
            });


            }
            catch (Exception ex) { 
            Console.WriteLine(ex.Message);
            }

            return Ok(daysInterval);
        }

        //[HttpPost]
        //public async Task<ActionResult<Booking>> AddBooking(BookingCreateDto bookingDto)
        //{

        //    var book = mapper.Map<Booking>(bookingDto);
        //    await _context.Bookings.AddAsync(book);
        //    await _context.SaveChangesAsync();

        //    return Ok(book.Id);
        //}

        [HttpPut]
        [Route("CheckIn")]
        public async Task<ActionResult<Booking>> CheckIn(CheckInOutDto checkInDto)
        {
            var bookings = await _context.Bookings.Where(x => x.Id == checkInDto.Id).ToListAsync();
            foreach (var data in bookings)
            {
                data.CheckInTime = checkInDto.CheckInOutTime;
            }
            _context.Bookings.UpdateRange(bookings);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(checkInDto.Id);
        }
        [HttpPut]
        [Route("ShortTimeExtension")]
        public async Task<ActionResult<Booking>> ExtendShortTime(ShortTimeExtensionDto extendBookEndDto)
        {
            DateTime currentTime = DateTime.Now;

            var existingBilling = await _context.Billings.FirstOrDefaultAsync(te => te.CustomerBookingId == extendBookEndDto.bookingId);
            var bookings = await _context.Bookings.FindAsync(extendBookEndDto.bookingId);
            var daysInterval = (int)((TimeSpan)(bookings.BookEnd - bookings.BookStart)).TotalDays;
            var timeDifference = (TimeSpan)(bookings.BookEnd - bookings.BookStart);
            var remainingHours = (decimal)(timeDifference.TotalHours % 24);
            if (remainingHours < 24 && remainingHours > 0)
            {
                daysInterval++;
            }
            
            if (extendBookEndDto == null)
            {
                return StatusCode(500, "extendBookEndDto is null");
            }
            if (extendBookEndDto.Hours == "16:00" && bookings != null)
            {
                if (bookings.BookEnd.HasValue)
                {
                    bookings.BookEnd = new DateTime(
                        bookings.BookEnd.Value.Year,
                        bookings.BookEnd.Value.Month,
                        bookings.BookEnd.Value.Day,
                        16,
                        0,
                        0
                    );
                }
            }
            if (extendBookEndDto.Hours == "22:00" && bookings != null)
            {
                if (bookings.BookEnd.HasValue)
                {
                    bookings.BookEnd = new DateTime(
                        bookings.BookEnd.Value.Year,
                        bookings.BookEnd.Value.Month,
                        bookings.BookEnd.Value.Day,
                        22,
                        0,
                        0
                    );
                    existingBilling.Debit = ((existingBilling.RoomRate * daysInterval) + extendBookEndDto?.Price);

                }


            }

            

           
           
            _context.Bookings.Update(bookings);
            _context.Billings.Update(existingBilling);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(extendBookEndDto.BookEnd?.Hour);
        }
        [HttpPut]
        [Route("ExtendBookDate")]
        public async Task<ActionResult<Booking>> ExtendBookDate(ExtendBookEndDto extendBookEndDto)
        {
            DateTime currentTime = DateTime.Now;

            var existingBilling = await _context.Billings.FirstOrDefaultAsync(te => te.CustomerBookingId == extendBookEndDto.Id);
            var daysInterval = (int)((TimeSpan)(extendBookEndDto.BookEnd - extendBookEndDto.BookStart)).TotalDays;
            if (existingBilling != null)
            {

                // Calculate remaining hours
                //extendBookEndDto.BookEnd
                var timeDifference = (TimeSpan)(extendBookEndDto.BookEnd - extendBookEndDto.BookStart);
                var remainingHours = (decimal)(timeDifference.TotalHours % 24);
                if (remainingHours >= 0 && remainingHours < 24)

                {


                    existingBilling.Debit = (existingBilling.RoomRate * (daysInterval + 1));



                }


                existingBilling.Debit = (existingBilling.RoomRate * daysInterval);

            }



            var bookings = await _context.Bookings.Where(x => x.Id == extendBookEndDto.Id).ToListAsync();
            foreach (var data in bookings)
            {
                data.BookEnd = extendBookEndDto.BookEnd;

            }
            _context.Bookings.UpdateRange(bookings);
            _context.Billings.Update(existingBilling);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(extendBookEndDto.BookEnd?.Hour);
        }


        //extending for short time hours
        [HttpPost]
        [Route("extendShortTime")]
        public async Task<ActionResult<HoursExtension>> ExtendShortBooking(HoursExtension shortBooking)
        {
            try
            {
                if (shortBooking == null)
                {
                    return BadRequest("Invalid request data");
                }

                _context.HoursExtensions.Add(shortBooking);
                    await _context.SaveChangesAsync();
                

                return Ok(shortBooking);
            }
            catch (DbUpdateException ex )
            {
                return StatusCode(500, $"Database operation failed. Please try again later {ex.Message}");
            }
            catch (Exception ex)
            {
                

                return StatusCode(500, $"An unexpected error occurred. Please contact support. {ex.Message}");
            }
        }

        //getting all short time bookings

        [HttpGet]
        [Route("getAllShortTimeExtension")]
        public async Task<ActionResult<HoursExtension>> GetAllShortBookings()
        {
            try
            {
               var shortTimeExtension=await  _context.HoursExtensions.ToListAsync();
                if (shortTimeExtension == null) return StatusCode(500, "could not get items");
                return  Ok(shortTimeExtension);

            }
            catch(DbUpdateException ) {
                return StatusCode(500, "Database error while fetching items");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "External error occurred while getting items ");
            }
        }

        [HttpPut]
        [Route("CheckOut")]
        public async Task<ActionResult<Booking>> CheckOut(CheckInOutDto checkOutDto)
        {
            List<Billing> billingData;
            List<BillingsHistory> billingsHistoryData;
            List<Booking> bookings;

            
            billingData = await _context.Billings.Where(x => x.CustomerId == checkOutDto.Id).ToListAsync();
            _context.Billings.RemoveRange(billingData);
            billingsHistoryData = billingData.Select(billing => new BillingsHistory
                {

                    CustomerId = billing.CustomerId,
                    RoomId = billing.RoomId,
                    Description = billing.Description,
                    Credit = billing.Credit,
                    Debit = billing.Debit,
                    CustomerBookingId = billing.CustomerBookingId,
                    Timestamp = billing.Timestamp,
                    Currency = billing.Currency,
                    IsPayment = billing.IsPayment,
                    PaymentMethod = billing.PaymentMethod,
                    IsAccomodation = billing.IsAccomodation,
                    CustomerIdTransferedFrom = billing.CustomerIdTransferedFrom,
                    RoomRate = billing.RoomRate,
                    ActualRoomRate = billing.ActualRoomRate,
                    CreatedBy = billing.CreatedBy,

                }).ToList();
            bookings = await _context.Bookings.Where(x => x.GuestId == checkOutDto.Id && (x.IsCancelled == false || x.IsCancelled == null) && x.CheckInTime != null && x.CheckOutTime == null).ToListAsync();

            

            foreach (var data in bookings)
            {
                data.CheckOutTime = DateTime.Now;
            }
            _context.Bookings.UpdateRange(bookings);
            // _context.BillingsHistories.AddRange(billingsHistoryData);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(checkOutDto.Id);
        }

        //[HttpGet('')]
        //public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        //{
        //    var response = await _context.Bookings.Where(x=>x.IsCancelled==null||x.IsCancelled==false).ToListAsync();
        //    return Ok(response);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Find bookings with null CheckInTime and BookEnd date before today
            var bookingsToCancel = await _context.Bookings
                .Where(b => b.CheckInTime == null && b.BookEnd < currentDate)
                .ToListAsync();

            if (bookingsToCancel.Count == 0)
            {
                Console.WriteLine("bookingsToCancel.Count: " + bookingsToCancel.Count);
                //return NotFound("No eligible bookings found to cancel.");
            }

            // Set IsCancelled to true for the found bookings
            foreach (var booking in bookingsToCancel)
            {
                booking.IsCancelled = true;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            var response = await _context.Bookings.Where(x => (x.IsCancelled == null || x.IsCancelled == false) && (x.CheckOutTime == null)).ToListAsync();
            return Ok(response);
        }
        [HttpGet("CheckedOut")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetCheckedOutBookings()
        {
            var response = await _context.Bookings.Where(x => (x.IsCancelled == null || x.IsCancelled == false) && (x.CheckOutTime != null))
                .OrderByDescending(x => x.CheckOutTime)
                .ToListAsync();
            return Ok(response);
        }

        [HttpGet("GetFilteredBooking")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetFilteredBooking()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Find bookings with null CheckInTime and BookEnd date before today
            var bookingsToCancel = await _context.Bookings
                .Where(b => b.CheckInTime == null && b.BookStart < currentDate)
                .ToListAsync();

            if (bookingsToCancel.Count == 0)
            {
                Console.WriteLine("bookingsToCancel.Count: " + bookingsToCancel.Count);
                return NotFound("No eligible bookings found to cancel.");
            }

            // Set IsCancelled to true for the found bookings
            foreach (var booking in bookingsToCancel)
            {
                booking.IsCancelled = true;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            var response = await _context.Bookings.Where(x => (x.IsCancelled == null || x.IsCancelled == false) && (x.CheckOutTime == null)).ToListAsync();
            return Ok(response);
        }



        [HttpGet("CheckOccupancy")]
        public async Task<ActionResult<Booking>> GetOccupancyInfo()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;

            // Count the number of occupied rooms
            int occupiedRooms = await _context.Bookings
            .Where(b => ((b.CheckInTime != null && b.CheckOutTime == null) && (b.IsCancelled == null || b.IsCancelled == false)))
            .Select(b => b.RoomId)
            .Distinct()
            .CountAsync();

            // Count the number of vacant rooms
            int totalRooms = await _context.Rooms.CountAsync();
            int vacantRooms = totalRooms - occupiedRooms;

            var occupancyInfo = new OccupancyInfoDto
            {
                OccupiedRooms = occupiedRooms,
                VacantRooms = vacantRooms
            };

            return Ok(occupancyInfo);
        }

        [HttpGet("vacantRooms")]
        public async Task<ActionResult<Booking>> GetVacantRoomsInfo()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now.Date;
            var rooms = await _context.Rooms.ToListAsync();
            if (rooms == null)
            {
                return BadRequest("rooms are null");
            }

            // Count the number of occupied rooms
            //  int occupiedRooms = await _context.Bookings
            //.Where(b => b.CheckInTime != null && b.CheckOutTime == null && (b.IsCancelled == null || b.IsCancelled == false))


            var vacancies = from room in _context.Rooms
                            join booking in _context.Bookings
                                .Where(b => b.CheckOutTime == null && (b.IsCancelled == null || b.IsCancelled == false))
                                on room.Id equals booking.RoomId into roomBookings
                            from booking in roomBookings.DefaultIfEmpty()

                            where booking == null || (booking.CheckInTime == null && booking.CheckOutTime == null && (booking.IsCancelled == false || booking.IsCancelled == null))

                            group new { booking, room } by new { room.Id, room.Name } into roomGroup
                            select new
                            {
                                RoomId = roomGroup.Key.Id,
                                RoomName = roomGroup.Key.Name,
                                Bookings = roomGroup
                                    .Where(b => b.booking != null && b.booking.BookStart != null && b.booking.BookEnd != null)
                                    .Select(b => new
                                    {
                                        BookStart = b.booking.BookStart,
                                        BookEnd = b.booking.BookEnd,
                                        BookingId = b.booking.Id,
                                        GuestName = _context.Guests
                                            .Where(e => e.Id == b.booking.GuestId)
                                            .Select(e => e.Firstname + " " + e.Lastname)
                                            .FirstOrDefault()
                                    }),
                                BookingCount = roomGroup
                                    .Where(b => b.booking != null && b.booking.BookStart != null && b.booking.BookEnd != null)
                                    .Count()
                            };







            // Count the number of vacant rooms
            // int totalRooms = await _context.Rooms.CountAsync();
            //int vacantRooms = totalRooms - occupiedRooms;

            //var occupancyInfo = new OccupancyInfoDto
            //{
            //   OccupiedRooms = occupiedRooms,
            //  VacantRooms = vacantRooms
            //};

            return Ok(vacancies);
        }

        [HttpGet("CheckRoomAvailability")]
        public async Task<ActionResult<IEnumerable<Booking>>> CheckRoomAvailability(int roomId, DateTime bookEnd)
        {
            try
            {
                DateTime bookStart = DateTime.Today;
                var overlappingBooking = await _context.Bookings
                .Where(b =>
                    b.RoomId == roomId && (b.IsCancelled == null || b.IsCancelled == false) &&
                    //b.Id != bookingDto.Id && // Exclude the current booking if editing
                    (
                        (bookStart >= b.BookStart && bookStart < b.BookEnd) ||
                        (bookEnd > b.BookStart && bookEnd <= b.BookEnd) ||
                        (bookStart <= b.BookStart && bookEnd >= b.BookEnd)
                    )
                )
                .FirstOrDefaultAsync();

                if (overlappingBooking != null)
                {
                    return BadRequest("Booking period overlaps with an existing booking.");
                }
                else
                {
                    return Ok(overlappingBooking);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("RoomTransfer")]
        public async Task<ActionResult<IEnumerable<Booking>>> RoomTransfer(BookingPutDto bookingPut)
        {
            var oldRoom = _context.Bookings.Where(x => x.Id == bookingPut.id).ToList();
            foreach (var data in oldRoom)
            {
                data.CheckOutTime = DateTime.Now;
                //var room = mapper.Map<Booking>(data);
                _context.Bookings.Update(data);

            }

            //bookingPut.checkInTime = DateTime.Now;
            Booking book = new Booking();
            book.BookStart = DateTime.Now;
            book.BookEnd = bookingPut.bookEnd;
            book.RoomId = bookingPut.roomId;
            book.CheckInTime = DateTime.Now;
            book.GuestId = bookingPut.customerId;
            book.Timestamp = DateTime.Now;
            //var tax = _context.TaxTables.Where(x => x.Id == taxPut.Id).ToList();

            //var mapTaxes = mapper.Map
            _context.Bookings.Add(book);
            await _context.SaveChangesAsync();

            return Ok(book.Id);
        }
        //[HttpGet("RoomAvailabity")]
        //public async Task<ActionResult<IEnumerable<Booking>>> GetRoomAvailability(int roomId)
        //{
        //    // Get the current date
        //    DateTime currentDate = DateTime.Now.Date;

        //    // Find bookings with null CheckInTime and BookEnd date before today
        //    var bookings = await _context.Bookings
        //        .Where(b => b.CheckInTime == null && b.BookStart < currentDate)
        //        .ToListAsync();
        //    return Ok(bookings);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Booking(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);
            var billings = await _context.Billings.Where(te => te.CustomerBookingId == id).ToListAsync();
            var billingHistory = await _context.BillingsHistories.Where(te => te.CustomerBookingId == id).ToListAsync();
            var guestServices = await _context.GuestServices.Where(te => te.GuestId == id).ToListAsync();
            if (Booking == null)
            {
                return BadRequest("Could not delete booking");
            }
            bookings.IsCancelled = true;
            _context.Bookings.Update(bookings);
            _context.Billings.RemoveRange(billings);
            _context.BillingsHistories.RemoveRange(billingHistory);
            _context.GuestServices.RemoveRange(guestServices);
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpPut("shortenBookingStay")]
        public async Task<IActionResult> ShortenBooking(BookingDto shortenBookingData)
        {
            //var bookingData = mapper.Map<Booking>(shortenBookingData);
            var existingBooking= await _context.Bookings.FirstOrDefaultAsync(te => te.Id == shortenBookingData.Id);
            if (shortenBookingData == null)
            {
                return StatusCode(500, "Booking object is null");
            }

            var currentDate = DateTime.Now;
            var newBookEnd=new DateTime(
                    currentDate.Year,
                    currentDate.Month,
                    currentDate.Day,
                    12,
                    0,
                    0
                );
            var existingBilling = await _context.Billings.FirstOrDefaultAsync(te => te.CustomerBookingId == shortenBookingData.Id);

            if (existingBooking.BookEnd.HasValue)
            {
                // Set the booking end time to 12:00 PM of the current date
                existingBooking.BookEnd = new DateTime(
                    currentDate.Year,
                    currentDate.Month,
                    currentDate.Day,
                    12,
                    0,
                    0
                );

                // Calculate the number of days between booking start and end
                var daysInterval = (int)((TimeSpan)(newBookEnd - existingBooking.BookStart)).TotalDays;

                if (existingBilling != null)
                {
                    // Calculate remaining hours
                    var timeDifference = (TimeSpan)(existingBooking.BookEnd - existingBooking.BookStart);
                    var remainingHours = (decimal)(timeDifference.TotalHours % 24);

                    if (remainingHours >= 0 && remainingHours < 24)
                    {
                        existingBilling.Debit = (existingBilling.RoomRate * (daysInterval + 1));
                    }
                    else
                    {
                        existingBilling.Debit = (existingBilling.RoomRate * daysInterval);
                    }
                }
            }

            // Update booking and billing entities in the context
            _context.Bookings.Update(existingBooking);
            _context.Billings.Update(existingBilling);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(shortenBookingData);
        }


        [HttpPost]
        [Route("NewGuestBooking")]
        public async Task<ActionResult<Booking>> NewGuestBooking(NewGuestBookingCreateDto bookingDto)
        {
            // Check if the booking period overlaps with existing bookings
            var overlappingBooking = await _context.Bookings
                .Where(b =>
                    b.RoomId == bookingDto.RoomId &&
                    //b.Id != bookingDto.Id && // Exclude the current booking if editing
                    (
                        (bookingDto.BookStart >= b.BookStart && bookingDto.BookStart < b.BookEnd) ||
                        (bookingDto.BookEnd > b.BookStart && bookingDto.BookEnd <= b.BookEnd) ||
                        (bookingDto.BookStart <= b.BookStart && bookingDto.BookEnd >= b.BookEnd)
                    )
                )
                .FirstOrDefaultAsync();

            if (overlappingBooking != null)
            {
                return BadRequest("Booking period overlaps with an existing booking.");
            }

            // Create a new guest object and save it to the database
            var guest = mapper.Map<Guest>(bookingDto);
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();


            //bookingDto.CheckInTime = System.DateTime.Now;

            // Now that the guest has been saved, you can obtain the guest's ID
            // and use it to create the booking record
            var book = mapper.Map<Booking>(bookingDto);
            book.GuestId = guest.Id; // Set the GuestId property in the booking
            await _context.Bookings.AddAsync(book);
            await _context.SaveChangesAsync();

            //bookingDto.CheckInTime = System.DateTime.Now;
            //var guest = mapper.Map<Guest>(bookingDto);
            //var book = mapper.Map<Booking>(bookingDto);
            //await _context.Guests.AddAsync(guest);
            //await _context.Bookings.AddAsync(book);
            //await _context.SaveChangesAsync();

            return Ok(book.Id);
        }
    }

}
