using System.Text.Json.Serialization;
using Domain.Enums;

namespace Domain.Models;

public class GoodItem
{
    [JsonPropertyName("Id")]
    public Guid Id { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("Price")]
    public decimal Price { get; set; }
    [JsonPropertyName("Weight")]
    public double Weight { get; set; }
    [JsonPropertyName("MyProductType")]
    public ProductType MyProductType { get; set; }
    [JsonPropertyName("CreationDate")]
    public DateTime CreationDate { get; set; }
    [JsonPropertyName("WarehouseNumber")]
    public int WarehouseNumber { get; set; }

    public GoodItem()
    {
        
    }

    public GoodItem(Guid id, string name, decimal price, double weight, ProductType productType, DateTime creationDate, 
        int warehouseNumber)
    {
        Id = id;
        Name = name;
        Price = price;
        Weight = weight;
        MyProductType = productType;
        CreationDate = creationDate;
        WarehouseNumber = warehouseNumber;
    }
}