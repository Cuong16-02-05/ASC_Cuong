using ASC.Tests.Mocks;
using ASC.Web.Configuration;
using ASC.Web.Controllers;
using ASC.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ASC.Tests
{
    /// <summary>
    /// Unit tests for HomeController following TDD approach (Lab 3)
    /// Naming convention: ControllerName_ActionName_TestCondition_Test
    /// </summary>
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> _optionsMock;
        private readonly ApplicationSettings _settings;

        public HomeControllerTests()
        {
            _settings = new ApplicationSettings
            {
                Title = "Automobile Service Center",
                AdminEmail = "admin@asc.com",
                AdminName = "Admin",
                AdminPassword = "Admin@123456"
            };

            _optionsMock = new Mock<IOptions<ApplicationSettings>>();
            _optionsMock.Setup(o => o.Value).Returns(_settings);
        }

        private HomeController CreateController()
        {
            var controller = new HomeController(
                _optionsMock.Object,
                new TransientLoggerService(),
                new TransientLoggerService(),
                new ScopedLoggerService(),
                new ScopedLoggerService(),
                new SingletonLoggerService(),
                new SingletonLoggerService()
            );

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new FakeSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            return controller;
        }

        // --- Index Action Tests ---

        [Fact]
        public void HomeController_Index_ReturnsViewResult_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeController_Index_ReturnsNullModel_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.Null(result?.Model);
        }

        [Fact]
        public void HomeController_Index_NoValidationErrors_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            controller.Index();

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public void HomeController_Index_SessionSetWithSettings_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            controller.Index();

            // Assert - session should have been set (FakeSession stores value)
            Assert.True(controller.HttpContext.Session.Keys.Any());
        }

        // --- Error Action Tests ---

        [Fact]
        public void HomeController_Error_ReturnsViewResult_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Error();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeController_Error_NoValidationErrors_Test()
        {
            // Arrange
            var controller = CreateController();

            // Act
            controller.Error();

            // Assert
            Assert.True(controller.ModelState.IsValid);
        }

        // --- ApplicationSettings Tests ---

        [Fact]
        public void ApplicationSettings_Title_IsNotNull_Test()
        {
            // Assert
            Assert.NotNull(_settings.Title);
        }

        [Fact]
        public void ApplicationSettings_AdminEmail_IsValid_Test()
        {
            // Assert
            Assert.Contains("@", _settings.AdminEmail!);
        }

        [Fact]
        public void ApplicationSettings_AdminPassword_MeetsMinLength_Test()
        {
            // Assert
            Assert.True(_settings.AdminPassword!.Length >= 6);
        }
    }
}
