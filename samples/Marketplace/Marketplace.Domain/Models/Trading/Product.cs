// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Configuration;
using System.Collections.Generic;
using Prolix.Domain;

namespace Marketplace.Domain.Models.Trading
{
    public class Product : ActiveDetailModel
    {
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }
        public int? DealerId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Dealer Dealer { get; set; }
    }
}
