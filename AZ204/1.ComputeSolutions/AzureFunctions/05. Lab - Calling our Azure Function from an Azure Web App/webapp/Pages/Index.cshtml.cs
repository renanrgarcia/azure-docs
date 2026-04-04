using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using webapp;
using System.Text.Json;
using System.Net.Http;

namespace webapp.Pages.Orders;

public class IndexModel : PageModel
{
    private readonly IConfiguration _config;
     private readonly HttpClient _http;

    public IndexModel(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _http = httpClientFactory.CreateClient();
    }

    public List<CourseOrder> Orders { get; private set; } = new();

    public async Task OnGetAsync()
    {
        
    string? url = "https://func-dev-eus-01.azurewebsites.net/api/GetCourseOrders";
    using var response = await _http.GetAsync(url);
    await using var stream = await response.Content.ReadAsStreamAsync();
      var orders = await JsonSerializer.DeserializeAsync<List<CourseOrder>>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    Orders = orders ?? new List<CourseOrder>();
    }

    
}
