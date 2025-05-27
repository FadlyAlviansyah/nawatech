using System.ComponentModel.DataAnnotations;

namespace NawatechApp.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? Users { get; set; }

        [Required]
        public required string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}