using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Repositories;

namespace xUnitTestProject;

public class GoodItemRepositoryTests
{
    [Fact]
    public void Test_GoodItemRepository_Create()
    {
        IGoodItemRepository repository = new GoodItemRepository();
        
        var goodItem = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        
        Assert.Equal(goodItem.Name, "name");
        Assert.Equal(goodItem.Price, 200.0m);
        Assert.Equal(goodItem.Weight, 10.0);
        Assert.Equal(goodItem.MyProductType, ProductType.GENERAL);
        Assert.Equal(goodItem.CreationDate, DateTime.Today);
        Assert.Equal(goodItem.WarehouseNumber, 2);
    }

    [Fact]
    public void Test_GoodItemRepository_FindById()
    {
        IGoodItemRepository repository = new GoodItemRepository();
        
        var goodItem = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        var gotGoodItem = repository.FindGoodItemById(goodItem.Id);
        
        Assert.Equivalent(goodItem, gotGoodItem);
    }

    [Fact]
    public void Test_GoodItemRepository_FindByWrongId()
    {
        IGoodItemRepository repository = new GoodItemRepository();

        var goodItem = repository.FindGoodItemById(Guid.NewGuid());
        
        Assert.Null(goodItem);
    }

    [Fact]
    public void Test_GoodItemRepository_UpdatePrice()
    {
        IGoodItemRepository repository = new GoodItemRepository();
        
        var goodItem = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        repository.UpdatePriceById(goodItem.Id, 300.0m);
        
        Assert.Equivalent(goodItem, new GoodItem(goodItem.Id, "name", 300.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2));
    }

    [Fact]
    public void Test_GoodItemRepository_FindGoodItems()
    {
        IGoodItemRepository repository = new GoodItemRepository();
        
        var goodItem1 = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        var goodItem2 = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 2);
        var goodItem3 = repository.CreateGoodItem("name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 1);
        var goodItem4 = repository.CreateGoodItem("name", 200.0m, 20.0, ProductType.GENERAL, DateTime.Today, 2);
        List<GoodItem> goodItems = repository.FindGoodItems(DateTime.Today, ProductType.GENERAL, 2);
        
        Assert.Equivalent(new List<GoodItem>(){ goodItem1, goodItem4 }, goodItems);
    }
}