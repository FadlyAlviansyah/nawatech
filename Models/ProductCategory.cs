using System.ComponentModel.DataAnnotations;

namespace NawatechApp.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}