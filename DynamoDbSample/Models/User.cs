using Amazon.DynamoDBv2.DataModel;

namespace DynamoDbSample.Models;

[DynamoDBTable("Users")]
public class User
{
    [DynamoDBHashKey]
    public required string Id { get; set; }
    [DynamoDBProperty]
    public required string Name { get; set; }
}