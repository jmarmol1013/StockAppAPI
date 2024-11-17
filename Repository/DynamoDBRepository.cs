
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using StockAppAPI.Models;

namespace StockAppAPI.Repository
{
    public class DynamoDBRepository<T> : IRepository<T> where T : class
    {
        private readonly IDynamoDBContext _context;

        public DynamoDBRepository(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task AddAsync(T entity)
        {
            await _context.SaveAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            var data = await GetByIdAsync(id);
            if (data != null)
            {
                await _context.DeleteAsync(data);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var search = _context.ScanAsync<T>(new List<ScanCondition>());
            var data = await search.GetRemainingAsync();
            return data;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _context.LoadAsync<T>(id);
        }

        public async Task UpdateAsync(T entity)
        {
            await _context.SaveAsync(entity);
        }
    }
}
