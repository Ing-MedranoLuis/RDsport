using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.User
{
    public class LoginRequestDTO
    {
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string? Password { get; set; }
    }
}
