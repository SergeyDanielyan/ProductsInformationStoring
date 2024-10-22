using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc.Testing;
using OzonHW2;
using Xunit;

namespace IntegrationTestProject;

public class GoodsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GoodsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Test_GoodsController_Create()
    {
        var client = _factory.CreateClient();
        string endpoint = "api/v1/Goods";
        
        var goodItemDto = new GoodItemDto("name", 200.0m, 10.0, 0,
            new DateTime(2024, 9, 28), 2);
        
        var response = await client.PostAsJsonAsync(endpoint, goodItemDto); 
        
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test_GoodsController_FindById()
    {
        var client = _factory.CreateClient();
        string endpoint = "api/v1/Goods";

        var goodsDto = new GoodItemDto
        {
            Name = "name",
            Price = 200.0m,
            Weight = 10.0,
            MyProductType = 0,
            CreationDate = new DateTime(2024, 9, 28),
            WarehouseNumber = 2
        };

        var creationResponse = await client.PostAsJsonAsync(endpoint, goodsDto);
        
        var creationResponseObject = await creationResponse.Content.ReadAsStringAsync();

        var endpointWithId = $"{endpoint}/{creationResponseObject}";

        var response = await client.GetAsync(endpointWithId);

        var responseContent = await response.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<GoodItem>(responseContent);
        
        Assert.Equivalent(responseObject, new GoodItem(Guid.Parse(creationResponseObject), "name", 200.0m,
            10.0, 0, new DateTime(2024, 9, 28), 2));
    }

    [Fact]
    public async Task Test_GoodsController_FindByWrongId()
    {
        var client = _factory.CreateClient();
        Guid id = Guid.NewGuid();
        var endpoint = $"api/v1/Goods/{id.ToString()}";

        var response = await client.GetAsync(endpoint);
        
        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
        Assert.Equal(await response.Content.ReadAsStringAsync(), "Wrong id");
    }

    [Fact]
    public async Task Test_GoodsController_FindByWrongFormatId()
    {
        var client = _factory.CreateClient();
        var endpoint = "api/v1/Goods/mYiD";

        var response = await client.GetAsync(endpoint);
        
        Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
        Assert.Equal(await response.Content.ReadAsStringAsync(), "Wrong id format");
    }

    [Fact]
    public async Task Test_GoodsController_UpdatePrice()
    {
        var client = _factory.CreateClient();
        var endpoint = "api/v1/Goods";
        
        var goodsDto = new GoodItemDto
        {
            Name = "name", 
            Price = 200.0m, 
            Weight = 10.0, 
            MyProductType = 0, 
            CreationDate = new DateTime(2024, 9, 28), 
            WarehouseNumber = 2
        };
        
        var creationResponse = await client.PostAsJsonAsync(endpoint, goodsDto);
        var creationResponseObject = await creationResponse.Content.ReadAsStringAsync();

        string endpointWithId = $"{endpoint}/{creationResponseObject}";
        
        var updateResponse = await client.PatchAsJsonAsync(endpointWithId, 300.0m);
        
        var getResponse = await client.GetAsync(endpointWithId);
        var getResponseContent = await getResponse.Content.ReadAsStringAsync();
        var getResponseObject = JsonSerializer.Deserialize<GoodItem>(getResponseContent);
        
        Assert.Equal(System.Net.HttpStatusCode.OK, updateResponse.StatusCode);
        Assert.Equivalent(getResponseObject, new GoodItem
        {
            Id = Guid.Parse(creationResponseObject),
            Name = goodsDto.Name,
            Price = 300.0m,
            Weight = goodsDto.Weight,
            CreationDate = goodsDto.CreationDate,
            MyProductType = goodsDto.MyProductType,
            WarehouseNumber = goodsDto.WarehouseNumber
        });
    }

    private bool GoodItemsEqual(GoodItem goodItem1, GoodItem goodItem2)
    {
        return goodItem1.Id == goodItem2.Id && goodItem1.Name == goodItem2.Name && goodItem1.Price == goodItem2.Price
               && goodItem1.Weight == goodItem2.Weight && goodItem1.MyProductType == goodItem2.MyProductType
               && goodItem1.CreationDate == goodItem2.CreationDate &&
               goodItem1.WarehouseNumber == goodItem2.WarehouseNumber;
    }

    [Fact]
    public async Task Test_GoodsController_FilterGoods()
    {
        var client = _factory.CreateClient();
        var endpoint = "api/v1/Goods";
        
        var goodItem1 = new GoodItemDto
        {
            Name = "name1",
            Price = 200.0m,
            Weight = 10.0,
            MyProductType = 0, 
            CreationDate = new DateTime(2024, 9, 28),
            WarehouseNumber = 2
        };
        var goodItem2 = new GoodItemDto
        {
            Name = "name2",
            Price = 200.0m,
            Weight = 10.0,
            MyProductType = 0, 
            CreationDate = new DateTime(2024, 9, 27),
            WarehouseNumber = 2
            
        };
        var goodItem3 = new GoodItemDto
        {
            Name = "name3",
            Price = 200.0m,
            Weight = 10.0,
            MyProductType = 0, 
            CreationDate = new DateTime(2024, 9, 27),
            WarehouseNumber = 1
            
        };
        var goodItem4 = new GoodItemDto
        {
            Name = "name4",
            Price = 200.0m,
            Weight = 20.0,
            MyProductType = 0, 
            CreationDate = new DateTime(2024, 9, 28),
            WarehouseNumber = 2
        };

        var createResponse1 = await client.PostAsJsonAsync(endpoint, goodItem1);
        var createResponse2 = await client.PostAsJsonAsync(endpoint, goodItem2); 
        var createResponse3 = await client.PostAsJsonAsync(endpoint, goodItem3); 
        var createResponse4 = await client.PostAsJsonAsync(endpoint, goodItem4); 
        
        var createResponseObject1 = await createResponse1.Content.ReadAsStringAsync();
        var createResponseObject2 = await createResponse2.Content.ReadAsStringAsync();
        var createResponseObject3 = await createResponse3.Content.ReadAsStringAsync();
        var createResponseObject4 = await createResponse4.Content.ReadAsStringAsync();
        
        var filterEndpoint = $"{endpoint}?creationDate={new DateTime(2024, 9, 28).ToString("o").Replace(":", "%3A")}" +
                             $"&productType=0&warehouseNumber=2&pageNumber=2&pageLength=1";
        var response = await client.GetAsync(filterEndpoint);
        var filterResponseObject = JsonSerializer.Deserialize<List<GoodItem>>(await response.Content.ReadAsStringAsync());
        var filterReply = new List<GoodItem>() 
        { 
            new GoodItem
            { 
                Id = Guid.Parse(createResponseObject1), 
                Name = goodItem1.Name, 
                Price = goodItem1.Price, 
                Weight = goodItem1.Weight, 
                CreationDate = goodItem1.CreationDate, 
                MyProductType = goodItem1.MyProductType, 
                WarehouseNumber = goodItem1.WarehouseNumber
            },
            new GoodItem
            { 
                Id = Guid.Parse(createResponseObject4), 
                Name = goodItem4.Name, 
                Price = goodItem4.Price, 
                Weight = goodItem4.Weight, 
                CreationDate = goodItem4.CreationDate, 
                MyProductType = goodItem4.MyProductType, 
                WarehouseNumber = goodItem4.WarehouseNumber
            }
        };
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(filterResponseObject.Count, 1);
        Assert.True(GoodItemsEqual(filterReply[0], filterResponseObject[0]) 
                    || GoodItemsEqual(filterReply[1], filterResponseObject[0]));
    }
}