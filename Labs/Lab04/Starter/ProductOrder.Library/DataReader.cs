using System.Text.Json;

namespace ProductOrder.Library;

public class DataReader
{
    protected HttpClient client =
        new HttpClient() { BaseAddress = new Uri("http://localhost:9874") };
    protected JsonSerializerOptions options =
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
}
