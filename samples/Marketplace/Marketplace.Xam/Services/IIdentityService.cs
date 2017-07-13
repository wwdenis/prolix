using Marketplace.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wwa.Core.Ioc;

namespace Marketplace.Xam.Services
{
    public interface IIdentityService : IService
    {
        Task<AccessModel> Login(LoginModel model);
    }
}
