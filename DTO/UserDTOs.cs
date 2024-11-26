using Amazon.DynamoDBv2.DataModel;
using StockAppAPI.Models;
namespace StockAppAPI.DTO
{
    public class RegisterUserDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class LoginUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public string Email { get; set; }
        public bool Admin { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class UserOverviewDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public List<FavoriteStock> Favorites { get; set; } = new List<FavoriteStock>();

    }

    public class UserDetailDTO
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public string Password {  get; set; }
        public List<FavoriteStock> Favorites { get; set; } = new List<FavoriteStock>();

    }

    public class UserUpdateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddFavoriteDTO
    {
        public string StockSymbol { get; set; }
    }

}
