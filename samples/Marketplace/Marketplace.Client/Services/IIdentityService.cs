using Marketplace.Client.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolix.Core.Ioc;

namespace Marketplace.Client.Services
{
    public interface IIdentityService : IService
    {
        Task<AccessModel> Login(LoginModel model);
    }
}
