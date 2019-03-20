// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Logic;

namespace Marketplace.Domain.Descriptors.Security
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class LoginDescriptor : ModelDescriptor<LoginRequest>
    {
        public LoginDescriptor()
        {
            Model("Login");

            Field(i => i.UserName)
                .Caption("Email")
                .MaxLength(100)
                .Required();

            Field(i => i.Password)
                .Caption("Password")
                .MaxLength(20)
                .Required();
        }
    }
}
