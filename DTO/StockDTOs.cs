using Amazon.DynamoDBv2.DataModel;

namespace StockAppAPI.DTO
{
    public class StocksOverviewDTO
    {
        public List<OverviewDataDTO> StocksData { get; set; } = new List<OverviewDataDTO>();
    }

    public class OverviewDataDTO
    {
        public string StockSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public string Date { get; set; }
    }

    public class AddStockDto
    {
        public string StockSymbol { get; set; }

        public HistoricalDataDTO HistoricalData { get; set; }
    }

    public class UpdateStockDTO
    {
        public double CurrentPrice { get; set; }
        public double OpeningPrice { get; set; }
        public double ClosingPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double Volume { get; set; }
    }

    public class StockDetailDTO
    {
        public string StockSymbol { get; set; }
        public List<HistoricalDataDTO> StockHistory { get; set;} = new List<HistoricalDataDTO>();
    }

    public class HistoricalDataDTO
    {
        public double CurrentPrice { get; set; }
        public double OpeningPrice { get; set; }
        public double ClosingPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double Volume { get; set; }
        public string Date { get; set; }
        public double Change { get; set; }
    }


}
