using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.Test.IntegrationTests
{
    public class VatRegistrationControllerTest
    {
        private readonly VatRegistrationController _controller;
        private readonly Mock<VatRegistrationService> _mockService;
        private readonly Mock<ILogger<VatRegistrationController>> _mockLogger;
        private readonly Mock<IVatRegistrationHandler> _mockUkHandler;
        private readonly Mock<IVatRegistrationHandler> _mockFrHandler;
        private readonly Mock<IVatRegistrationHandler> _mockDeHandler;

        public VatRegistrationControllerTest()
        {
            _mockLogger = new Mock<ILogger<VatRegistrationController>>();

            // Mock the handlers
            _mockUkHandler = new Mock<IVatRegistrationHandler>();
            _mockUkHandler.Setup(h => h.CountryCode).Returns("GB");
            _mockUkHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);

            _mockFrHandler = new Mock<IVatRegistrationHandler>();
            _mockFrHandler.Setup(h => h.CountryCode).Returns("FR");
            _mockFrHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);

            _mockDeHandler = new Mock<IVatRegistrationHandler>();
            _mockDeHandler.Setup(h => h.CountryCode).Returns("DE");
            _mockDeHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);

            var handlers = new List<IVatRegistrationHandler>
            {
                _mockUkHandler.Object,
                _mockFrHandler.Object,
                _mockDeHandler.Object
            };

            var serviceLogger = new Mock<ILogger<VatRegistrationService>>();

            var vatRegistrationService = new VatRegistrationService(handlers, serviceLogger.Object);

            _controller = new VatRegistrationController(vatRegistrationService, _mockLogger.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_ForValidRequest()
        {
            // Arrange
            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = "GB" };

            // Act
            var result = await _controller.Post(request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_ForInvalidModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("CompanyName", "Required");

            // Act
            var result = await _controller.Post(new VatRegistrationRequest());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var modelState = badRequestResult.Value as SerializableError;
            Assert.NotNull(modelState);
            Assert.True(modelState.ContainsKey("CompanyName"));
            var errors = modelState["CompanyName"] as string[];
            Assert.NotNull(errors);
            Assert.Contains("Required", errors);
        }

        [Fact]
        public async Task Post_ShouldReturnInternalServerError_OnServiceException()
        {
            // Arrange
            var mockLoggerService = new Mock<ILogger<VatRegistrationService>>();
            var mockLoggerController = new Mock<ILogger<VatRegistrationController>>();

            var mockHandler = new Mock<IVatRegistrationHandler>();
            mockHandler.Setup(h => h.CountryCode).Returns("GB");
            mockHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()))
                       .ThrowsAsync(new Exception("Test exception"));

            var handlers = new List<IVatRegistrationHandler> { mockHandler.Object };
            var vatRegistrationService = new VatRegistrationService(handlers, mockLoggerService.Object);
            var controller = new VatRegistrationController(vatRegistrationService, mockLoggerController.Object);

            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = "GB" };

            // Act
            var result = await controller.Post(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_ForUnsupportedCountry()
        {
            // Arrange
            var mockLoggerService = new Mock<ILogger<VatRegistrationService>>();
            var handlers = new List<IVatRegistrationHandler>();
            var vatRegistrationService = new VatRegistrationService(handlers, mockLoggerService.Object);

            var controllerLogger = new Mock<ILogger<VatRegistrationController>>();
            var controller = new VatRegistrationController(vatRegistrationService, controllerLogger.Object);

            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = "XZ" };

            // Act
            var result = await controller.Post(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Country not supported", badRequestResult.Value);
        }
    }
}
