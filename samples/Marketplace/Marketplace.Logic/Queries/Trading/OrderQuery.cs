// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using Marketplace.Domain.Models.Trading;
using Prolix.Core.Collections;

namespace Marketplace.Logic.Queries.Trading
{
    public class OrderQuery : QueryRequest<Order>
    {
        public OrderQuery()
        {
            // Map sort expressions
            MapSort("Date", i => i.Date);
            MapSort("CustomerName", i => i.Customer.Name);
            MapSort("DealerName", i => i.Dealer.Name);
            MapSort("TotalAmount", i => i.TotalAmount);
            MapSort("StatusName", i => i.Status.Name);
            
            SortField = "Date";
        }

        public int? CustomerId { get; set; }
        public int? DealerId { get; set; }
        public int? ProductId { get; set; }
        public int? StatusId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
    }
}
