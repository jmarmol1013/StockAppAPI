using Microsoft.AspNetCore.Mvc;
using StockAppAPI.DTO;
using StockAppAPI.Services;

namespace StockAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : Controller
    {
        private readonly ILogger<StocksController> _logger;
        private readonly IStockService _stockService;

        public StocksController(ILogger<StocksController> logger, IStockService stockService)
        {
            _logger = logger;
            _stockService = stockService;
        }

        // POST: api/stocks/Stock
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddStock([FromBody] AddStockDto addStockDto)
        {
            if (addStockDto == null)
                return BadRequest("Stock data is required.");

            await _stockService.AddAsync(addStockDto);
            return CreatedAtAction(nameof(GetStockById), new { id = addStockDto.StockSymbol }, addStockDto);
        }

        // DELETE: api/stocks/Stock/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteStock(string id)
        {
            StockDetailDTO stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            await _stockService.DeleteAsync(id);
            return Ok();
        }

        // GET: api/stocks/Stock
        [HttpGet]
        [ProducesResponseType(typeof(StocksOverviewDTO), 200)]
        public async Task<IActionResult> GetAllStocks()
        {
            StocksOverviewDTO stocksOverview = await _stockService.GetAllAsync();
            return Ok(stocksOverview);
        }

        // GET: api/stocks/Stock/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StockDetailDTO), 200)]
        public async Task<IActionResult> GetStockById(string id)
        {
            StockDetailDTO stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            return Ok(stock);
        }

        // PUT: api/stocks/Stock/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateStock(string id, [FromBody] UpdateStockDTO updateStockDto)
        {
            if (updateStockDto == null)
                return BadRequest("Stock update data is required.");

            StockDetailDTO existingStock = await _stockService.GetStockByIdAsync(id);
            if (existingStock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            await _stockService.UpdateAsync(id, updateStockDto);
            return Ok();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PatchStock(string id, [FromBody] UpdateStockDTO updateStockDto)
        {
            if (updateStockDto == null)
                return BadRequest("Stock update data is required.");

            var existingStock = await _stockService.GetStockByIdAsync(id);
            if (existingStock == null)
            {
                return NotFound($"Stock with ID {id} not found.");
            }

            await _stockService.UpdateAsync(id, updateStockDto);

            return Ok();
        }
    }
}
