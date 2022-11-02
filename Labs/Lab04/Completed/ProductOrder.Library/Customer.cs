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

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<Customer>(stringResult, options)!;
        }
        return new Customer(0, "");
    }

    public async Task<Customer> GetCustomerAsync(int customerId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"customer/{customerId}").ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var stringResult =
                await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<Customer>(stringResult, options)!;
        }
        return new Customer(0, "");
    }
}
