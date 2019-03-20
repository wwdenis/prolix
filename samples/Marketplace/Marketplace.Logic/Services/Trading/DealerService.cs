// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Models.Trading;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Core.Collections;

namespace Marketplace.Logic.Services.Trading
{
    public sealed class DealerService : UpdatableService<Dealer>, IDealerService
    {
        public DealerService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        async public override Task Add(Dealer model)
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

        async public override Task<bool> Update(Dealer model)
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
            current.PhoneNumber = model.PhoneNumber;
            current.City = model.City;
            current.ZipCode = model.ZipCode;
            current.ProvinceId = model.ProvinceId;
            current.CountryId = model.CountryId;
            current.UserId = model.UserId;
            current.Active = model.Active;

            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Dealer model)
        {
            if (model == null)
                return false;

            if (model.Products.Any())
                Rule.Throw("This dealer can't be deleted because it has associated products.");

            if (model.Orders.Any())
                Rule.Throw("This dealer can't be deleted because it has associated orders.");
            
            return await base.Delete(model);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Dealer> List(DealerQuery request)
        {
            var query = base.List();

            // Build que query
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(i => i.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.CityName))
                query = query.Where(i => i.City.Contains(request.CityName));

            if (request.ProvinceId != null)
                query = query.Where(i => i.ProvinceId == request.ProvinceId);

            if (request.CountryId!= null)
                query = query.Where(i => i.CountryId  == request.CountryId);

            if (request.HasOrders)
                query = query.Where(i => i.Orders.Any());

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }
    }
}
