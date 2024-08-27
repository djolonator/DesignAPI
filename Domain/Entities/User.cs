using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User: IdentityUser
    {
        public int Points { get; set; }
    }
}
