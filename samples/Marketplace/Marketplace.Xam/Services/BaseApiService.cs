using Marketplace.Xam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wwa.Core.Domain;
using Wwa.Http.Client;

namespace Marketplace.Xam.Services
{
    public class BaseApiService<ModelType> : ApiService<ModelType>
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
