using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalAddress.Api.Models
{
    public class Profile
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProfileId { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
