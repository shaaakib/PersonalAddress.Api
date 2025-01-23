using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAddress.Api.Data;
using PersonalAddress.Api.Models.DTO;

namespace PersonalAddress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly PersonalInfoDbContext _db;
        public FilterController(PersonalInfoDbContext db)
        {
            _db = db;
        }

        [HttpGet("SearchPersonalFname")]
        public async Task<IActionResult> SearchPersonalName(string fname)
        {
            var filterFirstName = await _db.PersonalInfo.Where(n => n.FirstName == fname).
                Select(n => new PersonalInfoDto
                {
                    FirstName = n.FirstName,
                    //LastName = n.LastName,
                    Email = n.Email,
                    Phone = n.Phone,
                }).ToListAsync();

            if (string.IsNullOrWhiteSpace(fname)) return BadRequest(new { message = "First name cannot be null or empty." });

            return Ok(filterFirstName);
        }

        [HttpGet("SearchMultipleInfo")]
        public async Task<IActionResult> SearchMultipleInfo(string text)
        {
            // var filterData = _context.EmployeeMaster.Where(s => s.empName.StartsWith(searchText)).ToList();
            var contextList = await _db.PersonalInfo.Where(t => t.FirstName.Contains(text)).ToListAsync();
 
            return Ok(contextList);
        }

        [HttpGet("searchMemberEmployee")]
        public async Task<IActionResult> searchMemberEmployee(string? fname, string? email, string? phone)
        {
            var mList = await (from m in _db.PersonalInfo
                         where m.FirstName != ""
                         && (fname == null || m.FirstName.Contains(fname))
                         && (email == null || m.Email.StartsWith(email))
                         && (phone == null || m.Phone.StartsWith(phone))
                         select new PersonalInfoDto
                         {
                            FirstName = m.FirstName,
                            Email = m.Email,
                            Phone = m.Phone
                         }).ToListAsync();

            return Ok(mList);
        }

        [HttpPost("getDropDwonModel")]
        public async Task<IActionResult> getDropDwonModel()
        {
            var list = (from emp in _db.PersonalInfo
                        select new PersonalInfoDto
                        {
                            FirstName = emp.FirstName,
                        }).ToList();
            return Ok(list);
        }
    }
}
