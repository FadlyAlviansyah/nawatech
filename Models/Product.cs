using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace NawatechApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Price { get; set; }

        public string? Image { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CategoryId { get; set; }
        public ProductCategory? Category { get; set; }
    }
}
