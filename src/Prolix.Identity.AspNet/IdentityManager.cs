// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

using System;
using System.Threading.Tasks;

using Prolix.Core.Identity;

namespace Prolix.Identity.AspNet
{
    public class IdentityManager : IIdentityManager
    {
        public IdentityManager(IOwinContext owinContext)
        {
            OwinContext = owinContext;
        }

        IOwinContext OwinContext { get; }
        UserManager<IdentityUser> UserManager => OwinContext?.GetUserManager<UserManager<IdentityUser>>();
        IAuthenticationManager AuthManager => OwinContext?.Authentication;

        async public Task<IdentityAccount> Get(string id, string userName)
        {
            IdentityUser user = null;

            if (string.IsNullOrWhiteSpace(id))
                user = await UserManager.FindByIdAsync(id);
            else if (string.IsNullOrWhiteSpace(userName))
                user = await UserManager.FindByNameAsync(userName);

            IdentityAccount result = null;

            if (user != null)
            {
                result = new IdentityAccount(user.Id, user.UserName);
                result.IsLocked = await UserManager.IsLockedOutAsync(user.Id);
                result.HasPassword = await UserManager.HasPasswordAsync(user.Id);
            }

            return result;
        }

        async public Task Add(IdentityAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var user = new IdentityUser
            {
                UserName = account.UserName,
                Email = account.UserName
            };

            IdentityResult result = await UserManager.CreateAsync(user);
            
            if (!result.Succeeded)
                throw new IdentityException(result.Errors);
        }

        async public Task Update(IdentityAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var user = new IdentityUser
            {
                UserName = account.UserName,
                Email = account.UserName
            };

            IdentityResult result = await UserManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);
        }

        async public Task Delete(IdentityAccount account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var user = new IdentityUser
            {
                UserName = account.UserName,
                Email = account.UserName
            };

            IdentityResult result = await UserManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);
        }

        async public Task<string> Register(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var user = new IdentityUser
            {
                UserName = userName,
                Email = userName
            };

            IdentityResult result = await UserManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new IdentityException(result.Errors);

            await Unlock(user.Id);

            string token = IssueToken(user.Id);

            return  token;
        }

        async public Task<string> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));    
                    
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var user = await UserManager.FindByNameAsync(userName);

            if (user == null)
            {
                throw new IdentityException(IdentityError.AccountNotFound);
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                throw new IdentityException(IdentityError.AccountNotActive);
            }

            if (user.LockoutEnabled)
            {
                throw new IdentityException(IdentityError.AccountLocked);
            }

            var success = await UserManager.CheckPasswordAsync(user, password);

            if (success)
            {
                await Unlock(user.Id);
            }
            else
            {
                var attempts = user.AccessFailedCount + 1;
                IdentityError error = IdentityError.Generic;

                if (attempts >= UserManager.MaxFailedAccessAttemptsBeforeLockout)
                {
                    await UserManager.SetLockoutEnabledAsync(user.Id, true);
                    error = IdentityError.AccountLocked;
                }
                else
                {
                    await UserManager.AccessFailedAsync(user.Id);
                    error = IdentityError.InvalidPassword;
                }

                await UserManager.UpdateAsync(user);
                throw new IdentityException(error);
            }

            var token = IssueToken(user.Id);

            return token;
        }

        public void Logout()
        {
            AuthManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        }

        public string IssueToken(string id)
        {
            string userId = $"{id}";
            return IdentityServer.IssueToken(userId);
        }

        async public Task Lock(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEnabledAsync(id, true);
        }

        async public Task Unlock(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            await UserManager.ResetAccessFailedCountAsync(id);
            await UserManager.SetLockoutEnabledAsync(id, false);
        }

        async public Task ChangePassword(string id, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(oldPassword))
                throw new ArgumentNullException(nameof(oldPassword));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));
            
            if (string.Equals(oldPassword, newPassword))
                throw new IdentityException(IdentityError.InvalidPassword);

            IdentityResult result = await UserManager.ChangePasswordAsync(id, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors);
            }
        }

        async public Task ResetPassword(string id, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException(nameof(newPassword));

            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                throw new IdentityException(IdentityError.AccountNotFound);

            var hasPassword = await UserManager.HasPasswordAsync(id);
            IdentityResult result = null;

            if (hasPassword)
            {
                if (UserManager.UserTokenProvider != null)
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    result = await UserManager.ResetPasswordAsync(user.Id, token, newPassword);
                }
                else
                {
                    string newHash = UserManager.PasswordHasher.HashPassword(newPassword);
                    user.PasswordHash = newHash;
                    result = await UserManager.UpdateAsync(user);
                }
            }
            else
            {
                result = await UserManager.AddPasswordAsync(user.Id, newPassword);
            }

            if (!result.Succeeded)
            {
                throw new IdentityException(result.Errors);
            }
        }
    }
}
