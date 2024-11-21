﻿using Amazon.DynamoDBv2.DataModel;

namespace StockAppAPI.Models
{
    [DynamoDBTable("Users")]
    public class User
    {
        [DynamoDBHashKey("email")]
        public string Email { get; set; }

        [DynamoDBProperty("admin")]
        public bool Admin { get; set; }

        [DynamoDBProperty("firstname")]
        public string FirstName { get; set; }

        [DynamoDBProperty("lastname")]
        public string LastName { get; set; }

        [DynamoDBProperty("password")]
        public string Password { get; set; }
    }
}