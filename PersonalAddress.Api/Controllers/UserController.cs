using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAddress.Api.Data;
using PersonalAddress.Api.Models;
using PersonalAddress.Api.Models.DTO;

namespace PersonalAddress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PersonalInfoDbContext _db;
        public UserController(PersonalInfoDbContext db)
        {
            _db = db;
        }

        // GET: api/User
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _db.Users.Include(u => u.Profile).ToListAsync();

            var usersDto = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Profile = new ProfileDTO
                {
                    ProfileId = u.Profile.ProfileId,
                    Address = u.Profile.Address,
                    PhoneNo = u.Profile.PhoneNo
                }
            }).ToList();

            return Ok(usersDto);
        }


        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _db.Users.Include(u => u.Profile)
                                           .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Mapping the User entity to UserDTO
            var userDto = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Profile = new ProfileDTO
                {
                    ProfileId = user.Profile.ProfileId,
                    Address = user.Profile.Address,
                    PhoneNo = user.Profile.PhoneNo
                }
            };

            return Ok(userDto);
        }

        // POST: api/User
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody]UserDto userDto)
        {
            // Map UserDTO to User entity
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Profile = new Profile
                {
                    Address = userDto.Profile.Address,
                    PhoneNo = userDto.Profile.PhoneNo
                }
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Return the created UserDTO
            userDto.UserId = user.UserId; // Add the generated UserId
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
        }


        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            
            var user = await _db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Profile.Address = userDto.Profile.Address;
            user.Profile.PhoneNo = userDto.Profile.PhoneNo;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            var response = new DeleteResponseDto
            {
                Message = $"Record with Id = {id} Update successfully.",
                Id = id
            };

            return Ok(response);
        }




        // DELETE: api/User/5
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            // Delete the associated Profile first to maintain referential integrity
            _db.Profiles.Remove(user.Profile);

            // Now delete the User
            _db.Users.Remove(user);

            await _db.SaveChangesAsync();

            var response = new DeleteResponseDto
            {
                Message = $"Record with Id = {id} deleted successfully.",
                Id = id
            };

            return Ok(response);
        }



    }
}
