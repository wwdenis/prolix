// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Core.Logic;

namespace Marketplace.Domain.Descriptors.Trading
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class ProductDescriptor : ModelDescriptor<Product>
    {
        public ProductDescriptor()
        {
            Model("Product");

            Field(i => i.Active)
                .Caption("Active");

            Field(i => i.Name)
                .Caption("Name")
                .MaxLength(50)
                .Required();

            Field(i => i.Name)
                .Caption("Detail")
                .MaxLength(100)
                .Required();

            Field(i => i.Stock)
                .Caption("Stock")
                .GreaterThan(0);

            Field(i => i.Price)
                .Caption("Price")
                .GreaterThan(0);

            Field(i => i.CategoryId)
                .Caption("Category")
                .Required();

            Field(i => i.DealerId)
                .Caption("Dealer")
                .Required();
        }
    }
}
