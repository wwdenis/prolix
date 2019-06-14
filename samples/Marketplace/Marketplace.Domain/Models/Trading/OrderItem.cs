// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using Prolix.Domain;

namespace Marketplace.Domain.Models.Trading
{
    public class OrderItem : ActiveModel
    {
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }

        public int? OrderId { get; set; }
        public int? ProductId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
