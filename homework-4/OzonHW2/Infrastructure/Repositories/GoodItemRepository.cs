using System.Collections.Concurrent;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class GoodItemRepository : IGoodItemRepository
{
    private ConcurrentDictionary<Guid, GoodItem> _goodItems;

    public GoodItemRepository()
    {
        _goodItems = new ConcurrentDictionary<Guid, GoodItem>();
    }

    public GoodItem CreateGoodItem(string name, decimal price, double weight, ProductType productType, DateTime creationDate,
        int warehouseNumber)
    {
        Guid id;
        do
        {
            id = Guid.NewGuid();
        } while (_goodItems.ContainsKey(id));
        GoodItem goodItem = new GoodItem(id, name, price, weight, productType, creationDate, warehouseNumber);
        _goodItems[id] = goodItem;
        return goodItem;
    }

    public GoodItem? FindGoodItemById(Guid id)
    {
        if (_goodItems.ContainsKey(id))
        {
            return _goodItems[id];
        }
        return null;
    }

    public void UpdatePriceById(Guid id, decimal price)
    {
        _goodItems[id].Price = price;
    }

    public List<GoodItem> FindGoodItems(DateTime creationDate, ProductType productType,
        int warehouseNubmer)
    {
        return _goodItems.Where(goodItem => goodItem.Value.CreationDate == creationDate
                                            && goodItem.Value.MyProductType == productType
                                            && goodItem.Value.WarehouseNumber == warehouseNubmer)
            .Select(goodItem => goodItem.Value).ToList();
    }
}