namespace Net6Chat.Consumers.StockBot.DTO
{
    public class GetStockQuotationResult
    {
        public bool Success { get; set; }
        public StockCsvDto? Data { get; set; }
        public string? Error { get; set; }
    }
}
