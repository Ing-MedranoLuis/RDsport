using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDsport.Models
{
    public class Player
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? DominicanId { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? Position { get; set; }

        [ForeignKey("TeamId")]
        [Required]
        public int TeamId { get; set; }
        public Team? Team { get; set; }



    }
}
