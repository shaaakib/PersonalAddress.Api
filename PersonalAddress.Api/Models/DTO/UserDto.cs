namespace PersonalAddress.Api.Models.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ProfileDTO Profile { get; set; }
    }
}
