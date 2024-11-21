using Microsoft.AspNetCore.Mvc;
using StockAppAPI.Services;
using StockAppAPI.DTO;
using System.Threading.Tasks;
using StockAppAPI.Models;
using AutoMapper;

namespace StockAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper; 

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // POST: api/users/register
        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUserDto)
        {
            if (registerUserDto == null)
                return BadRequest("User data is required.");

            await _userService.RegisterUserAsync(registerUserDto);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = registerUserDto.Email }, registerUserDto);
        }

        // POST: api/users/login
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUserDto)
        {
            if (loginUserDto == null)
                return BadRequest("Login data is required.");

            try
            {
                await _userService.LoginUserAsync(loginUserDto);
                return Ok("Login successful.");
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid email or password.");
            }
        }

        // GET: api/users
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserOverviewDTO>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/{email}
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(UserDetailDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email {email} not found.");
            }

            return Ok(user);
        }

        // DELETE: api/users/{email}
        [HttpDelete("{email}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                await _userService.DeleteUserAsync(email);  
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with email {email} not found.");
            }
        }

        // PUT: api/user/{email}
        [HttpPut("{email}")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(404)]  // Not found if user doesn't exist
        public async Task<IActionResult> UpdateUser(string email, [FromBody] UserUpdateDTO userUpdateDto)
        {
            // Get the user by email
            UserDetailDTO userToUpdate = await _userService.GetUserByEmailAsync(email);
            if (userToUpdate == null)
            {
                return NotFound($"User with email {email} not found.");
            }

            // Map the update details from the DTO to the User model
            User userModel = _mapper.Map<User>(userToUpdate); // Convert DTO to User model
            userModel.FirstName = userUpdateDto.FirstName;
            userModel.LastName = userUpdateDto.LastName;

            // Update the user in the repository
            await _userService.UpdateUserAsync(userModel);

            return Ok("User updated successfully.");
        }


    }

}
