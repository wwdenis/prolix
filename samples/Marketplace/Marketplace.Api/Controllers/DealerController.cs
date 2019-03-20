// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Models;
using Marketplace.Models.Trading;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Api.Controllers;
using Prolix.Api.Extensions;

namespace Marketplace.Api.Controllers
{
    public class DealerController : BaseController
    {
        #region Constructors

        public DealerController(IDealerService companyService)
        {
            DealerService = companyService;
        }

        #endregion

        #region Properties

        IDealerService DealerService { get; }

        #endregion

        #region Endpoints

        //GET Dealer/1
        public IHttpActionResult Get(int id)
        {
            var item = DealerService.Get(id);

            if (item == null)
                return NotFound();

            var model = item.Map<DealerModel>();
            return Ok(model);
        }

        //GET /Dealer
        public IHttpActionResult Get([FromUri] DealerQuery request)
        {
            // Query, ordering, pagination
            var list = DealerService.List(request);

            // Siple Id/name list
            if (request.IsSimple)
            {
                var names = list.Map<Dealer, NamedModel>();
                return Page(names);
            }

            // Maps to Api model
            var mapped = list.Map<Dealer, DealerModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /Dealer/1
        async public Task<IHttpActionResult> Put(int id, DealerModel model)
        {
            var item = model.Map<Dealer>(id);
            
            bool success = await DealerService.Update(item);

            if (!success)
                return NotModified();

            return Ok(item.Id);
        }

        //Post: /Dealer
        async public Task<IHttpActionResult> Post(DealerModel model)
        {
            var item = model.Map<Dealer>();

            // Run business rules, save to the database
            await DealerService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: Dealer/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await DealerService.Delete(id);

            return Ok();
        }
        
        #endregion
    }
}
