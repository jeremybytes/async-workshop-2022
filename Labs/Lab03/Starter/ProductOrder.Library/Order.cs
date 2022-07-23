using System.Net;
using System.Text.Json;

namespace ProductOrder.Library;

public class Order
{
    public int Id { get; set; }
    public DateTime DateOrdered { get; set; }
    public DateTime DateFulfilled { get; set; }
    public Customer? Customer { get; set; }
    public List<Product>? Products { get; set; }
}

public class OrderReader : DataReader
{
    private IExceptionLogger logger;
    private CustomerReader customerReader;
    private ProductReader productReader;

    public OrderReader(IExceptionLogger logger)
    {
        this.logger = logger;
        customerReader = new CustomerReader();
        productReader = new ProductReader();
    }

    public Task<Order> GetOrderAsync(int orderId)
    {
        var order = new Order();
        try
        {
            throw new NotImplementedException("This has not been implemented yet");
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        return Task.FromResult<Order>(order);
    }

    private async Task<Order> GetOrderDetailsAsync(int orderId)
    {
        HttpResponseMessage response =
            await client.GetAsync($"order/{orderId}").ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Unable to complete request: status code {response.StatusCode}");

        var stringResult =
            await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var result = JsonSerializer.Deserialize<Order>(stringResult, options);
        return result ?? new Order();
    }
}
