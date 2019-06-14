// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Logic;

namespace Marketplace.Domain.Descriptors.Security
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class PasswordResetDescriptor : ModelDescriptor<PasswordReset>
    {
        public PasswordResetDescriptor()
        {
            Model("Passsword Reset");

            Field(i => i.IdentityId)
                .Caption("Credentials")
                .Required("Invalid credentials");

            Field(i => i.NewPassword)
                .Caption("New Password")
                .MaxLength(20)
                .Required();
        }
    }
}
