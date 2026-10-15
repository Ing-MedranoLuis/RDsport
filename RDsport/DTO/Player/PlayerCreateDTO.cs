using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.Player
{
    public class PlayerCreateDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? DominicanId { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? Position { get; set; }
        public int TeamId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
