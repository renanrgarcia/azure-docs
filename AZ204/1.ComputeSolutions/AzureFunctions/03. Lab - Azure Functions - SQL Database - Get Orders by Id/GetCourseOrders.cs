using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class GetCourseOrders
{
    private readonly ILogger<GetCourseOrders> _logger;

    public GetCourseOrders(ILogger<GetCourseOrders> logger)
    {
        _logger = logger;
    }

    [Function("GetCourseOrders")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
         _logger.LogInformation("Fetching course orders from Azure SQL...");
         string? connString = Environment.GetEnvironmentVariable("SqlConnectionString");

         var orders = new List<object>();

         const string sql = @"
            SELECT 
                OrderId, CustomerName, CustomerEmail, CourseName, Amount, OrderDateUtc
            FROM dbo.CourseOrders
            ORDER BY OrderDateUtc DESC;";

            await using var conn = new SqlConnection(connString);
            await conn.OpenAsync();

              await using var cmd = new SqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

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

        var res = req.CreateResponse(HttpStatusCode.OK);
        await res.WriteAsJsonAsync(orders);
        return res;

    }
}