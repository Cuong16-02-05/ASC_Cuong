using ASC.Tests.Mocks;
using ASC.Utilities;
using ASC.Web.Configuration;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests for SessionExtensions (Lab 3 - FakeSession usage)
    /// </summary>
    public class SessionExtensionsTests
    {
        [Fact]
        public void Session_SetAndGet_String_Test()
        {
            // Arrange
            var session = new FakeSession();
            var value = "TestValue";

            // Act
            session.SetSession("key", value);
            var result = session.GetSession<string>("key");

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Session_SetAndGet_Object_Test()
        {
            // Arrange
            var session = new FakeSession();
            var settings = new ApplicationSettings
            {
                Title = "Test App",
                AdminEmail = "admin@test.com"
            };

            // Act
            session.SetSession("settings", settings);
            var result = session.GetSession<ApplicationSettings>("settings");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test App", result.Title);
            Assert.Equal("admin@test.com", result.AdminEmail);
        }

        [Fact]
        public void Session_Get_NonExistentKey_ReturnsDefault_Test()
        {
            // Arrange
            var session = new FakeSession();

            // Act
            var result = session.GetSession<string>("nonexistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Session_Set_OverwritesExistingValue_Test()
        {
            // Arrange
            var session = new FakeSession();
            session.SetSession("key", "original");

            // Act
            session.SetSession("key", "updated");
            var result = session.GetSession<string>("key");

            // Assert
            Assert.Equal("updated", result);
        }

        [Fact]
        public void FakeSession_IsAvailable_ReturnsTrue_Test()
        {
            // Arrange
            var session = new FakeSession();

            // Assert
            Assert.True(session.IsAvailable);
        }

        [Fact]
        public void FakeSession_Clear_RemovesAllKeys_Test()
        {
            // Arrange
            var session = new FakeSession();
            session.SetSession("k1", "v1");
            session.SetSession("k2", "v2");

            // Act
            session.Clear();

            // Assert
            Assert.Empty(session.Keys);
        }
    }
}
