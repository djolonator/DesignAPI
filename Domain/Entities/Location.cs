
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public record Location
    {
        [Key]
        public int LocationId { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
