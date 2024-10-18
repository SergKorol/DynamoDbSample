using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DynamoDbSample.Repository;

public class BaseRepository
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly ILogger<BaseRepository> _logger;

    protected BaseRepository(IAmazonDynamoDB amazonDynamoDb, ILogger<BaseRepository> logger)
    {
        _amazonDynamoDb = amazonDynamoDb;
        _logger = logger;
    }

    protected async Task CreateTableIfNotExists(string tableName)
    {
        try
        {
            var tableResponse = await _amazonDynamoDb.DescribeTableAsync(tableName);
            if (tableResponse.Table.TableStatus != TableStatus.ACTIVE)
            {
                _logger.LogInformation("Creating table {TableName}", tableName);
            }
        }
        catch (ResourceNotFoundException)
        {
            var request = GetCreateTableRequest(tableName);
            await _amazonDynamoDb.CreateTableAsync(request);
        }
    }

    private CreateTableRequest GetCreateTableRequest(string tableName)
    {
        return new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new("Id", ScalarAttributeType.S)
            },
            KeySchema = new List<KeySchemaElement>
            {
                new("Id", KeyType.HASH)
            },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };
    }
}