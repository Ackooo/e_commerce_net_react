namespace Benchmark.Benchmarks;

using BenchmarkDotNet.Attributes;
using Domain.Entities.Order;
using Domain.Entities.User;
using Domain.Shared.Enums;
using Infrastructure.Excel;

[MemoryDiagnoser]
[RankColumn]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]

//Debug
//[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
//[SimpleJob(RuntimeMoniker.Net90)] 
//[InProcess]

public class ExcelExportBenchmark
{
    private const int NumberOfOrders = 50_000;
    private readonly List<Order> _data;
    private readonly ExcelExporter _exporter; //IExcelExporter 

    public ExcelExportBenchmark()
    {
        _data = GenerateOrders(NumberOfOrders);
        _exporter = new ExcelExporter();
    }

    // This method runs once before all iterations of this benchmark
    [GlobalSetup]
    public void Setup()
    {
        // Optional: Reset any state necessary before the full run
    }
    
    [Benchmark]
    public void ExportToExcel_Optimized()
    {
        _exporter.ExportToExcel("OrdersExport.xlsx", "Orders", _data);
    }

    [Benchmark]
    public void ExportToExcel_Baseline()
    {
        _exporter.ExportToExcel("OrdersExport.xlsx", "Orders", _data);
    }

    private static List<Order> GenerateOrders(int numberOfOrders)
    {
        var random = new Random();
        var address = new Address
        {
            Address1 = "Address1",
            Address2 = "Address2",
            City = "City",
            Country = "Country",
            FullName = "FullName",
            Zip = "Zip"
        };
        var orderItem = new OrderItem
        {
            Name = "Name",
            Price = 0,
            ProductId = 1,
            Quantity = 1,
        };

        //return Enumerable.Range(1, count).Select(i => new { Id = i, Name = $"Item {i}" }).Cast<Order>().ToList();
        return [.. Enumerable.Range(1, numberOfOrders)
            .Select(i => new Order
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
                Subtotal = random.Next(100, 10000),
                DeliveryFee = random.Next(10, 100),
                OrderStatus = (OrderStatus)random.Next(0, Enum.GetValues<OrderStatus>().Length),
                ShippingAddressId = random.Next(1, 1000),
                UserId = Guid.NewGuid(),
                PaymentIntentId = random.Next(0, 2) == 0 ? null : Guid.NewGuid().ToString(),
                ShippingAddress = address,
                //User = null,
                OrderItems = new List<OrderItem> { orderItem }
            })];
    }

}
