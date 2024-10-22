using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories;

public interface IGoodItemRepository
{
    GoodItem CreateGoodItem(string name, decimal price, double weight, ProductType productType, DateTime creationDate, 
                              int warehouseNumber);
    
    GoodItem? FindGoodItemById(Guid id);

    void UpdatePriceById(Guid id, decimal price);

    List<GoodItem> FindGoodItems(DateTime creationDate,
        ProductType productType, int warehouseNubmer);
}