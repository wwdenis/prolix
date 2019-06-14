// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Configuration;
using Marketplace.Logic.Contracts.Security;

using Prolix.Identity;
using Prolix.Logic;

namespace Marketplace.Logic.Services.Security
{
    public class IdentityService : IIdentityService
    {
        #region Constructor

        public IdentityService(IIdentityManager manager, IDataContext context, IUserService userService, ISettingService settingService)
        {
            Manager = manager;
            Context = context;
            UserService = userService;
            SettingService = settingService;
        }

        #endregion

        #region Properties

        IIdentityManager Manager { get; }
        IDataContext Context { get; }
        IUserService UserService { get; }
        ISettingService SettingService { get; }
        
        #endregion

        #region Async Methods

        async public Task<Access> Register(Register request)
        {
            var rule = DescriptorManager.Validate(request);
            rule.Check();
            
            var found = await Manager.Get(null, request.Email);

            if (found != null)
                rule.Add("Email", "Email already taken");

            rule.Check();

            var settings = SettingService.Get();
            var customerRoleId = settings?.CustomerRoleId;

            if (customerRoleId == null)
                throw new InvalidOperationException("Customer Role nor configured");

            // Start a transaction
            Context.Start();

            string token = string.Empty;
            IdentityAccount account = null;

            try
            {
                token = await Manager.Register(request.Email, request.Password);
                account = await Manager.Get(null, request.Email);
            }
            catch (IdentityException ex)
            {
                Context.Rollback();

                string message = ex.TranslatedReason();
                throw new RuleException(message);
            }

            var user = new User
            {
                Email = request.Email,
                Name = request.FullName,
                RoleId = customerRoleId,
                IdentityId = account?.Id
            };

            // Adds the user
            await UserService.Add(user);

            // Commmit the transaction
            Context.Commit();

            var result = new Access
            {
                UserName = request.Email,
                FullName = request.FullName,
                Token = token
            };

            return result;
        }

        async public Task<Access> Login(LoginRequest request)
        {
            var rule = DescriptorManager.Validate(request);
            rule.Check();

            string token = string.Empty;
            
            try
            {
                token = await Manager.Login(request.UserName, request.Password);
            }
            catch (IdentityException ex)
            {
                string message = ex.TranslatedReason();
                throw new RuleException(message);
            }

            var user = UserService.Get(request.UserName);

            var result = new Access
            {
                UserName = request.UserName,
                FullName = user?.Name,
                Token = token
            };

            return result;
        }

        async public Task<bool> ChangePassword(PasswordChange request)
        {
            if (string.IsNullOrWhiteSpace(request?.IdentityId))
                throw new ArgumentNullException(nameof(PasswordChange.IdentityId));

            var rule = DescriptorManager.Validate(request);
            rule.Check();

            try
            {
                await Manager.ChangePassword(request.IdentityId, request.OldPassword, request.NewPassword);
            }
            catch (IdentityException ex)
            {
                var message = ex.TranslatedReason();
                throw new RuleException(message);
            }

            return true;
        }

        async public Task<bool> ResetPassword(PasswordReset request)
        {
            if (string.IsNullOrWhiteSpace(request?.IdentityId))
                throw new ArgumentNullException(nameof(PasswordChange.IdentityId));

            var rule = DescriptorManager.Validate(request);
            rule.Check();

            try
            {
                await Manager.ResetPassword(request.IdentityId, request.NewPassword);
            }
            catch (IdentityException ex)
            {
                var message = ex.TranslatedReason();
                throw new RuleException(message);
            }

            return true;
        }

        public void Logout()
        {
            Manager.Logout();
        }

        #endregion
    }
}
