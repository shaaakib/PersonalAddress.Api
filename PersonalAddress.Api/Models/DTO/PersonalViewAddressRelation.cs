using System.ComponentModel.DataAnnotations;

namespace PersonalAddress.Api.Models.DTO
{
    public class PersonalViewAddressRelation
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [Required, MaxLength(50)]
        public string Email { get; set; }
        [Required, MaxLength(15)]
        public string Phone { get; set; }

        public List<PersonalInfoAddress> AddressList { get; set; } = new List<PersonalInfoAddress> { new PersonalInfoAddress() };
    }
}
