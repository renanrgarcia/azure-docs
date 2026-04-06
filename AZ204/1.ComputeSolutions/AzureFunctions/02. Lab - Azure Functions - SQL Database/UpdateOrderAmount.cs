using System.Data;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace _02._Lab___Azure_Functions___SQL_Database;

public class UpdateOrderAmount(ILogger<UpdateOrderAmount> logger)
{
    private sealed record UpdateAmountRequest(int OrderId, decimal Amount);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    [Function("UpdateOrderAmount")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        logger.LogInformation("Updating order amount...");

        string? connString = Environment.GetEnvironmentVariable("SqlConnectionString");

        UpdateAmountRequest? payload;
        payload = await JsonSerializer.DeserializeAsync<UpdateAmountRequest>(req.Body, JsonOptions);

        const string sql = @"
            UPDATE dbo.CourseOrders
            SET Amount = @Amount
            WHERE OrderId = @OrderId;";

        await using var conn = new SqlConnection(connString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = payload?.Amount;
        cmd.Parameters["@Amount"].Precision = 10;
        cmd.Parameters["@Amount"].Scale = 2;

        cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = payload?.OrderId;

        int rows = await cmd.ExecuteNonQueryAsync();
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            message = "Order amount updated",
            orderId = payload?.OrderId,
            amount = payload?.Amount
        });

        return response;
    }
}