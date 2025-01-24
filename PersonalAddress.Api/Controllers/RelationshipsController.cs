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
    public class RelationshipsController : ControllerBase
    {
        private readonly PersonalInfoDbContext _db;
        public RelationshipsController(PersonalInfoDbContext db)
        {
            _db = db;
        }

        // One to One
        [HttpPost("CreateInfoAddrssOne")]
        public async Task<IActionResult> CreateInfoAddrssOne(PersonalViewRelation obj)
        {
            PersonalInfo _per = new()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Phone = obj.Phone
            };

            await _db.PersonalInfo.AddAsync(_per);
            await _db.SaveChangesAsync();

            PersonalInfoAddress _address = new()
            {
                Address = obj.Address,
                City = obj.City,
                State = obj.State,
                ZipCode = obj.ZipCode,
                PersonalId = _per.Id,
            };

            await _db.PersonalInfoAddress.AddAsync(_address);
            await _db.SaveChangesAsync();

            return Ok("Created Seccussfull");
        }

        // One to Many
        [HttpPost("CreateInfoAddrssMany")]
        public async Task<IActionResult> CreateInfoAddrssMany(PersonalViewAddressRelation obj)
        {
            PersonalInfo _per = new()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Phone = obj.Phone
            };

            await _db.PersonalInfo.AddAsync(_per);
            await _db.SaveChangesAsync();

            foreach (var item in obj.AddressList)
            {
                PersonalInfoAddress _address = new()
                {
                    Address = item.Address,
                    City = item.City,
                    State = item.State,
                    ZipCode = item.ZipCode,
                    PersonalId = _per.Id
                };

                await _db.PersonalInfoAddress.AddAsync(_address);
                await _db.SaveChangesAsync();
            }

            return Ok("Created Seccussfull");
        }
    }
}
