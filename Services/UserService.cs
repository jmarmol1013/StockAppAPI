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
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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

    }
}
