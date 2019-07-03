// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Security.Principal;

using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Security;

using Prolix.AspNet.Filters;

namespace Marketplace.Api.Infrastructure.Filters
{
    internal class PermissionAttribute : PermissionBaseAttribute
    {
        public PermissionAttribute(SecurityContext security, IUserService userService, IFeatureService featureService)
        {
            Security = security;
            UserService = userService;
            FeatureService = featureService;
        }

        IUserService UserService { get; }
        IFeatureService FeatureService { get; }
        SecurityContext Security { get; }

        protected override bool Evaluate(IIdentity identity, string route, string method)
        {
            var path = $"{route}/{method}";
            var identityId = identity?.Name;
            
            // Check feature premission
            User user = UserService.Get(i => i.IdentityId == identityId);

            bool isAdmin = user?.Role.IsAdmin ?? false;
            var permissions = user?.Role?.Permissions ?? new Feature[0];

            // Admin has full access
            bool allowAccess = isAdmin || permissions.Any(i => i.Path.ToLower() == path.ToLower());

            Security.Feature = FeatureService.Get(path);
            Security.User = user;
            
            return allowAccess;
        }
    }
}
