// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http;

using Marketplace.Client.Models.Geography;
using Marketplace.Domain.Models.Geography;
using Marketplace.Logic.Contracts.Geography;
using Marketplace.Logic.Queries.Geography;

using Prolix.AspNet.Controllers;
using Prolix.AspNet.Extensions;

namespace Marketplace.Api.Controllers
{
    public class ProvinceController : BaseController
    {
        public ProvinceController(IProvinceService provinceService)
        {
            ProvinceService = provinceService;
        }

        IProvinceService ProvinceService { get; }

        // GET Province
		public IHttpActionResult Get([FromUri] ProvinceQuery request)
        {
            var list = ProvinceService.List(request);
            var mapped = list.Map<Province, ProvinceModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }
    }
}
