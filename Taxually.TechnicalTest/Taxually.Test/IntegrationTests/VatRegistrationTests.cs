using Taxually.TechnicalTest.Services;
using Moq;
using Taxually.TechnicalTest.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Taxually.TechnicalTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace Taxually.Test.IntegrationTests
{
    public class VatRegistrationTests
    {
        private readonly VatRegistrationService _vatRegistrationService;
        private readonly Mock<ILogger<VatRegistrationService>> _mockLogger;
        private readonly Mock<IVatRegistrationHandler> _mockUkHandler;
        private readonly Mock<IVatRegistrationHandler> _mockFrHandler;
        private readonly Mock<IVatRegistrationHandler> _mockDeHandler;

        public VatRegistrationTests()
        {
            _mockLogger = new Mock<ILogger<VatRegistrationService>>();
            _mockUkHandler = new Mock<IVatRegistrationHandler>();
            _mockFrHandler = new Mock<IVatRegistrationHandler>();
            _mockDeHandler = new Mock<IVatRegistrationHandler>();

            _mockUkHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            _mockFrHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);
            _mockDeHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).Returns(Task.CompletedTask);

            _mockUkHandler.Setup(h => h.CountryCode).Returns("GB");
            _mockFrHandler.Setup(h => h.CountryCode).Returns("FR");
            _mockDeHandler.Setup(h => h.CountryCode).Returns("DE");

            var handlers = new List<IVatRegistrationHandler>
        {
            _mockUkHandler.Object,
            _mockFrHandler.Object,
            _mockDeHandler.Object
        };

            _vatRegistrationService = new VatRegistrationService(handlers, _mockLogger.Object);
        }

        [Theory]
        [InlineData("GB", "UkHandler")]
        [InlineData("FR", "FrHandler")]
        [InlineData("DE", "DeHandler")]
        public async Task RegisterVatAsync_ShouldCallCorrectHandler_ForGivenCountry(string country, string expectedHandler)
        {
            // Arrange
            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = country };

            // Act
            var result = await _vatRegistrationService.RegisterVatAsync(request);

            // Assert
            Assert.IsType<OkResult>(result);

            switch (expectedHandler)
            {
                case "UkHandler":
                    _mockUkHandler.Verify(h => h.HandleAsync(request), Times.Once);
                    _mockFrHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    _mockDeHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    break;
                case "FrHandler":
                    _mockFrHandler.Verify(h => h.HandleAsync(request), Times.Once);
                    _mockUkHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    _mockDeHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    break;
                case "DeHandler":
                    _mockDeHandler.Verify(h => h.HandleAsync(request), Times.Once);
                    _mockUkHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    _mockFrHandler.Verify(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>()), Times.Never);
                    break;
            }
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldReturnBadRequest_ForUnsupportedCountry()
        {
            // Arrange
            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = "XZ" };

            // Act
            var result = await _vatRegistrationService.RegisterVatAsync(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Country not supported", badRequestResult.Value);
        }

        [Fact]
        public async Task RegisterVatAsync_ShouldReturnInternalServerError_OnHandlerException()
        {
            // Arrange
            var request = new VatRegistrationRequest { CompanyName = "TestCompany", CompanyId = "12345", Country = "GB" };
            _mockUkHandler.Setup(h => h.HandleAsync(It.IsAny<VatRegistrationRequest>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _vatRegistrationService.RegisterVatAsync(request);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            _mockUkHandler.Verify(h => h.HandleAsync(request), Times.Once);
        }
    }
}
