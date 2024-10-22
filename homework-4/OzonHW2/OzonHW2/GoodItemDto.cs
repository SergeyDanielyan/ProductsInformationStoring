namespace OzonHW2;

public class GoodItemDto
{
    public string Name { get; set; } 
    public decimal Price { get; set; } 
    public double Weight { get; set; } 
    public Domain.Enums.ProductType MyProductType { get; set; } 
    public DateTime CreationDate { get; set; } 
    public int WarehouseNumber { get; set; }
    
    public GoodItemDto()
    {
    }
    
    public GoodItemDto(string name, decimal price, double weight, Domain.Enums.ProductType productType, DateTime creationDate, 
            int warehouseNumber)
    {
        Name = name; 
        Price = price; 
        Weight = weight; 
        MyProductType = productType;
        CreationDate = creationDate; 
        WarehouseNumber = warehouseNumber;
    }
}