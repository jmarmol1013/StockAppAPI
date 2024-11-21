using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using StockAppAPI.Models;
using StockAppAPI.Repository;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using ConfigurationManager = System.Configuration.ConfigurationManager;
using Amazon;
using StockAppAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string accessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
string secretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
string region = ConfigurationManager.AppSettings["AWSRegion"];

AWSOptions awsOptions = new AWSOptions
{
    Credentials = new BasicAWSCredentials(accessKey, secretKey),
    Region = RegionEndpoint.GetBySystemName(region)
};

// Add AWS options to services
builder.Services.AddDefaultAWSOptions(awsOptions);

// Register DynamoDB service with the configured AWS options
builder.Services.AddAWSService<IAmazonDynamoDB>();

// Add scoped services
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IRepository<Stock>, DynamoDBRepository<Stock>>();
builder.Services.AddScoped<IStockService, StockService>();

builder.Services.AddScoped<IRepository<User>, DynamoDBRepository<User>>(); // Adjust if using a custom repository
builder.Services.AddScoped<IUserService, UserService>();
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
