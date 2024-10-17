using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Threading.Tasks;

public class BaseRepository
{
    private readonly IAmazonDynamoDB _amazonDynamoDB;

    // Inject the IAmazonDynamoDB client via the constructor
    public BaseRepository(IAmazonDynamoDB amazonDynamoDB)
    {
        _amazonDynamoDB = amazonDynamoDB;
    }

    // Method to create the table if it doesn't exist
    protected async Task CreateTableIfNotExists(string tableName)
    {
        try
        {
            // Describe the table. If it exists, it will not throw an exception.
            var tableResponse = await _amazonDynamoDB.DescribeTableAsync(tableName);
            if (tableResponse.Table.TableStatus != TableStatus.ACTIVE)
            {
                // Optionally handle the case where the table exists but is not active
                Console.WriteLine("Table exists but is not active yet.");
            }
        }
        catch (ResourceNotFoundException)
        {
            // If the table does not exist, create it.
            var request = GetCreateTableRequest(tableName);
            await _amazonDynamoDB.CreateTableAsync(request);
        }
    }

    private CreateTableRequest GetCreateTableRequest(string tableName)
    {
        return new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("Id", ScalarAttributeType.S)
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("Id", KeyType.HASH)
            },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };
    }
}