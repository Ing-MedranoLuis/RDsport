using System.ComponentModel.DataAnnotations;

namespace RDsport.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? TeamName { get; set; }

        public string? ImageUrl { get; set; }

        public string? FieldAddress { get; set; }
    }
}
