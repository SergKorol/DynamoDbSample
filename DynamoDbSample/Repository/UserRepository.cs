using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoDbSample.Models;

namespace DynamoDbSample.Repository;

public class UserRepository: BaseRepository ,IUserRepository
{
    private readonly IDynamoDBContext _context;
    
    public UserRepository(IAmazonDynamoDB amazonDynamoDb, IDynamoDBContext context)
        : base(amazonDynamoDb)
    {
        _context = context;
        CreateTableIfNotExists("Users").GetAwaiter().GetResult(); // Initialize table once during construction
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.ScanAsync<User>(new List<ScanCondition>()).GetRemainingAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByIdAsync(IEnumerable<string> ids)
    {
        var results = new List<User>();

        foreach (var id in ids)
        {
            var user = await _context.LoadAsync<User>(id);
            if (user != null)
            {
                results.Add(user);
            }
        }

        return results;
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _context.LoadAsync<User>(id);
    }

    public async Task CreateUserAsync(User user)
    {
        await _context.SaveAsync(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var existingUser = await GetUserByIdAsync(user.Id);
        if (existingUser == null)
        {
            return false;
        }

        await _context.SaveAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserByIdAsync(string id)
    {
        var existingUser = await GetUserByIdAsync(id);
        if (existingUser == null)
        {
            return false;
        }

        await _context.DeleteAsync<User>(id);
        return true;
    }
}

