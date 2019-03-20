// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Core.Collections;
using Prolix.Core.Extensions.Parsing;
using Prolix.Core.Extensions.Reflection;
using Prolix.Core.Extensions.Collections;

namespace Marketplace.Logic.Services.Trading
{
    public sealed class OrderService : UpdatableService<Order>, IOrderService
    {
        public OrderService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        async public override Task Add(Order model)
        {
            // Simple validations
            Validate(model);

            if (!model.Items.Any())
                Rule.Add("Items", "Your order must have at least one item!");

            var dateLimit = DateTime.Now.AddDays(7);

            var invalidItems = from i in model.Items
                               where i.Amount <= 0 || i.Quantity <= 0 || i.ProductId == null
                               select i;

            // Custom validations
            if (model.Date > dateLimit)
            {
                Rule.Add("Date", "Future order are allowed to 1 week maximum"); ;
            }

            if (invalidItems.Any())
            {
                Rule.Add("Items", "There are invalid items in your order. Please verify."); ;
            }
            
            // Throws an exception if there are validation errors
            CheckRule();

            // New records are set to active
            model.Active = true;

            // Persist on database
            await base.Add(model);
        }

        async public override Task<bool> Update(Order model)
        {
            // Simple validations
            Validate(model);

            var dateLimit = DateTime.Now.AddDays(7);

            // Custom validations
            if (model.Date > dateLimit)
            {
                Rule.Add("Date", "Future order are allowed to 1 week maximum"); ;
            }

            // Throws an exception if there are validation errors
            CheckRule();

            // Retrieves data
            var current = Get(model.Id);
            
            if (current == null)
                return false;

            if (model.StatusId < current.StatusId)
            {
                Rule.Add("StatusId", "Order Status can't move backwards."); ;
                Rule.Check();
            }

            // Update fields
            current.Date = model.Date;
            current.TotalAmount = model.TotalAmount;
            current.StatusId = model.StatusId;
            current.CustomerId = model.CustomerId;
            current.DealerId = model.DealerId;

            // Update children
            current.Items.Clear();
            current.Items.AddRange(model.Items);
            
            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Order model)
        {
            var orderDate = model?.Date;

            if (orderDate == null)
                return false;

            var difference = DateTime.Now - orderDate.Value;

            if (difference.TotalDays > 30)
                Rule.Throw("This order is too old to be deleted.");

            return await base.Delete(model);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Order> List(OrderQuery request)
        {
            // Nao permite filtro vazio
            if (request.IsEmpty())
                return PagedList<Order>.Empty();

            // Valida os filtros
            if (request.StartDate != null && request.EndDate != null && request.StartDate > request.EndDate)
            {
                Rule.Add("StartDate", "Check your date criteria");
                Rule.Add("EndDate", "Check your date criteria");
            }

            if (request.AmountFrom != null && request.AmountTo != null && request.AmountFrom > request.AmountTo)
            {
                Rule.Add("AmountFrom", "Check your ammount criteria");
                Rule.Add("AmountTo", "Check your ammount criteria");
            }

            // Throw and error if there are broken rulea
            CheckRule();

            var query = base.List();

            // Build que query
            if (request.CustomerId != null)
                query = query.Where(i => i.CustomerId == request.CustomerId);

            if (request.DealerId != null)
                query = query.Where(i => i.DealerId == request.DealerId);

            if (request.ProductId != null)
                query = query.Where(i => i.Items.Any(c => c.ProductId == request.ProductId));

            if (request.StatusId != null)
                query = query.Where(i => i.StatusId == request.StatusId);

            // Date
            if (request.StartDate != null && request.EndDate != null)
                query = query.Where(i => i.Date >= request.StartDate && i.Date <= request.EndDate);
            else if (request.StartDate != null)
                query = query.Where(i => i.Date >= request.StartDate);
            else if (request.EndDate != null)
                query = query.Where(i => i.Date <= request.EndDate);

            // Ammount
            if (request.AmountFrom != null && request.AmountTo != null)
                query = query.Where(i => i.TotalAmount >= request.AmountFrom && i.TotalAmount <= request.AmountTo);
            else if (request.AmountFrom != null)
                query = query.Where(i => i.TotalAmount >= request.AmountFrom);
            else if (request.AmountTo != null)
                query = query.Where(i => i.TotalAmount <= request.AmountTo);

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }
    }
}
