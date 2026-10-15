using System.ComponentModel.DataAnnotations;

namespace RDsport.Models
{
    public class OutStats
    {
        [Key]

        public int Id { get; set; }
        public int GroundOut { get; set; }
        public int FlyOuts { get; set; }
        public int StrikeOuts { get; set; }
      
    }
}
