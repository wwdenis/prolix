// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;

namespace Marketplace.Models.Trading
{
    public class OrderItemModel : ActiveModel
    {
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string OrderName { get; set; }
        public string ProductName { get; set; }
    }
}
