using Net6Chat.Consumers.StockBot.DTO;
using TinyCsvParser.Mapping;

namespace Net6Chat.Consumers.StockBot.Csv
{
    public class StockCsvDtoMapping : CsvMapping<StockCsvDto>
    {
        public StockCsvDtoMapping()
        {
            MapProperty(0, m => m.Symbol);
            MapProperty(1, m => m.Date);
            MapProperty(2, m => m.Time);
            MapProperty(3, m => m.Open);
            MapProperty(4, m => m.High);
            MapProperty(5, m => m.Low);
            MapProperty(6, m => m.Close);
            MapProperty(7, m => m.Volume);
        }
    }
}
