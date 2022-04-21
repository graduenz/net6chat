using Net6Chat.Consumers.StockBot.DTO;

namespace Net6Chat.Consumers.StockBot.Services
{
    public interface IStockService
    {
        Task<GetStockQuotationResult> GetStockQuotationAsync(string stock);
    }
}
