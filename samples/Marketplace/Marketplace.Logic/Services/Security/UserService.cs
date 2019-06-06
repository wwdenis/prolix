// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;

using Marketplace.Data;
using Marketplace.Domain.Models.Security;
using Marketplace.Domain.Security;
using Marketplace.Logic.Contracts.Security;
using Marketplace.Logic.Queries.Security;

using Prolix.Core.Collections;
using Prolix.Core.Identity;

namespace Marketplace.Logic.Services.Security
{
    public class UserService : RepositoryService<User>, IUserService
    {
        #region Constructor

        public UserService(IDataContext context, IIdentityManager manager, SecurityContext security) : base(context, security)
        {
        }

        #endregion

        #region Overriden Methods

        async public override Task Add(User model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Email == model.Email))
                Rule.Add("Email", "Email already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // New records are set to active
            model.Active = true;

            // Persist on database
            await base.Add(model);
        }

        async public override Task<bool> Update(User model)
        {
            // Simple validations
            Validate(model);

            // Custom validations
            if (Exists(i => i.Id != model.Id && i.Name == model.Name))
                Rule.Add("Name", "Name already taken");

            if (Exists(i => i.Id != model.Id && i.Email == model.Email))
                Rule.Add("Email", "Email already taken");

            // Throws an exception if there are validation errors
            CheckRule();

            // Retrieves data
            var current = Get(model.Id);

            if (current == null)
                return false;

            // Update fields
            current.Name = model.Name;
            current.Email = model.Email;
            current.RoleId = model.RoleId;
            current.Active = model.Active;

            // Persist on database
            return await base.Update(current);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get user from email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User Get(string email)
        {
            return Get(i => i.Email == email);
        }

        /// <summary>
        /// Search by criteria
        /// </summary>
        /// <param name="request">Filter, pagination, sorting parameters</param>
        /// <returns>Paged result (sorted)</returns>
        public PagedList<User> List(UserQuery request)
        {
            var query = base.List();

            // Build que query
            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(i => i.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.Email))
                query = query.Where(i => i.Email == request.Email);

            if (request.RoleId.HasValue)
                query = query.Where(i => i.RoleId == request.RoleId.Value);

            // Apply paging and sorting
            var result = query.ToPaged(request);

            return result;
        }

        #endregion
    }
}
