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
    public class PersonalInfoController : ControllerBase
    {
        private readonly PersonalInfoDbContext _db;
        public PersonalInfoController(PersonalInfoDbContext db)
        {
            _db = db;
        }

        [HttpGet("getAllInfo")]
        public async Task<IActionResult> getAllInfo()
        {
            var data = await _db.PersonalInfo.ToListAsync();

            if(data == null || !data.Any())
            {
                return NotFound("No personal information found.");
            }

            var listInfo = data.Select(item => new PersonalInfoDto
            {
                Id = item.Id,
                Name = item.FirstName,
                Email = item.Email

            });

            return Ok(listInfo);
        }

        [HttpGet("getInfoById/{id:int}")]
        public async Task<IActionResult> getInfoById(int id)
        {
            var singalRecord = await _db.PersonalInfo.FirstOrDefaultAsync(s => s.Id == id);

            if (singalRecord == null)
            {
                return NotFound($"Record with ID {id} was not found");
            }

            var res = new PersonalInfoDto()
            {
                Id = singalRecord.Id,
                Name = singalRecord.FirstName,
                Email = singalRecord.Email
            };

            return Ok(res);
        }

        [HttpPost("CreateInfo")]
        public async Task<IActionResult> CreateInfo([FromBody]PersonalInfo obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personalInfo = new PersonalInfo()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Phone = obj.Phone
            };

            await _db.PersonalInfo.AddAsync(personalInfo);
            await _db.SaveChangesAsync();

            var resDto = new CreateInfoDto()
            {
                FirstName = personalInfo.FirstName,
                //LastName = personalInfo.LastName,
                Email = personalInfo.Email,
                //Phone = personalInfo.Phone
            };

            return CreatedAtAction(nameof(getInfoById), new {id = personalInfo.Id}, resDto);
        }
    }
}
