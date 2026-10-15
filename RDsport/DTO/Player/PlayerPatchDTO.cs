using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.Player
{
    public class PlayerPatchDTO
    {

       
        public string? Name { get; set; }
        
        [MaxLength(12, ErrorMessage = "You over passed the maximum caracteres")]
        [MinLength(9, ErrorMessage = "You are under the minimum caracteres")]
        public string? DominicanId { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? Position { get; set; }
        public int? TeamId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
