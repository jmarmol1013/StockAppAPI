using AutoMapper;
using StockAppAPI.DTO;
using StockAppAPI.Models;
using StockAppAPI.Repository;

namespace StockAppAPI.Services
{
    public class StockService : IStockService
    {
        private readonly IRepository<Stock> _stockRepository;
        private readonly IMapper _mapper;

        public StockService(IRepository<Stock> stockRepository, IMapper mapper) 
        {
            _stockRepository = stockRepository;
            _mapper = mapper;

        }
        public async Task AddAsync(AddStockDto addStockDto)
        {
            HistoricalData historicalData = _mapper.Map<HistoricalData>(addStockDto.HistoricalData);
            historicalData.Date = DateTime.Now.ToString("yyyy-MM-dd");
            historicalData.Change = 0;
            string stockId = addStockDto.StockSymbol;

            Stock newStock = new Stock
            {
                StockSymbol = stockId,
                HistoricalData = new List<HistoricalData> { historicalData }
            };

            await _stockRepository.AddAsync(newStock);
        }

        public async Task DeleteAsync(string id)
        {
            await _stockRepository.DeleteAsync(id);
        }

        public async Task<StocksOverviewDTO> GetAllAsync()
        {
            IEnumerable<Stock> stocks = await _stockRepository.GetAllAsync();

            IEnumerable<OverviewDataDTO> overviewData = _mapper.Map<IEnumerable<OverviewDataDTO>>(stocks);

            StocksOverviewDTO stocksOverviewDTO = new StocksOverviewDTO();
            stocksOverviewDTO.StocksData = overviewData.ToList();

            return stocksOverviewDTO;
        }

        public async Task<StockDetailDTO> GetStockByIdAsync(string id)
        {
            Stock stock = await _stockRepository.GetByIdAsync(id);

            StockDetailDTO stockDetail = _mapper.Map<StockDetailDTO>(stock);
            return stockDetail;
        }

        public async Task UpdateAsync(string stockId, UpdateStockDTO updateStockDto)
        {
            HistoricalData historicalData = _mapper.Map<HistoricalData>(updateStockDto);
            Stock stock = await _stockRepository.GetByIdAsync(stockId);

            // Check if stock historical is not empty, to update change field
            HistoricalData lastHistoricalData = stock.HistoricalData.LastOrDefault();
            if (lastHistoricalData != null)
            {
                historicalData.Change = historicalData.CurrentPrice - lastHistoricalData.CurrentPrice;
            }
            else
            {
                historicalData.Change = 0;
            }

            historicalData.Date = DateTime.Now.ToString("yyyy-MM-dd");

            stock.HistoricalData.Add(historicalData);

            await _stockRepository.UpdateAsync(stock);
        }
    }
}
