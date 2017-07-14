// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Models.Trading;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Wwa.Api.Controllers;
using Wwa.Api.Extensions;

namespace Marketplace.Api.Controllers
{
    public class CustomerController : BaseController
    {
        #region Constructors

        public CustomerController(ICustomerService service)
        {
            CustomerService = service;
        }

        #endregion

        #region Properties

        ICustomerService CustomerService { get; }

        #endregion

        #region Endpoints

        //GET Customer/1
        public IHttpActionResult Get(int id)
        {
            var item = CustomerService.Get(id);
            if (item == null)
                return NotFound();

            var model = item.Map<CustomerModel>();
            return Ok(model);
        }

        //GET /Customer
        public IHttpActionResult Get([FromUri] CustomerQuery request)
        {
            // Query, ordering, pagination
            var list = CustomerService.List(request);

            // Maps to Api model
            var mapped = list.Map<Customer, CustomerModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /Customer/1
        async public Task<IHttpActionResult> Put(int id, CustomerModel model)
        {
            var item = model.Map<Customer>(id);

            bool success = await CustomerService.Update(item);

            if (!success)
                return NotModified();

            return Ok();
        }

        //Post: /Customer
        async public Task<IHttpActionResult> Post(CustomerModel model)
        {
            var item = model.Map<Customer>();

            // Run business rules, save to the database
            await CustomerService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: Customer/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await CustomerService.Delete(id);

            return Ok();
        }

        #endregion
    }
}
