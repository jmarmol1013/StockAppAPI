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
         
            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {email} not found.");

            var stock = await _stockRepository.GetByIdAsync(stockSymbol);
            if (stock == null)
                throw new KeyNotFoundException($"Stock with symbol {stockSymbol} not found.");

            var mostRecentData = stock.HistoricalData
                .OrderByDescending(h => DateTime.Parse(h.Date)) 
                .FirstOrDefault();

            if (mostRecentData == null)
                throw new InvalidOperationException($"No historical data available for stock {stockSymbol}.");
           
            if (user.Favorites.Any(f => f.StockSymbol == stockSymbol))
                throw new InvalidOperationException($"Stock with symbol {stockSymbol} is already a favorite.");
   
            user.Favorites.Add(new FavoriteStock
            {
                StockSymbol = stockSymbol,
                CurrentPrice = mostRecentData.CurrentPrice 
            });

            await _userRepository.UpdateAsync(user);
        }
        

        public async Task<List<FavoriteStock>> GetUserFavoritesAsync(string email)
        {
           
            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found.");
            }

           
            return user.Favorites;
        }
        public async Task<UserOverviewDTO> GetUserOverviewByEmailAsync(string email)
        {
        
            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found.");
            }

            return _mapper.Map<UserOverviewDTO>(user);
        }

        public async Task DeleteFavoriteStockAsync(string email, string stockSymbol)
        {

            var user = await _userRepository.GetByIdAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found.");
            }

            var favoriteStock = user.Favorites.FirstOrDefault(f => f.StockSymbol == stockSymbol);
            if (favoriteStock == null)
            {
                throw new KeyNotFoundException($"Stock with symbol {stockSymbol} not found in user's favorites.");
            }

            user.Favorites.Remove(favoriteStock);
            await _userRepository.UpdateAsync(user);
        }

    }
}
