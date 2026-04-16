using System.ComponentModel.DataAnnotations;

namespace ASC.Model
{
    public class ServiceRequest : BaseEntity, IAuditTracker
    {
        [Key]
        public string? UniqueId { get; set; }
        public string? RequestedServices { get; set; }
        public string? ServiceEngineer { get; set; }
        public string? Status { get; set; }
        public DateTime? RequestedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? VehicleName { get; set; }
        public string? VehicleRegistrationNumber { get; set; }
        public string? Comments { get; set; }
        public string? CustomerEmail { get; set; }
    }
}
