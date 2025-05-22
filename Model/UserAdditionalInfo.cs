using Microsoft.AspNetCore.Identity;
namespace EasyTrackerAPI.Models
{
    public class UserAdditionalInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IdentityUser User { get; set; }
    }
}