using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.User
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        
        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}
