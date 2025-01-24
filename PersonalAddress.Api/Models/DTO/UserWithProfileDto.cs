namespace PersonalAddress.Api.Models.DTO
{
    public class UserWithProfileDto
    {
        public string UserName { get; set; } = string.Empty; // User's name
        public string Email { get; set; } = string.Empty;    // User's email
        public string Address { get; set; } = string.Empty;  // Profile address
        public string PhoneNo { get; set; } = string.Empty;  // Profile phone number
    }

}
