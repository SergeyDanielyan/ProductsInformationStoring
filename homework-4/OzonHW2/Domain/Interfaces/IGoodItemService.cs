using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories;

public interface IGoodItemService
{
    public string CreateGoodItem(string name, decimal price, double weight, ProductType myProductType,
        DateTime creationDate, int warehouseNumber);
    
    public GoodItem GetGoodById(string id);
    
    public GoodItem UpdatePrice(string id, decimal newPrice);

    public List<GoodItem> FilterGoods(DateTime creationDate, ProductType myProductType, int warehouseNumber,
        int pageNumber, int pageLength);
}