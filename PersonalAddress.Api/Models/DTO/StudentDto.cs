namespace PersonalAddress.Api.Models.DTO
{
    public class StudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<CourseDto> Courses { get; set; } = new();
    }

}
