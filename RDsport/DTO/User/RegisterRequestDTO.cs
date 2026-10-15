using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.User
{
    public class RegisterRequestDTO
    {
        public string? FirstName { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
