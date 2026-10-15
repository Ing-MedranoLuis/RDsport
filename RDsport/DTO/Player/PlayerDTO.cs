
using RDsport.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace RDsport.DTO.Player
{
    public class PlayerDTO
    {
        public string? Name { get; set; }
        public string? DominicanId { get; set; }
        public string? ImageUrl { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? Position { get; set; }
        public int TeamId { get; set; }
        public Team? team { get; set; }

    }
}
