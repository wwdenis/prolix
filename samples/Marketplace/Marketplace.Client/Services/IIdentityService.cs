using System.Threading.Tasks;

using Marketplace.Client.Models.Security;
using Prolix.Core.Ioc;

namespace Marketplace.Client.Services
{
    public interface IIdentityService : IService
    {
        Task<AccessModel> Login(LoginModel model);
    }
}
