// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Prolix.Core.Ioc;

namespace Prolix.Core.Identity
{
    public interface IIdentityManager : IService
    {
        Task<IdentityAccount> Get(string id, string userName);
        Task Add(IdentityAccount account);
        Task Update(IdentityAccount account);
        Task Delete(IdentityAccount account);

        Task<string> Register(string userName, string password);
        Task<string> Login(string userName, string password);
        void Logout();
        string IssueToken(string id);

        Task Lock(string id);
        Task Unlock(string id);
        Task ChangePassword(string id, string oldPassword, string newPassword);
        Task ResetPassword(string id, string newPassword);
    }
}
