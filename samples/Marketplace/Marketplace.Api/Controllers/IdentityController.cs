// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Threading.Tasks;
using System.Web.Http;

using Marketplace.Client.Models.Security;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Security;

using Prolix.Api.Controllers;
using Prolix.Api.Extensions;
using Prolix.Api.Filters;

namespace Marketplace.Api.Controllers
{
    [RoutePrefix("api")]
    public class IdentityController : BaseController
    {
        public IdentityController(IIdentityService identityService)
        {
            IdentiyService = identityService;
        }

        private IIdentityService IdentiyService { get; set;}
        
        // POST /Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        async public Task<IHttpActionResult> Login(LoginModel model)
        {
            if (model == null)
                return BadRequest("Invalid credentials");

            var item = model.Map<LoginRequest>();
            
            var result = await IdentiyService.Login(item);
            var parsed = result.Map<AccessModel>();

            return Ok(parsed);
        }

        // POST /Register
        [AllowAnonymous]
        [OnlyLocal]
        [Route("Register")]
        async public Task<IHttpActionResult> Register(RegisterModel model)
        {
            if (model == null)
                return BadRequest("Invalid credentials");

            var item = model.Map<Register>();

            var result = await IdentiyService.Register(item);
            var parsed = result.Map<AccessModel>();

            return Ok(parsed);
        }

        // POST /ChangePassword
        [Route("ChangePassword")]
        async public Task<IHttpActionResult> ChangePassword(PasswordChangeModel model)
        {
            if (model == null)
                return BadRequest("Invalid credentials");

            var item = model.Map<PasswordChange>();
            item.IdentityId = User.Identity.Name;

            await IdentiyService.ChangePassword(item);

            return Ok();
        }

        // POST /ResetPassword
        [Route("ResetPassword")]
        [AllowAnonymous]
        [OnlyLocal]
        async public Task<IHttpActionResult> ResetPassword(PasswordResetModel model)
        {
            if (model == null)
                return BadRequest("Invalid credentials");

            var item = model.Map<PasswordReset>();
            
            await IdentiyService.ResetPassword(item);
            
            return Ok();
        }

        // POST /Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            IdentiyService.Logout();
            return Ok(true);
        }
    }
}
