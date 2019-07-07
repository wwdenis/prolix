using Marketplace.Client.Models;

using Prolix.Domain;
using Prolix.Client.Api;

namespace Marketplace.Client.Services
{
    public abstract class BaseApiService<T> : ApiService<T>
        where T : Model, new()
    {
        public BaseApiService(ApplicationContext context)
        {
            Context = context;
            BaseUrl = context.BaseUrl;
        }

        ApplicationContext Context { get; }
    }
}
