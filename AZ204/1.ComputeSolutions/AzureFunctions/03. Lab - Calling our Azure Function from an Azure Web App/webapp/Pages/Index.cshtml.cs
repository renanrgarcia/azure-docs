using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using webapp;
using System.Text.Json;
using System.Net.Http;

namespace webapp.Pages.Orders;

public class IndexModel(IConfiguration config, IHttpClientFactory httpClientFactory) : PageModel
{
    private readonly HttpClient http = httpClientFactory.CreateClient();
    private readonly JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web);

    public List<CourseOrder> Orders { get; private set; } = [];

    public async Task OnGetAsync()
    {

        string? url = "https://my-func321654.azurewebsites.net/api/GetCourseOrders";
        using var response = await http.GetAsync(url);
        await using var stream = await response.Content.ReadAsStreamAsync();
        var orders = await JsonSerializer.DeserializeAsync<List<CourseOrder>>(
              stream,
              jsonOptions);
        Orders = orders ?? [];
    }
}
