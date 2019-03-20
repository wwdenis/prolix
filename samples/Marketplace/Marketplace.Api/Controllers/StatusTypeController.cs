// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Web.Http;

using Marketplace.Models.Configuration;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Logic.Contracts.Configuration;

using Prolix.Api.Controllers;
using Prolix.Api.Extensions;

namespace Marketplace.Api.Controllers
{
    public class StatusTypeController : BaseController
    {
        #region Constructors

        public StatusTypeController(IStatusTypeService service)
        {
            StatusTypeService = service;
        }

        #endregion

        #region Properties

        IStatusTypeService StatusTypeService { get; }

        #endregion

        #region Endpoints

        //GET StatusType/1
        public IHttpActionResult Get(int id)
        {
            var item = StatusTypeService.Get(id);
            if (item == null)
                return NotFound();

            var model = item.Map<StatusTypeModel>();
            return Ok(model);
        }

        //GET /StatusType
        public IHttpActionResult Get()
        {
            // Query, ordering, pagination
            var list = StatusTypeService.List();

            // Maps to Api model
            var mapped = list.Map<StatusType, StatusTypeModel>();

            // Returns a 200 status with custom headers (paging)
            return Ok(mapped);
        }

        #endregion
    }
}
