// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Collections;

namespace Marketplace.Logic.Services.Trading
{
    public sealed class ProductService : RepositoryService<Product>, IProductService
    {
        public ProductService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        async public override Task Add(Product model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // New records are set to active
            model.Active = true;

            // Persist on database
            await base.Add(model);
        }

        async public override Task<bool> Update(Product model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Id != model.Id && i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // Retrieves data
            var current = Get(model.Id);

            if (current == null)
                return false;

            // Update fields
            current.Name = model.Name;
            current.Active = model.Active;
            current.CategoryId = model.CategoryId;
            current.DealerId = model.DealerId;
            current.Price = model.Price;
            current.Stock = model.Stock;

            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Product model)
        {
            if (model == null)
                return false;

            if (Context.OrderItems.Any(i => i.ProductId == model.Id))
                Rule.Throw("This record can't be deleted because it has child records.");

            return await base.Delete(model);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Product> List(ProductQuery request)
        {
            var query = base.List();

            // Build que query
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(i => i.Name.Contains(request.Name));

            if (request.CategoryId != null)
                query = query.Where(i => i.CategoryId == request.CategoryId);

            if (request.DealerId != null)
                query = query.Where(i => i.DealerId == request.DealerId);

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }
    }
}
