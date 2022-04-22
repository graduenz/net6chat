using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net6Chat.Consumers.StockBot.Services;
using Net6Chat.Domain.DTO;

namespace Net6Chat.Consumers.StockBot
{
    [CapSubscribe("bots.stock")]
    public class StockBotSub : ICapSubscribe
    {
        private readonly ILogger<StockBotSub> _logger;
        private readonly IConfiguration _configuration;
        private readonly IStockService _stockService;

        public StockBotSub(ILogger<StockBotSub> logger, IConfiguration configuration, IStockService stockService)
        {
            _logger = logger;
            _configuration = configuration;
            _stockService = stockService;
        }

        [CapSubscribe("get", isPartial: true)]
        public async Task GetStockQuotation(GetStockDto data)
        {
            _logger.LogInformation($"Getting stock quotation for {data.Stock}");

            if (string.IsNullOrEmpty(data.Stock)) return;

            var stockQuotation = await _stockService.GetStockQuotationAsync(data.Stock);

            _logger.LogInformation($"Successful stock quotation for {data.Stock}");

            var hubUrl = string.Format(_configuration["ExternalServices:ChatHubUrl"], data.Room);
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(new Uri(hubUrl))
                .WithAutomaticReconnect()
                .Build();

            try
            {
                await connection.StartAsync();
            }
            catch
            {
                _logger.LogError($"Failure trying to connect to chat hub at {hubUrl}");
                throw;
            }

            var botResponse = stockQuotation.Success
                ? $"{data.Stock} quote is ${stockQuotation.Data?.Close:C2} per share"
                : stockQuotation.Error;

            await connection.InvokeAsync("StockBotResponse", $"{data.ConnectionId}", botResponse);

            await connection.StopAsync();
        }
    }
}
