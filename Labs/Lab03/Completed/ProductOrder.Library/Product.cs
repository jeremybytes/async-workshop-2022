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

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<List<Product>>(stringResult, options)!;
        }
        return new List<Product>();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"product/{productId}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<Product>(stringResult, options)!;
        }
        return new Product(0, "", "");
    }
}
