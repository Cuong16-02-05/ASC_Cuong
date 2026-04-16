using System.ComponentModel.DataAnnotations;

namespace ASC.Model
{
    public class MasterDataKey : BaseEntity, IAuditTracker
    {
        [Key]
        public string? UniqueId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }

    public class MasterDataValue : BaseEntity, IAuditTracker
    {
        [Key]
        public string? UniqueId { get; set; }
        public string? MasterDataKeyId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
