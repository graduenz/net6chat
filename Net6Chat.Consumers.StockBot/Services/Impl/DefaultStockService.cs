using Net6Chat.Consumers.StockBot.Csv;
using Net6Chat.Consumers.StockBot.DTO;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Net6Chat.Consumers.StockBot.Services.Impl
{
    public class DefaultStockService : IStockService
    {
        public const string STOCK_URL = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

        private readonly HttpClient _httpClient;

        public DefaultStockService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<GetStockQuotationResult> GetStockQuotationAsync(string stock)
        {
            var url = string.Format(STOCK_URL, stock);
            using (var stream = await _httpClient.GetStreamAsync(url))
            {
                StreamReader reader = new StreamReader(stream);
                var csvText = reader.ReadToEnd();
                var parseResult = ParseCsv(csvText);

                if (parseResult == null || parseResult.Result.Close == "N/D")
                    return new GetStockQuotationResult() {
                        Success = false,
                        Error = $"No stock data was found for '{stock}'",
                    };

                return new GetStockQuotationResult() {
                    Success = true,
                    Data = parseResult.Result,
                };
            }
        }

        private CsvMappingResult<StockCsvDto>? ParseCsv(string csvText)
        {
            var csvParser = new CsvParser<StockCsvDto>(
                options: new CsvParserOptions(true, ','),
                mapping: new StockCsvDtoMapping()
            );

            var csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine });

            return csvParser.ReadFromString(csvReaderOptions, csvText).FirstOrDefault();
        }
    }
}
