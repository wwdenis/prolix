// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Client.Models.Security;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Security;
using Marketplace.Logic.Queries.Security;

using Prolix.AspNet.Controllers;
using Prolix.AspNet.Extensions;

namespace Marketplace.Api.Controllers
{
    public class UserController : BaseController
    {
        #region Constructors

        public UserController(IUserService service)
        {
            UserService = service;
        }

        #endregion

        #region Properties

        IUserService UserService { get; }

        #endregion

        #region Endpoints

        //GET User/1
        public IHttpActionResult Get(int id)
        {
            var item = UserService.Get(id);
            if (item == null)
                return NotFound();

            var model = item.Map<UserModel>();
            return Ok(model);
        }

        //GET /User
        public IHttpActionResult Get([FromUri] UserQuery request)
        {
            // Query, ordering, pagination
            var list = UserService.List(request);

            // Maps to Api model
            var mapped = list.Map<User, UserModel>();

            // Returns a 200 status with custom headers (paging)
            return Page(mapped);
        }

        //PUT: /User/1
        async public Task<IHttpActionResult> Put(int id, UserModel model)
        {
            var item = model.Map<User>(id);
            
            bool success = await UserService.Update(item);

            if (!success)
                return NotModified();

            return Ok();
        }

        //Post: /User
        async public Task<IHttpActionResult> Post(UserModel model)
        {
            var item = model.Map<User>();

            // Run business rules, save to the database
            await UserService.Add(item);

            // HTTP Status 201
            return CreatedAt(item.Id);
        }

        // DELETE: User/5     
        async public Task<IHttpActionResult> Delete(int id)
        {
            await UserService.Delete(id);

            return Ok();
        }
        
        #endregion
    }
}
