namespace Domain.Entities
{
    public record User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Points { get; set; }
    }
}
