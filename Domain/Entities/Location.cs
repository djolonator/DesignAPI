
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public record Location
    {
        [Key]
        public int LocationId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

    }
}
