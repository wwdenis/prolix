// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;

namespace Marketplace.Client.Models.Trading
{
    public class ProductModel : ActiveDetailModel
    {
        public int Stock { get; set; }
        public decimal Price { get; set; }

        public int? CategoryId { get; set; }
        public int? DealerId { get; set; }

        public string CategoryName { get; set; }
        public string DealerName { get; set; }
    }
}
