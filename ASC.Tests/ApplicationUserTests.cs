using ASC.Model;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests for ApplicationUser (Lab 3)
    /// </summary>
    public class ApplicationUserTests
    {
        [Fact]
        public void ApplicationUser_IsActive_DefaultsTrue_Test()
        {
            var user = new ApplicationUser { IsActive = true };
            Assert.True(user.IsActive);
        }

        [Fact]
        public void ApplicationUser_FirstName_CanBeSet_Test()
        {
            var user = new ApplicationUser { FirstName = "John" };
            Assert.Equal("John", user.FirstName);
        }

        [Fact]
        public void ApplicationUser_CreatedDate_CanBeSet_Test()
        {
            var now = DateTime.UtcNow;
            var user = new ApplicationUser { CreatedDate = now };
            Assert.Equal(now, user.CreatedDate);
        }

        [Fact]
        public void Constants_Roles_Admin_IsCorrect_Test()
        {
            Assert.Equal("Admin", Constants.Roles.Admin);
        }

        [Fact]
        public void Constants_Roles_Engineer_IsCorrect_Test()
        {
            Assert.Equal("Engineer", Constants.Roles.Engineer);
        }

        [Fact]
        public void Constants_Roles_User_IsCorrect_Test()
        {
            Assert.Equal("User", Constants.Roles.User);
        }
    }
}
