using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDsport.Models
{
    public class Stats
    {
        [Key]

        public int Id { get; set; }
        public int Avg { get; set; }
        public int Hr { get; set; }
        public int Doubles { get; set; }
        public int Triples { get; set; }
        public int Hits { get; set; }
        
        public int Ops { get; set; }
        public int Bb { get; set; }
        [Required]
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player? Players { get; set; }

    }
}
