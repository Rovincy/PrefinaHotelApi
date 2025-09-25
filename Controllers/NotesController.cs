//using HotelWebApi.Data;
using HotelWebApi.Dtos.Notes;
using HotelWebApi.Dtos.Room;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly FrankiesHotelContext _context;
        private readonly IMapper mapper;
        public NotesController(FrankiesHotelContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            var response = await _context.Notes.ToListAsync();
            return Ok(response);
        }
        // POST: api/AddRoom
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Note>> AddNotes(NoteCreateDto noteDto)
        {

            var note = mapper.Map<Note>(noteDto);
            note.Timestamp = DateTime.Now;
            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Room), new { id = room.Id }, room);
            return Ok(note.Id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var notes= await _context.Notes.FindAsync(id);
            
            if(notes == null)
            {
                return BadRequest("Could not find note");
            }

           _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
