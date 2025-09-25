using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelWebApi.UserModels;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using HotelWebApi.Helpers;
using HotelWebApi.Dtos.User;

namespace HotelWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDBContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(UserDBContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            this._mapper = mapper;
            this._config = config;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        // GET: api/UsersRole
        [HttpGet]
        [Route("Role")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("NewRole")]
        public async Task<ActionResult<User>> PostRole(UserRoleCreateDto roleDto)
        {
            //string password = PasswordHasher.hashPassword(userDto.Password);
            var userRole = _mapper.Map<Role>(roleDto);
            _context.Roles.Add(userRole);
            await _context.SaveChangesAsync();
            return Ok();
        }
        // DELETE: api/Users/5
        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var userRole = await _context.Roles.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(userRole);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {

            try
            {
                TokenResponse tokenResponse = new TokenResponse();
                string password = PasswordHasher.hashPassword(user.Password);
                //Console.WriteLine(user.Username);
                //Console.WriteLine(user.Password);
                //Console.WriteLine(password);
                User curUser = _context.Users.Where(u => u.Username == user.Username && u.Password == password).FirstOrDefault();
                Role userRole = _context.Roles.Where(r=>r.Id==curUser.RoleId).FirstOrDefault();
                
                UserDto userDto = new UserDto();
                userDto.Id = curUser.Id;
                //userDto.Id = userRole.Id;
                userDto.Username = curUser.Username;
                userDto.FirstName = curUser.FirstName;
                userDto.LastName = curUser.LastName;
                userDto.RoleId = curUser.RoleId;
                userDto.Role = userRole.Name;
                userDto.Email = curUser.Email;
                //Console.WriteLine(curUser);
                //foreach (var data in curUser) {
                //    Console.WriteLine(data);
                //}

                if (curUser != null)
                {
                    var token = Generate(userDto);
                    tokenResponse.JWTToken = token;
                    return Ok(tokenResponse);
                }
                return BadRequest("Username or password incorrect!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //[HttpPost]
        //[Route("Login/Super")]
        //public IActionResult LoginSuper([FromBody] UserLoginDto user)
        //{

        //    try
        //    {
        //        TokenResponse tokenResponse = new TokenResponse();
        //        string password = PasswordHasher.hashPassword(user.Password);

        //        var curUser = _context.Users.Where(u => u.Username == user.Username && u.Password == password && u.Id==42).FirstOrDefault();

        //        if (curUser != null)
        //        {
        //            var token =  Generate(curUser);
        //            tokenResponse.JWTToken = token;
        //            return  Ok(tokenResponse);
        //        }
        //        return BadRequest("Username or password incorrect!");
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //// GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserPutDto userDto)
        {

            var user = _mapper.Map<User>(userDto);
            
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreateDto userDto)
        {
            string password = PasswordHasher.hashPassword(userDto.Password);
            var user = _mapper.Map<User>(userDto);
            user.Password = password;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetUser", new { id = user.Id }, user);
            return Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private string Generate(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim("lastName",user.LastName),
                new Claim("firstName",user.FirstName),
                //new Claim("gender",user.Gender),
                new Claim("roleId",user.RoleId.ToString()),
                new Claim("role",user.Role.ToString()),
                new Claim("id", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                _config["Jwt: Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private bool UserExists(int id) 
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
