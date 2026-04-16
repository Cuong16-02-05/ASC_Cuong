using System.ComponentModel.DataAnnotations;

namespace ASC.Model
{
    /// <summary>
    /// Product model - thêm vào cuối Lab 2 theo yêu cầu
    /// </summary>
    public class Product : BaseEntity, IAuditTracker
    {
        [Key]
        public string? UniqueId { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Category { get; set; }
    }
}
