using System.ComponentModel.DataAnnotations;

namespace NawatechApp.Models
{
    
    public class User
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public string? ProfilePictures { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}