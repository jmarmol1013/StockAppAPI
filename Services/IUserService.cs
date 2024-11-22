using System.Collections.Generic;
using System.Threading.Tasks;
using StockAppAPI.DTO;
using StockAppAPI.Models;

namespace StockAppAPI.Services
{
    public interface IUserService
    {
        Task<UserDetailDTO> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserOverviewDTO>> GetAllUsersAsync();
        Task RegisterUserAsync(RegisterUserDTO registerUserDto);
        Task LoginUserAsync(LoginUserDTO loginUserDto);
        Task DeleteUserAsync(string email);        
        Task UpdateUserAsync(User user);
        Task AddFavoriteStockAsync(string email, string stockSymbol);
    }
}
