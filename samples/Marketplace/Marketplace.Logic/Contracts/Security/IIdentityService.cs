// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using System.Threading.Tasks;

using Prolix.Ioc;

namespace Marketplace.Logic.Contracts.Security
{
    public interface IIdentityService : IService
    {
        Task<Access> Register(Register model);

        Task<Access> Login(LoginRequest model);

        Task<bool> ChangePassword(PasswordChange request);

        Task<bool> ResetPassword(PasswordReset request);

        void Logout();
    }
}
