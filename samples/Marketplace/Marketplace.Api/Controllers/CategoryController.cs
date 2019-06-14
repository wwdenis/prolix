// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Client.Models.Configuration;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Logic.Contracts.Configuration;
using Marketplace.Logic.Queries.Configuration;

using Prolix.AspNet.Controllers;
using Prolix.AspNet.Extensions;

namespace Marketplace.Api.Controllers
{
    public class CategoryController : BaseController
    {
        #region Constructors

        public CategoryController(ICategoryService unitService)
        {
            CategoryService = unitService;
        }

        #endregion

        #region Properties

        ICategoryService CategoryService { get; }

        #endregion

        #region Endpoints

        //GET Category/1
        public IHttpActionResult Get(int id)
        {
            var item = CategoryService.Get(id);
            if (item == null)
                return NotFound();

            var model = item.Map<CategoryModel>();
            return Ok(model);
        }

        //GET /Category
        public IHttpActionResult Get([FromUri] CategoryQuery request)
        {
            // Query, ordering, pagination
            var list = CategoryService.List(request);

            // Maps to Api model
            var mapped = list.Map<Category, CategoryModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /Category/1
        async public Task<IHttpActionResult> Put(int id, CategoryModel model)
        {
            var item = model.Map<Category>(id);
            
            bool success = await CategoryService.Update(item);

            if (!success)
                return NotModified();

            return Ok();
        }

        //Post: /Category
        async public Task<IHttpActionResult> Post(CategoryModel model)
        {
            var item = model.Map<Category>();

            // Run business rules, save to the database
            await CategoryService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: Category/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await CategoryService.Delete(id);

            return Ok();
        }
        
        #endregion
    }
}
