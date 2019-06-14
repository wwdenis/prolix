// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Configuration;
using Prolix.Logic;

namespace Marketplace.Domain.Descriptors.Configuration
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class CategoryDescriptor : ModelDescriptor<Category>
    {
        public CategoryDescriptor()
        {
            Model("Category");

            Field(i => i.Active)
                .Caption("Active");

            Field(i => i.Name)
                .Caption("Name")
                .MaxLength(50)
                .Required();
        }
    }
}
