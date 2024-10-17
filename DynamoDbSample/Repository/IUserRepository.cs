using DynamoDbSample.Models;

namespace DynamoDbSample.Repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();

    Task<IEnumerable<User>> GetUsersByIdAsync(IEnumerable<string> ids);
    
    Task<User?> GetUserByIdAsync(string id);
    
    Task CreateUserAsync(User user);
    
    Task<bool> UpdateUserAsync(User user);
    
    Task<bool> DeleteUserByIdAsync(string id);
}