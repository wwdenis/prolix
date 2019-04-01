// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http;

using Marketplace.Client.Models.Geography;
using Marketplace.Domain.Models.Geography;
using Marketplace.Logic.Contracts.Geography;
using Marketplace.Logic.Queries.Geography;

using Prolix.Api.Controllers;
using Prolix.Api.Extensions;
using Marketplace.Logic.Services.Geography;

namespace Marketplace.Api.Controllers
{
    public class CountryController : BaseController
    {
        public CountryController(ICountryService countryService)
        {
            CountryService = countryService;
        }

        ICountryService CountryService { get; }

        // GET Country
        public IHttpActionResult Get([FromUri] CountryQuery request)
        {
            var list = CountryService.List(request);
            var mapped = list.Map<Country, CountryModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }
    }
}
