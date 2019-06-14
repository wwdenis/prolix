// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Client.Models.Trading;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.AspNet.Controllers;
using Prolix.AspNet.Extensions;

namespace Marketplace.Api.Controllers
{
    public class OrderController : BaseController
    {
        #region Constructors

        public OrderController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        #endregion

        #region Properties

        IOrderService OrderService { get; }

        #endregion

        #region Endpoints

        //GET Order/1
        public IHttpActionResult Get(int id)
        {
            var item = OrderService.Get(id);
            if (item == null)
                return NotFound();

            // No Items
            var model = item.Map<DetailOrderModel>();

            return Ok(model);
        }

        //GET /Order
        public IHttpActionResult Get([FromUri] OrderQuery request)
        {
            // Query, ordering, pagination
            var list = OrderService.List(request);

            // Maps to Api model
            var mapped = list.Map<Order, OrderModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /Order/1
        async public Task<IHttpActionResult> Put(int id, DetailOrderModel model)
        {
            var item = model.Map<Order>(id);
            
            bool success = await OrderService.Update(item);

            if (!success)
                return NotModified();

            return Ok();
        }

        //Post: /Order
        async public Task<IHttpActionResult> Post(DetailOrderModel model)
        {
            var item = model.Map<Order>();

            // Run business rules, save to the database
            await OrderService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: Order/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await OrderService.Delete(id);

            return Ok();
        }
        
        #endregion
    }
}
