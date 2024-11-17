using StockAppAPI.DTO;

namespace StockAppAPI.Services
{
    public interface IStockService
    {
        Task<StockDetailDTO> GetStockByIdAsync(string id);
        Task<StocksOverviewDTO> GetAllAsync();
        Task AddAsync(AddStockDto updateStockDto);
        Task UpdateAsync(string stockId, UpdateStockDTO updateStockDto);
        Task DeleteAsync(string id);
    }
}
