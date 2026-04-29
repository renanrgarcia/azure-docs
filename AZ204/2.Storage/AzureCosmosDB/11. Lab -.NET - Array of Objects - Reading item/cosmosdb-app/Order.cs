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

     [JsonProperty("course")]
    public CourseInfo Course { get; set; } = default!;

    [JsonProperty("payment")]
    public PaymentInfo Payment { get; set; } = default!;
}

public class CourseInfo
{
    [JsonProperty("courseId")]
    public string CourseId { get; set; } = default!;

    [JsonProperty("courseName")]
    public string CourseName { get; set; } = default!;

    [JsonProperty("category")]
    public string Category { get; set; } = default!;
}

public class PaymentInfo
{
    [JsonProperty("method")]
    public string Method { get; set; } = default!;

    [JsonProperty("provider")]
    public string Provider { get; set; } = default!;

    [JsonProperty("transactionId")]
    public string TransactionId { get; set; } = default!;

    [JsonProperty("paidAmount")]
    public decimal PaidAmount { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; } = default!;

    [JsonProperty("failureReason")]
    public string? FailureReason { get; set; }
}