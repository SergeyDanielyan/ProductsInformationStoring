using Domain.Models;
using Domain.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace OzonHW2.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GoodsController : ControllerBase
{
    private IGoodItemService _goodItemService;
    
    public GoodsController(IGoodItemService goodItemService)
    {
        _goodItemService = goodItemService;
    }

    [HttpPost]
    public IActionResult CreateGoods(GoodItemDto goodsDto)
    {
        return Ok(_goodItemService.CreateGoodItem(goodsDto.Name, (decimal)goodsDto.Price, goodsDto.Weight, 
            goodsDto.MyProductType, goodsDto.CreationDate, goodsDto.WarehouseNumber).ToString());
    }
    
    [HttpGet("{myId}")]
    public IActionResult GetGoodById(string myId)
    {
        GoodItem goodItem;
        try
        {
            goodItem = _goodItemService.GetGoodById(myId);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }

        return Ok(goodItem);
    }
    
    [HttpPatch("{myId}")]
    public IActionResult UpdatePrice(string myId, [FromBody] decimal newPrice)
    {
        GoodItem goodItem;

        try
        {
            goodItem = _goodItemService.UpdatePrice(myId, newPrice);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        return Ok(goodItem);
    }
    
    [HttpGet]
    public IActionResult FilterGoods([FromQuery] DateTime creationDate, 
        [FromQuery] int productType, [FromQuery] int warehouseNumber, [FromQuery] int pageNumber, 
        [FromQuery] int pageLength)
    {
        return Ok(_goodItemService.FilterGoods(creationDate,
                              (Domain.Enums.ProductType)productType, warehouseNumber, pageNumber,
                              pageLength));
    }
}