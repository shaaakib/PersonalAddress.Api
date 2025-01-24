using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalAddress.Api.Data;
using PersonalAddress.Api.Models.DTO;
using PersonalAddress.Api.Models;

namespace PersonalAddress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly PersonalInfoDbContext _db;
        public StudentsController(PersonalInfoDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _db.Students
         .Select(s => new
         {
             s.Name,
             s.Email,
             Courses = s.Courses.Select(c => new
             {
                 c.CourseName
             }).ToList()
         })
         .ToListAsync();

            return Ok(students);
        }

        // Get Single Student by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _db.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            var studentDto = new StudentDto
            {
                Name = student.Name,
                Email = student.Email,
                Courses = student.Courses.Select(c => new CourseDto
                {
                    CourseName = c.CourseName
                }).ToList()
            };

            return Ok(studentDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
        {
            var student = new Student
            {
                Name = studentDto.Name,
                Email = studentDto.Email,
                Courses = studentDto.Courses.Select(c => new Course
                {
                    CourseName = c.CourseName
                }).ToList()
            };

            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDto studentDto)
        {
            var student = await _db.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            // Update fields
            student.Name = studentDto.Name;
            student.Email = studentDto.Email;

            // Update Courses
            student.Courses.Clear();
            student.Courses = studentDto.Courses.Select(c => new Course
            {
                CourseName = c.CourseName
            }).ToList();

            _db.Students.Update(student);
            await _db.SaveChangesAsync();

            // Use a DTO to prevent circular reference in the response
            var result = new StudentDto
            {
                Name = student.Name,
                Email = student.Email,
                Courses = student.Courses.Select(c => new CourseDto
                {
                    
                    CourseName = c.CourseName
                }).ToList()
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _db.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();

            return Ok("Delete successfull");
        }


    }
}
