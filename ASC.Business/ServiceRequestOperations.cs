using ASC.DataAccess;
using ASC.Model;

namespace ASC.Business
{
    public interface IServiceRequestOperations
    {
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task<ServiceRequest?> GetServiceRequestByIdAsync(string id);
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync();
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerEmail);
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerEmail);
        Task<ServiceRequest> UpdateServiceRequestStatusAsync(string id, string status, string updatedBy);
        Task<ServiceRequest> AssignEngineerAsync(string requestId, string engineerEmail, string updatedBy);
    }

    public class ServiceRequestOperations : IServiceRequestOperations
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRequestOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            request.UniqueId = Guid.NewGuid().ToString();
            request.Status = "Pending";
            request.RequestedDate = DateTime.UtcNow;
            request.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<ServiceRequest>().CreateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return request;
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdAsync(string id)
            => await _unitOfWork.Repository<ServiceRequest>().FindAsync(id);

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync()
            => await _unitOfWork.Repository<ServiceRequest>().FindAllAsync();

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByCustomerAsync(string customerEmail)
            => await _unitOfWork.Repository<ServiceRequest>().FindAllByAsync(r => r.CustomerEmail == customerEmail);

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByEngineerAsync(string engineerEmail)
            => await _unitOfWork.Repository<ServiceRequest>().FindAllByAsync(r => r.ServiceEngineer == engineerEmail);

        public async Task<ServiceRequest> UpdateServiceRequestStatusAsync(string id, string status, string updatedBy)
        {
            var request = await _unitOfWork.Repository<ServiceRequest>().FindAsync(id)
                ?? throw new Exception($"ServiceRequest {id} not found");
            request.Status = status;
            request.UpdatedBy = updatedBy;
            request.UpdatedDate = DateTime.UtcNow;
            if (status == "Completed") request.CompletedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<ServiceRequest>().UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return request;
        }

        public async Task<ServiceRequest> AssignEngineerAsync(string requestId, string engineerEmail, string updatedBy)
        {
            var request = await _unitOfWork.Repository<ServiceRequest>().FindAsync(requestId)
                ?? throw new Exception($"ServiceRequest {requestId} not found");
            request.ServiceEngineer = engineerEmail;
            request.UpdatedBy = updatedBy;
            request.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.Repository<ServiceRequest>().UpdateAsync(request);
            await _unitOfWork.SaveChangesAsync();
            return request;
        }
    }
}
