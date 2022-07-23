using System.Net;
using System.Text.Json;

namespace ProductOrder.Library;

public record Product(int Id, string ProductName, string ProductDescription);

public class ProductReader : DataReader
{
    public async Task<List<Product>> GetProductsForOrderAsync(int orderId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"product/fororder/{orderId}").ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<List<Product>>(stringResult, options);
        return result ?? new List<Product>();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"product/{productId}").ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<Product>(stringResult, options);
        return result ?? new Product(0, "", "");
    }
}
