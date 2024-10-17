using DynamoDbSample.Models;
using DynamoDbSample.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDbSample.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userRepository.GetAllUsersAsync();

        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
    
        return Ok(user);
    }
    
    [HttpGet("multiple")]
    public async Task<IActionResult> GetUsersByIds([FromBody]IEnumerable<string> ids)
    {
        var users = await userRepository.GetUsersByIdAsync(ids);
    
        return Ok(users);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody]User user)
    {
        await userRepository.CreateUserAsync(user);
        return Ok();
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromBody]User user)
    {
        var result = await userRepository.UpdateUserAsync(user);
        return result ? Ok() : BadRequest();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await userRepository.DeleteUserByIdAsync(id);
        return result ? Ok() : BadRequest();
    }
}