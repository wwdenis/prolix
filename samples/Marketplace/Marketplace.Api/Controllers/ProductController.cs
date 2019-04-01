// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Client.Models.Trading;
using Marketplace.Domain.Models.Trading;
using Marketplace.Logic.Contracts.Trading;
using Marketplace.Logic.Queries.Trading;

using Prolix.Api.Controllers;
using Prolix.Api.Extensions;

namespace Marketplace.Api.Controllers
{
    public class ProductController : BaseController
    {
        #region Constructors

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        #endregion

        #region Properties

        IProductService ProductService { get; }

        #endregion

        #region Endpoints

        //GET Product/1
        public IHttpActionResult Get(int id)
        {
            var item = ProductService.Get(id);

            if (item == null)
                return NotFound();

            var model = item.Map<ProductModel>();
            return Ok(model);
        }

        //GET /Product
        public IHttpActionResult Get([FromUri] ProductQuery request)
        {
            // Query, ordering, pagination
            var list = ProductService.List(request);

            // Maps to Api model
            var mapped = list.Map<Product, ProductModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /Product/1
        async public Task<IHttpActionResult> Put(int id, ProductModel model)
        {
            var item = model.Map<Product>(id);
            
            bool success = await ProductService.Update(item);

            if (!success)
                return NotModified();

            return Ok();
        }

        //Post: /Product
        async public Task<IHttpActionResult> Post(ProductModel model)
        {
            var item = model.Map<Product>();

            // Run business rules, save to the database
            await ProductService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: Product/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await ProductService.Delete(id);

            return Ok();
        }
        
        #endregion
    }
}
