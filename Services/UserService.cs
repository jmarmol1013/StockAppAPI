using StockAppAPI.DTO;
using StockAppAPI.Models;
using StockAppAPI.Repository;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockAppAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Stock> _stockRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository,IRepository<Stock> stockRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserOverviewDTO>> GetAllUsersAsync()
        {
            IEnumerable<User> users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserOverviewDTO>>(users);
        }

        public async Task<UserDetailDTO> GetUserByEmailAsync(string email)
        {
            User user = await _userRepository.GetByIdAsync(email);
            return _mapper.Map<UserDetailDTO>(user);
        }

        public async Task RegisterUserAsync(RegisterUserDTO registerUserDto)
        {
            User newUser = _mapper.Map<User>(registerUserDto);
            await _userRepository.AddAsync(newUser);
        }

        public async Task LoginUserAsync(LoginUserDTO loginUserDto)
        {
            User user = await _userRepository.GetByIdAsync(loginUserDto.Email);
            if (user == null || user.Password != loginUserDto.Password)
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }
        }
        public async Task DeleteUserAsync(string email)
        {

            User user = await _userRepository.GetByIdAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found.");
            }

            await _userRepository.DeleteAsync(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task AddFavoriteStockAsync(string email, string stockSymbol)
        {
            // Retrieve the user
            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found.");

            // Retrieve the stock
            var stock = await _stockRepository.GetByIdAsync(stockSymbol);
            if (stock == null)
                throw new KeyNotFoundException($"Stock with symbol {stockSymbol} not found.");

            // Extract the most recent current price from historical data
            var mostRecentData = stock.HistoricalData
                .OrderByDescending(h => DateTime.Parse(h.Date)) // Ensure correct ordering
                .FirstOrDefault();

            if (mostRecentData == null)
                throw new InvalidOperationException($"No historical data available for stock {stockSymbol}.");

            // Check if the stock is already in the user's favorites
            if (user.Favorites.Any(f => f.StockSymbol == stockSymbol))
                throw new InvalidOperationException($"Stock with symbol {stockSymbol} is already a favorite.");

            // Add to favorites
            user.Favorites.Add(new FavoriteStock
            {
                StockSymbol = stockSymbol,
                CurrentPrice = mostRecentData.CurrentPrice // Use the most recent current price
            });

            // Save user back to the repository
            await _userRepository.UpdateAsync(user);
        }

    }
}
