using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace _02._Lab___Azure_Functions___SQL_Database___Get_Orders;

public class GetCourseOrders(ILogger<GetCourseOrders> logger)
{
    [Function("GetCourseOrders")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        logger.LogInformation("Fetching course orders from the database.");
        string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

        var orders = new List<object>();

        const string query = @"
            SELECT 
                OrderId, CustomerName, CustomerEmail, CourseName, Amount, OrderDateUtc
            FROM dbo.Orders
            ORDER BY OrderDateUtc DESC;";

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

        while (await reader.ReadAsync())
        {
            orders.Add(new
            {
                OrderId = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                CustomerEmail = reader.GetString(2),
                CourseName = reader.GetString(3),
                Amount = reader.GetDecimal(4),
                OrderDateUtc = reader.GetDateTime(5)
            });
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(orders);
        return response;
    }
}