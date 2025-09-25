//using HotelWebApi.Data;
using HotelWebApi.Dtos.Guest;
using HotelWebApi.Dtos.Room;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;

        public static IWebHostEnvironment _environment;
        public GuestsController(IWebHostEnvironment environment,FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
        {
            var response = await _context.Guests.OrderBy(x=>x.Firstname).ToListAsync();
            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult<Guest>> AddGuest([FromForm] GuestCreateDto guestDto)
        {
            if (guestDto.File != null && guestDto.File.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Uploads\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Uploads\\");
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(guestDto.File.FileName);
                    string filePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await guestDto.File.CopyToAsync(fileStream);
                    }

                    guestDto.DocUrl = "\\Uploads\\" + fileName;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var guest = mapper.Map<Guest>(guestDto);
            await _context.Guests.AddAsync(guest);
            await _context.SaveChangesAsync();

            return Ok(guest.Id);
        }
        [HttpPut]
        public async Task<ActionResult<Guest>> PutGuest(GuestDto guest)
        {
            if (guest == null)
            {
                return BadRequest("Guest data is null");
            }

            var existingGuest = await _context.Guests.FindAsync(guest.Id);
            if (existingGuest == null)
            {
                return NotFound("Could not find record");
            }

            existingGuest.NationalityId = guest.NationalityId;
            existingGuest.Idtype = guest.Idtype;
            existingGuest.Firstname = guest.Firstname;
            existingGuest.Lastname = guest.Lastname;
            existingGuest.Email = guest.Email;
            existingGuest.Gender = guest.Gender;
            existingGuest.Dob = guest.Dob;
            //existingGuest.CompanyId = guest.CompanyId;

            _context.Guests.Update(existingGuest);
            await _context.SaveChangesAsync();

            return Ok(existingGuest);
        }

        // POST: api/AddRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Guest>> AddGuest(GuestCreateDto guestDto)
        //{

        //    var guest = mapper.Map<Guest>(guestDto);
        //    await _context.Guests.AddAsync(guest);
        //    await _context.SaveChangesAsync();

        //    //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
        //    return Ok(guest.Id);
        //}
        [HttpDelete("{id}")]
         public async Task<IActionResult> DeleteGuest(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            var relatedNotes=_context.Notes.Where(te=>te.GuestId== guest.Id);
            var relatedBookings = _context.Bookings.Where(te => te.GuestId == guest.Id);
            if(relatedNotes != null)
            {
                _context.Notes.RemoveRange(relatedNotes);
            }
            if (relatedBookings != null)
            {
                _context.Bookings.RemoveRange(relatedBookings);
            }
            if (guest== null)
            {
                return NotFound();
            }
            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return NoContent();

        }


    }
}
