//using HotelWebApi.Data;
using HotelWebApi.Dtos.Guest;
using HotelWebApi.Dtos.Notes;
using HotelWebApi.Dtos.Room;
using HotelWebApi.Dtos.RoomType;
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
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;
        public static IWebHostEnvironment _environment;
        public RoomsController(IWebHostEnvironment environment, FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            //dev
            var url = $"{Request.Scheme}://{Request.Host}//";
            //prod
            //var url = $"{Request.Scheme}://{Request.Host}/PrefinaHotelApi//";
            var response = await _context.Rooms.Where(x => x.IsActive == true).OrderBy(x => x.Name.Trim()).ToListAsync();
            foreach (var room in response)
            {
                room.Image = room.Image == null ?"": url + room.Image;
            }
            return Ok(response);
        }
        [HttpGet("AllActiveRooms")]
        public async Task<ActionResult<IEnumerable<NewRoomDto>>> GetAllActiveRooms()
        {
            //dev
            //var url = $"{Request.Scheme}://{Request.Host}//";
            //prod
            var url = $"{Request.Scheme}://{Request.Host}/PrefinaHotelApi//";
            var rooms = await _context.Rooms
                .Where(x => x.IsActive == true)
                .Include(x => x.Type) // Eagerly load RoomType
                .OrderBy(x => x.Name.Trim())
                .Select(x => new NewRoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                    TypeId = (int)x.TypeId,
                    RoomTypeName = x.Type != null ? x.Type.Name : "Unknown",
                    IsActive = (bool)x.IsActive
                })
                .ToListAsync();
            foreach (var room in rooms)
            {
                room.Image = room.Image == null ? "" : url + room.Image;
            }
            return Ok(rooms);
        }

        [HttpGet("RoomsDetails")]
        public async Task<ActionResult<IEnumerable<NewRoomDto>>> GetRoomsDetails(int? roomTypeId)
        {
            //dev
            //var url = $"{Request.Scheme}://{Request.Host}//";
            //prod
            var url = $"{Request.Scheme}://{Request.Host}/PrefinaHotelApi//";
            var rooms = await _context.Rooms
                .Where(x => x.IsActive == true && x.TypeId==roomTypeId)
                .Include(x => x.Type) // Eagerly load RoomType
                .OrderBy(x => x.Name.Trim())
                .Select(x => new NewRoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                    TypeId = (int)x.TypeId,
                    RoomTypeName = x.Type != null ? x.Type.Name : "Unknown",
                    IsActive = (bool)x.IsActive
                })
                .ToListAsync();
            foreach (var room in rooms)
            {
                room.Image = room.Image == null ? "" : url + room.Image;
            }
            return Ok(rooms);
        }
        public class NewRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
    }
    [HttpGet("AllRooms")]
        public async Task<ActionResult<IEnumerable<Room>>> GetAllRooms()
        {
            //dev
            //var url = $"{Request.Scheme}://{Request.Host}//";
            //prod
            var url = $"{Request.Scheme}://{Request.Host}/PrefinaHotelApi//";
            var response = await _context.Rooms.OrderBy(x => x.Name.Trim()).ToListAsync(); 
            
            foreach (var room in response)
            {
                room.Image = room.Image == null ? "" : url + room.Image;
            }
            return Ok(response);
        }
        // POST: api/AddRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom([FromForm] RoomDto roomDto)
        {
            if (roomDto.File == null || roomDto.File.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Uploads/Rooms")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads/Rooms"));
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(roomDto.File.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "Uploads/Rooms", fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await roomDto.File.CopyToAsync(fileStream);
                }

                roomDto.Image = $"/Uploads/Rooms/{fileName}";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error uploading file for room");
                return BadRequest($"Failed to upload image: {ex.Message}");
            }

            try
            {
                var room = mapper.Map<Room>(roomDto);
                await _context.Rooms.AddAsync(room);
                await _context.SaveChangesAsync();
                return Ok(room.Id);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error saving room to database");
                return StatusCode(500, $"Failed to save room: {ex.Message}");
            }
        }
        [HttpPut]
        public async Task<ActionResult<Room>> PutRoom([FromForm] RoomDto roomDto)
        {
            if (roomDto.id == null)
            {
                return BadRequest("Room ID is required.");
            }

            if (roomDto.File == null || roomDto.File.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            try
            {
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Uploads/Rooms")))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads/Rooms"));
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(roomDto.File.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "Uploads/Rooms", fileName);

                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await roomDto.File.CopyToAsync(fileStream);
                }

                roomDto.Image = $"/Uploads/Rooms/{fileName}";
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error uploading file for room update");
                return BadRequest($"Failed to upload image: {ex.Message}");
            }

            try
            {
                var existingRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == roomDto.id);
                if (existingRoom == null)
                {
                    return NotFound($"Room with ID {roomDto.id} not found.");
                }

                existingRoom.Name = roomDto.Name;
                existingRoom.IsActive = roomDto.IsActive;
                existingRoom.TypeId = roomDto.TypeId;
                existingRoom.Image = roomDto.Image;

                _context.Rooms.Update(existingRoom);
                await _context.SaveChangesAsync();

                return Ok(existingRoom.Id);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating room in database");
                return StatusCode(500, $"Failed to update room: {ex.Message}");
            }
        }
        // DELETE: api/DeleteRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpDelete]
        public async Task<ActionResult<Room>> DeleteRoom(int id)
        {

            //var notes = await _context.Notes.FindAsync(id);

            //if (notes == null)
            //{
            //    return BadRequest("Could not find note");
            //}

            //_context.Notes.Remove(notes);
            //await _context.SaveChangesAsync();
            //return NoContent();

            var room = await _context.Rooms.FindAsync(id);
             _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok();
        }

    }
}
