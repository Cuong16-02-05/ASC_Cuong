using ASC.Model;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests for ServiceRequest model (Lab 3)
    /// </summary>
    public class ServiceRequestTests
    {
        [Fact]
        public void ServiceRequest_UniqueId_CanBeSet_Test()
        {
            // Arrange
            var request = new ServiceRequest();
            var id = Guid.NewGuid().ToString();

            // Act
            request.UniqueId = id;

            // Assert
            Assert.Equal(id, request.UniqueId);
        }

        [Fact]
        public void ServiceRequest_Status_DefaultsToNull_Test()
        {
            // Arrange & Act
            var request = new ServiceRequest();

            // Assert
            Assert.Null(request.Status);
        }

        [Fact]
        public void ServiceRequest_RequestedDate_CanBeSet_Test()
        {
            // Arrange
            var request = new ServiceRequest();
            var date = DateTime.UtcNow;

            // Act
            request.RequestedDate = date;

            // Assert
            Assert.Equal(date, request.RequestedDate);
        }

        [Theory]
        [InlineData("Pending")]
        [InlineData("InProgress")]
        [InlineData("Completed")]
        public void ServiceRequest_Status_AcceptsValidValues_Test(string status)
        {
            // Arrange
            var request = new ServiceRequest { Status = status };

            // Assert
            Assert.Equal(status, request.Status);
        }

        [Fact]
        public void ServiceRequest_InheritsBaseEntity_Test()
        {
            // Arrange & Act
            var request = new ServiceRequest();

            // Assert
            Assert.IsAssignableFrom<BaseEntity>(request);
        }

        [Fact]
        public void ServiceRequest_ImplementsIAuditTracker_Test()
        {
            // Arrange & Act
            var request = new ServiceRequest();

            // Assert
            Assert.IsAssignableFrom<IAuditTracker>(request);
        }
    }
}
