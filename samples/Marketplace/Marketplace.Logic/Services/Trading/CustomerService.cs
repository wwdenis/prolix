// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Models.Trading;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Collections;

namespace Marketplace.Logic.Services.Trading
{
    public sealed class CustomerService : RepositoryService<Customer>, ICustomerService
    {
        public CustomerService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        async public override Task Add(Customer model)
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

        async public override Task<bool> Update(Customer model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Id != model.Id && i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            if (Exists(i => i.Id != model.Id && i.UserId == model.UserId))
                Rule.Add("UserId", "Invalid user");

            // Throws an exception if there are validation errors
            CheckRule();

            // Retrieves data
            var current = Get(model.Id);

            if (current == null)
                return false;

            // Update fields
            current.Name = model.Name;
            current.UserId = model.UserId;
            current.Active = model.Active;

            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Customer model)
        {
            if (model == null)
                return false;
            
            if (model.Orders.Any())
                Rule.Throw("This customer can't be deleted because it has associated orders.");
            
            return await base.Delete(model);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Customer> List(CustomerQuery request)
        {
            var query = base.List();

            // Build que query
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(i => i.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.Email))
                query = query.Where(i => i.User.Email == request.Email);

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }
    }
}
