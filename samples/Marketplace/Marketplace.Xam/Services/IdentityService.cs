using System;
using System.Net;
using System.Threading.Tasks;

using Marketplace.Models.Security;
using Marketplace.Xam.Models;

using Prolix.Core.Http;
using Prolix.Http.Client;
using Prolix.Http.Extensions;

namespace Marketplace.Xam.Services
{
    public class IdentityService : IIdentityService
    {
        public IdentityService(ApplicationContext context, IRestService restService)
        {
            RestService = restService;
            Context = context;
            RestService.BaseUrl = context.BaseUrl;
        }

        IRestService RestService { get; }
        ApplicationContext Context { get; }

        async public Task<AccessModel> Login(LoginModel model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("model");

                var body = new HttpBody<LoginModel>();
                var response = await RestService.Post<AccessModel, LoginModel>("Login", body);
                var result = response.Content;

                Context.Credentials = result;

                return result;
            }
            catch (HttpException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    return null;

                ex.CheckRule();
                throw;
            }
        }
    }
}
