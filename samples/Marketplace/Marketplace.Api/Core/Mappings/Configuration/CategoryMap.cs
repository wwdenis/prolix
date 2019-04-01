// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using AutoMapper;

using Marketplace.Client.Models.Configuration;
using Marketplace.Domain.Models.Configuration;

namespace Marketplace.Api.Core.Mapping.Configuration
{
    public class CategoryMap : Profile
    {
        public CategoryMap()
        {
            CreateMap<Category, CategoryModel>()
                .ReverseMap();
        }
    }
}
