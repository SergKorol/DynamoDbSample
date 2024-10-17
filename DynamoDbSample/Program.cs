using DynamoDbSample.Configuration;
using DynamoDbSample.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddDynamoDb(builder.Configuration);
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // Set the Swagger endpoint
        c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();