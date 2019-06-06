// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Security;
using Marketplace.Domain.Models.Security;
using Marketplace.Logic.Contracts.Security;

namespace Marketplace.Logic.Services.Security
{
    public sealed class RoleService : RepositoryService<Role>, IRoleService
    {
        #region Constructors

        public RoleService(IDataContext context, SecurityContext security) : base(context, security)
        {
        }

        #endregion

        #region Overriden Methods

        async public override Task Add(Role model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // New records are set to active
            model.Active = true;

            // Persist on database
            await base.Add(model);
        }

        async public override Task<bool> Update(Role model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Id != model.Id && i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // Retrieves data
            var current = Get(model.Id);

            if (current == null)
                return false;

            // Update fields
            current.Name = model.Name;
            current.IsAdmin = model.IsAdmin;
            current.Active = model.Active;

            // Persist on database
            return await base.Update(current);
        }

        async public override Task<bool> Delete(Role model)
        {
            if (model.Users.Any())
                Rule.Throw("This record can't be deleted because it has child records.");

            return await base.Delete(model);
        }

        public override IQueryable<Role> List()
        {
            return base.List().Where(i => i.Active);
        }

        #endregion
    }
}
