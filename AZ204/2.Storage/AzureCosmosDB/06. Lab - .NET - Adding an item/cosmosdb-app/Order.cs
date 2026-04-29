using Newtonsoft.Json;

public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; } = default!;

    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = default!;

    [JsonProperty("customerName")]
    public string CustomerName { get; set; } = default!;

    [JsonProperty("courseId")]
    public string CourseId { get; set; } = default!;

    [JsonProperty("courseName")]
    public string CourseName { get; set; } = default!;

    [JsonProperty("orderDateUtc")]
    public DateTime OrderDateUtc { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; } = default!;

    [JsonProperty("status")]
    public string Status { get; set; } = default!;
}