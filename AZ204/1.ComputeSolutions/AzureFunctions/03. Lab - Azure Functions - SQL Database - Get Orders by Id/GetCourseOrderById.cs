using System.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Web;

public class GetCourseOrderById
{
    private readonly ILogger<GetCourseOrderById> _logger;

    public GetCourseOrderById(ILogger<GetCourseOrderById> logger)
    {
        _logger = logger;
    }

[Function("GetCourseOrderById")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("Fetching a course order by OrderId...");

        var query = HttpUtility.ParseQueryString(req.Url.Query);
        string? orderIdRaw = query.Get("orderId");

        string? connString = Environment.GetEnvironmentVariable("SqlConnectionString");

        const string sql = @"
            SELECT
                OrderId, CustomerName, CustomerEmail, CourseName, Amount, OrderDateUtc
            FROM dbo.CourseOrders
            WHERE OrderId = @OrderId;";

        await using var conn = new SqlConnection(connString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderIdRaw;

        await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);
        if (!await reader.ReadAsync())
{
    var notFound = req.CreateResponse(HttpStatusCode.NotFound);
    await notFound.WriteStringAsync($"No order found for OrderId = {orderIdRaw}");
    return notFound;
}
         var order = new
        {
            OrderId = reader.GetInt32(0),
            CustomerName = reader.GetString(1),
            CustomerEmail = reader.GetString(2),
            CourseName = reader.GetString(3),
            Amount = reader.GetDecimal(4),
            OrderDateUtc = reader.GetDateTime(5)
        };

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(order);
        return ok;

    }
    
    
     

}

