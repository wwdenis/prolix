// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Logic;

namespace Marketplace.Domain.Descriptors.Security
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class PasswordChangeDescriptor : ModelDescriptor<PasswordChange>
    {
        public PasswordChangeDescriptor()
        {
            Model("Passsword Change");

            Field(i => i.IdentityId)
                .Caption("Credentials")
                .Required("Invalid credentials");

            Field(i => i.OldPassword)
                .Caption("Old Password")
                .MaxLength(20)
                .Required();

            Field(i => i.NewPassword)
                .Caption("New Password")
                .MaxLength(20)
                .Required();

            Field(i => i.ConfirmPassword)
                .Caption("Confirm Password")
                .MaxLength(20)
                .Required();

            Validate(i => i.OldPassword != i.NewPassword, "Old and new passwords must NOT be the same.");
            Validate(i => i.NewPassword == i.ConfirmPassword, "New and confirm passwords must be the same.");
        }
    }
}
