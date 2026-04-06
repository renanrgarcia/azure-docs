namespace webapp;
public class CourseOrder
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime OrderDateUtc { get; set; }
}