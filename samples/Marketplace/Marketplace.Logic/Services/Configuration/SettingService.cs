// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

using Marketplace.Data;
using Marketplace.Domain.Models.Configuration;
using Marketplace.Domain.Models.Security;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Configuration;
using Marketplace.Logic.Contracts.Security;

namespace Marketplace.Logic.Services.Configuration
{
    public sealed class SettingService : RepositoryService<Setting>, ISettingService
    {
        public SettingService(IDataContext context, SecurityContext security, IRoleService roleService) : base(context, security)
        {
            RoleService = roleService;
        }

        IRoleService RoleService { get; }

        public Setting Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return Get(i => i.Name.ToLower() == name.ToLower());
        }

        public ApplicationSetting Get()
        {
            const string CustomerRoleSetting = "CustomerRole";

            var customerRoleSetting = Get(CustomerRoleSetting);
            var customerRoleName = customerRoleSetting?.Value;

            Role customerRole = null;

            if (!string.IsNullOrWhiteSpace(customerRoleName))
                customerRole = RoleService.Get(i => i.Name.ToLower() == customerRoleName.ToLower());

            if (customerRole.IsAdmin)
                throw new InvalidOperationException("Customer can't be Admin");

            var result = new ApplicationSetting
            {
                CustomerRoleId = customerRole?.Id
            };

            return result;
        }
    }
}
