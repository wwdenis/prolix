using Marketplace.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolix.Core.Http;
using Prolix.Core.Ioc;

namespace Marketplace.Xam.Services
{
    public interface ICategoryService : IApiService<CategoryModel>
    {
    }
}
