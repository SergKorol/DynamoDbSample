using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace DynamoDbSample.Configuration;

public static class ConfigurationDynamoDb
{
    public static void AddDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAmazonDynamoDB>(_ =>
        {
            var options = configuration.GetSection("DynamoDb");
            var credentials = new BasicAWSCredentials(options["AccessKey"], options["SecretKey"]);

            var config = new AmazonDynamoDBConfig
            {
                ServiceURL = options["ServiceUrl"],
            };

            return new AmazonDynamoDBClient(credentials, config);
        });
        services.AddSingleton<IDynamoDBContextBuilder>(provider =>
        {
            var clientFactory = new Func<IAmazonDynamoDB>(provider.GetRequiredService<IAmazonDynamoDB>);
            return new DynamoDBContextBuilder().WithDynamoDBClient(clientFactory);
        });

        services.AddSingleton<IDynamoDBContext>(provider =>
        {
            var builder = provider.GetRequiredService<IDynamoDBContextBuilder>();
            return builder.Build();
        });
    }
}