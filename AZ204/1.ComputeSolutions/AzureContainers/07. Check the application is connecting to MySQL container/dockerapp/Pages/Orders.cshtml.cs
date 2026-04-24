using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;

public class OrdersModel : PageModel
{
    public List<CourseOrder> Orders { get; private set; } = new();

    public async Task OnGetAsync()
    {
        // Read connection string from environment variable (best for containers)
        var connStr =
            "Server=localhost;Port=3306;Database=cloudxeusdb;User ID=appuser;Password=StrongPassword!123;";

        await using var conn = new MySqlConnection(connStr);
        await conn.OpenAsync();

        const string sql = @"
            SELECT OrderId, CustomerName, CustomerEmail, CourseName, Amount, OrderDateUtc
            FROM CourseOrders
            ORDER BY OrderId DESC
            LIMIT 10;";

        await using var cmd = new MySqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Orders.Add(new CourseOrder
            {
                OrderId = reader.GetInt32("OrderId"),
                CustomerName = reader.GetString("CustomerName"),
                CustomerEmail = reader.GetString("CustomerEmail"),
                CourseName = reader.GetString("CourseName"),
                Amount = reader.GetDecimal("Amount"),
                OrderDateUtc = reader.GetDateTime("OrderDateUtc")
            });
        }
    }

    public class CourseOrder
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public string CourseName { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime OrderDateUtc { get; set; }
    }
}