// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using System.Collections.Generic;
using Prolix.Domain;

namespace Marketplace.Domain.Models.Configuration
{
    public class Category : ActiveNamedModel
    {
        public virtual IList<Product> Products { get; } = new List<Product>();
    }
}
