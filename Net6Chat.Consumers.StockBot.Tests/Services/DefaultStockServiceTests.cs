using FluentAssertions;
using Net6Chat.Consumers.StockBot.Services.Impl;
using System.Threading.Tasks;
using Xunit;

namespace Net6Chat.Consumers.StockBot.Tests.Services
{
    public class DefaultStockServiceTests
    {
        private readonly DefaultStockService _service;

        public DefaultStockServiceTests()
        {
            _service = new DefaultStockService();
        }

        [Theory(DisplayName = "Should get valid stock quotation successfully")]
        [InlineData("AAPL.US")]
        [InlineData("AMZN.US")]
        public async Task GetStockQuotationAsync_WhenStockExists_ReturnsSuccessfully(string stock)
        {
            // Act
            var result = await _service.GetStockQuotationAsync(stock);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data?.Close.Should().NotBe("N/D");
        }

        [Fact(DisplayName = "Should return a failure when getting invalid stock quotation")]
        public async Task GetStockQuotationAsync_WhenStockDoesNotExist_ReturnsFailure()
        {
            // Act
            var result = await _service.GetStockQuotationAsync("INVALID");

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Error.Should().Be($"No stock data was found for 'INVALID'");
        }
    }
}