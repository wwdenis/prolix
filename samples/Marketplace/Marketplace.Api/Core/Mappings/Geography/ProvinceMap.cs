// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;

using Marketplace.Models.Geography;
using Marketplace.Domain.Models.Geography;

namespace Marketplace.Api.Core.Mapping.Geography
{
    public class ProvinceMap : Profile
    {
        public ProvinceMap()
        {
            CreateMap<Province, ProvinceModel>()
                .ReverseMap();
        }
    }
}
