using System.ComponentModel.DataAnnotations;

namespace RDsport.DTO.Teams
{
    public class TeamCreateDTO
    {
        [Required]
        public string? TeamName { get; set; }

        public string? ImageUrl { get; set; }

        public string? FieldAddress { get; set; }

        public IFormFile? Image { get; set; }
    }
}
