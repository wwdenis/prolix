// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;

using Marketplace.Client.Models.Trading;
using Marketplace.Domain.Models.Trading;

namespace Marketplace.Api.Core.Mapping.Trading
{
    public class SaleMap : Profile
    {
        public SaleMap()
        {
            CreateMap<OrderItem, OrderItemModel>()
                .ReverseMap();

            CreateMap<Order, OrderModel>()
                .ReverseMap();

            CreateMap<Order, DetailOrderModel>()
                .ReverseMap();
        }
    }
}
