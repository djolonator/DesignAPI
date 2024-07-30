
namespace Domain.Entities
{
    public record Location
    {
        public Guid LocationId { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

    }
}
