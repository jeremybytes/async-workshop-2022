using System.Net;
using System.Text.Json;

namespace ProductOrder.Library;

public record Customer(int Id, string CustomerName);

public class CustomerReader : DataReader
{
    public async Task<Customer> GetCustomerForOrderAsync(int orderId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"customer/fororder/{orderId}").ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<Customer>(stringResult, options);
        return result ?? new Customer(0, "");
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"customer/{customerId}").ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<Customer>(stringResult, options);
        return result ?? new Customer(0, "");
    }
}
