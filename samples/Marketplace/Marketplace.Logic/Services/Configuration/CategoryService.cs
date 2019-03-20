// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Logic.Contracts.Configuration;
using Marketplace.Logic.Queries.Configuration;

using Prolix.Core.Collections;

namespace Marketplace.Logic.Services.Configuration
{
    public sealed class CategoryService : UpdatableService<Category>, ICategoryService
    {
        public CategoryService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        async public override Task Add(Category model)
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

        async public override Task<bool> Update(Category model)
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

            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Category model)
        {
            if (model.Products.Any())
                Rule.Throw("This record can't be deleted because it has child records.");

            return await base.Delete(model);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Category> List(CategoryQuery request)
        {
            var query = base.List();

            // Build que query
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(i => i.Name.Contains(request.Name));

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }
    }
}
