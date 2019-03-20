// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;

using Marketplace.Data;
using Marketplace.Domain.Models.Geography;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Geography;
using Marketplace.Logic.Queries.Geography;

using Prolix.Core.Collections;

namespace Marketplace.Logic.Services.Geography
{
    public class CountryService : RepositoryService<Country>, ICountryService
    {
        public CountryService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        public override IQueryable<Country> List()
        {
            return
                from i in base.List()
                orderby i.Name
                select i;
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<Country> List(CountryQuery request)
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
