// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Core.Logic;
using System;
using System.Linq;

namespace Marketplace.Domain.Descriptors.Trading
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class OrderDescriptor : ModelDescriptor<Order>
    {
        public OrderDescriptor()
        {
            Model("Order");

            Field(i => i.Active)
                .Caption("Active");

            Field(i => i.Date)
                .Caption("Date")
                .Required()
                .Minimum(DateTime.Now, "Transaction date must be today or later");

            Field(i => i.TotalAmount)
                .Caption("Total Amount")
                .GreaterThan(0)
                .Required();

            Field(i => i.StatusId)
                .Caption("Status")
                .Required();

            Field(i => i.CustomerId)
                .Caption("Customer")
                .Required();

            Field(i => i.DealerId)
                .Caption("Dealer")
                .Required();
        }
    }
}
