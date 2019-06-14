using Marketplace.Client.Models;

using Prolix.Domain;
using Prolix.Client.Api;

namespace Marketplace.Client.Services
{
    public abstract class BaseApiService<ModelType> : ApiService<ModelType>
        where ModelType : Model, new()
    {
        public BaseApiService(ApplicationContext context)
        {
            Context = context;
            BaseUrl = context.BaseUrl;
        }

        ApplicationContext Context { get; }
    }
}
