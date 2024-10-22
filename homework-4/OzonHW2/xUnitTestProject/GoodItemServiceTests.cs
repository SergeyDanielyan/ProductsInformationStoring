using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Moq;

namespace xUnitTestProject;

public class GoodItemServiceTests
{
    [Fact]
    public void Test_GoodItemService_Create()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        mockRepository.Setup(x => x.CreateGoodItem("name", 200.0m, 10.0,
                ProductType.GENERAL, DateTime.Today, 2))
            .Returns(new GoodItem(myId, "name", 300.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2));
        
        var goodItemService = new GoodItemService(mockRepository.Object);

        string result = goodItemService.CreateGoodItem("name", 200.0m, 10.0,
            ProductType.GENERAL, DateTime.Today, 2);
        
        Assert.Equal(result, myId.ToString());
    }

    [Fact]
    public void Test_GoodItemService_GetById()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        var goodItem = new GoodItem(myId, "name", 300.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        mockRepository.Setup(x => x.FindGoodItemById(myId)).Returns(goodItem);
        
        var goodItemService = new GoodItemService(mockRepository.Object);

        GoodItem myGoodItem = goodItemService.GetGoodById(myId.ToString());
        
        Assert.Equivalent(myGoodItem, goodItem);
    }

    [Fact]
    public void Test_GoodItemService_GetByWrongId()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        GoodItem? goodItem = null;
        mockRepository.Setup(x => x.FindGoodItemById(myId)).Returns(goodItem);
        
        var goodItemService = new GoodItemService(mockRepository.Object);
        Assert.Throws<ArgumentException>(() => goodItemService.GetGoodById(myId.ToString()));
    }
    
    [Fact]
    public void Test_GoodItemService_GetByWrongFormatId()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        
        var goodItemService = new GoodItemService(mockRepository.Object);
        
        Assert.Throws<ArgumentException>(() => goodItemService.GetGoodById("IT'S ID!!!!!!!!!!"));
    }

    [Fact]
    public void Test_GoodItemService_UpdatePrice()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        var goodItemPrevious = new GoodItem(myId, "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        var goodItemNew = new GoodItem(myId, "name", 300.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        mockRepository.Setup(x => x.UpdatePriceById(myId, 300.0m));
        mockRepository.Setup(x => x.FindGoodItemById(myId)).Returns(goodItemPrevious);
        
        var goodItemService = new GoodItemService(mockRepository.Object);

        goodItemService.UpdatePrice(myId.ToString(), 300.0m);
    }
    
    [Fact]
    public void Test_GoodItemService_UpdatePriceByWrongId()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        
        var goodItemService = new GoodItemService(mockRepository.Object);
        
        Assert.Throws<ArgumentException>(() => goodItemService.UpdatePrice("IT'S ID!!!!!!!!!!", 300.0m));
    }
    
    [Fact]
    public void Test_GoodItemService_UpdatePriceByWrongFormatId()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        GoodItem? goodItem = null;
        mockRepository.Setup(x => x.FindGoodItemById(myId)).Returns(goodItem);
        
        var goodItemService = new GoodItemService(mockRepository.Object);
        Assert.Throws<ArgumentException>(() => goodItemService.UpdatePrice(myId.ToString(), 300.0m));
    }

    [Fact]
    public void Test_GoodItemService_Filter()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        var goodItem1 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        var goodItem2 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 2);
        var goodItem3 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 1);
        var goodItem4 = new GoodItem(Guid.NewGuid(),"name", 200.0m, 20.0, ProductType.GENERAL, DateTime.Today, 2);

        mockRepository.Setup(x => x.FindGoodItems(DateTime.Today, ProductType.GENERAL,
            2)).Returns(new List<GoodItem>() { goodItem1, goodItem4 });

        var goodItemService = new GoodItemService(mockRepository.Object);
        
        Assert.Equivalent(goodItemService.FilterGoods(DateTime.Today, ProductType.GENERAL, 2, 2, 1), new List<GoodItem>{goodItem4});
    }
    
    [Fact]
    public void Test_GoodItemService_Filter_Empty()
    {
        var mockRepository = new Mock<IGoodItemRepository>();
        var myId = Guid.NewGuid();
        var goodItem1 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today, 2);
        var goodItem2 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 2);
        var goodItem3 = new GoodItem(Guid.NewGuid(), "name", 200.0m, 10.0, ProductType.GENERAL, DateTime.Today.AddDays(-1), 1);
        var goodItem4 = new GoodItem(Guid.NewGuid(),"name", 200.0m, 20.0, ProductType.GENERAL, DateTime.Today, 2);

        mockRepository.Setup(x => x.FindGoodItems(DateTime.Today.AddDays(1), ProductType.GENERAL,
            2)).Returns(new List<GoodItem>() { });

        var goodItemService = new GoodItemService(mockRepository.Object);
        
        Assert.Equivalent(goodItemService.FilterGoods(DateTime.Today.AddDays(1), ProductType.GENERAL, 2, 2, 1), new List<GoodItem>{});
    }
}