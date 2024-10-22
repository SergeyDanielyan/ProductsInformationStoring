using System.Runtime.InteropServices.JavaScript;
using Domain.Models;
using Domain.Repositories;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OzonHW2;

namespace OzonHW2.Services;

public class GreeterService : GoodItemProtoService.GoodItemProtoServiceBase
{
    private readonly IGoodItemService _goodItemService;

    public GreeterService(IGoodItemService goodItemService)
    {
        _goodItemService = goodItemService;
    }
    
    public override Task<GoodsId> CreateGoods(GoodsDto request, ServerCallContext context)
    {
        return Task.FromResult(new GoodsId
        {
            Id = _goodItemService.CreateGoodItem(request.Name, (decimal)request.Price, request.Weight,
                (Domain.Enums.ProductType)request.MyProductType, request.CreationDate.ToDateTime(),
                request.WarehouseNumber).ToString()
        });
    }

    public override Task<Goods> GetGoodById(GoodsId request, ServerCallContext context)
    {
        GoodItem goodItem;
        try
        {
            goodItem = _goodItemService.GetGoodById(request.Id);
        }
        catch (ArgumentException e)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
        }

        return Task.FromResult(new Goods
        {
            Id = request.Id,
            Name = goodItem.Name,
            Price = (double)goodItem.Price,
            Weight = goodItem.Weight,
            MyProductType = (ProductType)goodItem.MyProductType,
            CreationDate = goodItem.CreationDate.ToTimestamp(),
            WarehouseNumber = goodItem.WarehouseNumber
        });
    }

    public override Task<Goods> UpdatePrice(UpdatePriceRequest request, ServerCallContext context)
    {
        GoodItem goodItem;

        try
        {
            goodItem = _goodItemService.UpdatePrice(request.Id, (decimal)request.NewPrice);
        }
        catch (ArgumentException e)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
        }
        return Task.FromResult(new Goods
        {
            Id = goodItem.Id.ToString(),
            Name = goodItem.Name,
            Price = (double)goodItem.Price,
            Weight = goodItem.Weight,
            MyProductType = (ProductType)goodItem.MyProductType,
            CreationDate = goodItem.CreationDate.ToTimestamp(),
            WarehouseNumber = goodItem.WarehouseNumber
        });
    }

    public override Task<FilterReply> FilterGoods(FilterRequest request, ServerCallContext context)
    {
        List<Goods> goods = _goodItemService.FilterGoods(request.CreationDate.ToDateTime(),
            (Domain.Enums.ProductType)request.MyProductType, request.WarehouseNumber, request.PageNumber,
            request.PageLength).Select(item => new Goods
        {
            Id = item.Id.ToString(),
            Name = item.Name,
            Price = (double)item.Price,
            Weight = item.Weight,
            MyProductType = (ProductType)item.MyProductType,
            CreationDate = item.CreationDate.ToTimestamp(),
            WarehouseNumber = item.WarehouseNumber
        }).ToList();
        FilterReply filterReply = new FilterReply();
        filterReply.GoodsList.Add(goods);
        return Task.FromResult(filterReply);
    }
}