//using HotelWebApi.Data;
using HotelWebApi.Dtos.Room;
using HotelWebApi.Dtos.RoomType;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsTypeController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;
        public static IWebHostEnvironment _environment;
        public RoomsTypeController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomType>>> GetRoomTypes()
        {
            //dev
            var url = $"{Request.Scheme}://{Request.Host}//";
            //prod
            //var url = $"{Request.Scheme}://{Request.Host}/PrefinaHotelApi//";
            var response = await _context.RoomTypes.OrderBy(x=>x.Name.Trim()).ToListAsync();
            //var response = await _context.Rooms.OrderBy(x => x.Name.Trim()).ToListAsync();

            foreach (var roomType in response)
            {
                roomType.Image = url + roomType.Image;
            }
            return Ok(response);
        }
        // POST: api/AddRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoomType>> AddRoomType([FromForm] RoomTypeCreateDto roomTypeDto)
        {
            if (roomTypeDto.File == null || roomTypeDto.File.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Uploads/RoomsType")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads/RoomsType"));
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(roomTypeDto.File.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "Uploads/RoomsType", fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await roomTypeDto.File.CopyToAsync(fileStream);
                }

                roomTypeDto.Image = $"/Uploads/RoomsType/{fileName}";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error uploading file for room");
                return BadRequest($"Failed to upload image: {ex.Message}");
            }
            var roomType = mapper.Map<RoomType>(roomTypeDto);
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(RoomType), new { id = roomType.Id }, roomType);
            return Ok(roomType.Id);
        }
       [HttpPut]
        public async Task<ActionResult<RoomType>> PutRoomType([FromForm] RoomTypeDto roomTypeDto)
        {
            if (roomTypeDto.id == null)
            {
                return BadRequest("Room Type ID is required.");
            }

            if (roomTypeDto.File == null || roomTypeDto.File.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Uploads/RoomsType")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads/RoomsType"));
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(roomTypeDto.File.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "Uploads/RoomsType", fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await roomTypeDto.File.CopyToAsync(fileStream);
                }

                roomTypeDto.Image = $"/Uploads/RoomsType/{fileName}";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error uploading file for room update");
                return BadRequest($"Failed to upload image: {ex.Message}");
            }
            var data = _context.RoomTypes.Where(x => x.Id == roomTypeDto.id).FirstOrDefault();
            data.Name = roomTypeDto.Name;
            data.Description = roomTypeDto.Description;
            data.Price = roomTypeDto.Price;
            data.Image = roomTypeDto.Image;
            var roomType = mapper.Map<RoomType>(data);

            //var mapTaxes = mapper.Map
            _context.RoomTypes.Update(roomType);
            await _context.SaveChangesAsync();

            return Ok(roomType.Id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RoomType(int id)
        {
            var rooms = await _context.RoomTypes.FindAsync(id);
            
            var relatedRooms = _context.Rooms.Where(te => te.TypeId == id);
           
            if (relatedRooms != null)
            {
                foreach (var relatedRoom in relatedRooms)
                {
                    var relatedBookings = _context.Bookings.Where(te => te.RoomId == relatedRoom.Id);
                    if (relatedBookings != null)
                    {
                        _context.Bookings.RemoveRange(relatedBookings);
                    }
                }
                _context.Rooms.RemoveRange(relatedRooms);
            }
            
            if (rooms == null)
            {
                return BadRequest("Could not delete room Type");
            }

            _context.RoomTypes.Remove(rooms);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
