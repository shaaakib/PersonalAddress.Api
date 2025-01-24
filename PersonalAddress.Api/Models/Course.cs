namespace PersonalAddress.Api.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
