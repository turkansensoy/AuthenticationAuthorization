using System.ComponentModel.DataAnnotations;

namespace CookieAuthentication.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? FullName { get; set; } = null;

        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";
    }
}
