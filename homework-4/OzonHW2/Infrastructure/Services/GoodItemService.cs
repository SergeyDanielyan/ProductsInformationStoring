using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class GoodItemService : IGoodItemService
{
    private readonly IGoodItemRepository _goodItemRepository;
    private readonly object _lock = new object();

    public GoodItemService(IGoodItemRepository goodItemRepository)
    {
        _goodItemRepository = goodItemRepository;
    }
    
    public string CreateGoodItem(string name, decimal price, double weight, ProductType myProductType, DateTime creationDate,
        int warehouseNumber)
    {
        string id;
        lock (_lock)
        {
            id = _goodItemRepository.CreateGoodItem(name, price, weight, myProductType, creationDate, 
                warehouseNumber).Id.ToString();
        }
        return id;
    }

    public GoodItem GetGoodById(string id)
    {
        if (Guid.TryParse(id, out Guid guidId))
        {
            GoodItem? goodItem = _goodItemRepository.FindGoodItemById(guidId);
            if (goodItem != null)
            {
                return goodItem;
            }
            
            throw new ArgumentException("Wrong id");
        }
        throw new ArgumentException("Wrong id format");
    }

    public GoodItem UpdatePrice(string id, decimal newPrice)
    {
        if (!Guid.TryParse(id, out Guid guidId))
        {
            throw new ArgumentException("Wrong id format");
        }
        if (_goodItemRepository.FindGoodItemById(guidId) == null)
        {
            throw new ArgumentException("Wrong id");
        }
        lock (_lock)
        {
            _goodItemRepository.UpdatePriceById(guidId, newPrice);
        }

        GoodItem? goodItem = _goodItemRepository.FindGoodItemById(guidId);
        return goodItem;
    }

    public List<GoodItem> FilterGoods(DateTime creationDate, ProductType myProductType, int warehouseNumber, int pageNumber, int pageLength)
    {
        List<GoodItem> filteredGoods = _goodItemRepository
            .FindGoodItems(creationDate, myProductType, warehouseNumber);
        if (filteredGoods.Count <= (pageNumber - 1) * pageLength)
        {
            return new List<GoodItem>();
        }

        return filteredGoods.GetRange(
            (pageNumber - 1) * pageLength,
            Math.Min(pageLength, filteredGoods.Count - (pageNumber - 1) * pageLength));
    }
}