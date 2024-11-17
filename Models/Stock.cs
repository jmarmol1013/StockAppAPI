using Amazon.DynamoDBv2.DataModel;

namespace StockAppAPI.Models
{
    [DynamoDBTable("Stocks")]
    public class Stock
    {
        [DynamoDBHashKey("stockSymbol")]
        public string StockSymbol { get; set; }

        [DynamoDBProperty("historicalData")]
        public List<HistoricalData> HistoricalData { get; set; } = new List<HistoricalData>();
    }

    public class HistoricalData
    {
        [DynamoDBProperty("currentPrice")]
        public double CurrentPrice { get; set; }

        [DynamoDBProperty("openingPrice")]
        public double OpeningPrice { get; set; }

        [DynamoDBProperty("closingPrice")]
        public double ClosingPrice { get; set; }

        [DynamoDBProperty("highPrice")]
        public double HighPrice { get; set; }

        [DynamoDBProperty("lowPrice")]
        public double LowPrice { get; set; }

        [DynamoDBProperty("volume")]
        public double Volume { get; set; }

        [DynamoDBProperty("date")]
        public string Date { get; set; }

        [DynamoDBProperty("change")]
        public double Change { get; set; }
    }

}
