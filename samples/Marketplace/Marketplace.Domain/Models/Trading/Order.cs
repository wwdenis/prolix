// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using Prolix.Domain;

namespace Marketplace.Domain.Models.Trading
{
    public class Order : ActiveModel
    {
        public DateTime? Date { get; set; }
        public decimal TotalAmount { get; set; }

        public int? StatusId { get; set; }
        public int? CustomerId { get; set; }
        public int? DealerId { get; set; }

        public virtual StatusType Status { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Dealer Dealer { get; set; }

        public virtual IList<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
