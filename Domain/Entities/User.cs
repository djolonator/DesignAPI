using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User: IdentityUser
    {
        public int Points { get; set; }
    }
}
