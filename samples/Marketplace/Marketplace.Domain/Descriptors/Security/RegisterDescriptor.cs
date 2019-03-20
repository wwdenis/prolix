// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Logic;

namespace Marketplace.Domain.Descriptors.Security
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public sealed class RegisterDescriptor : ModelDescriptor<Register>
    {
        public RegisterDescriptor()
        {
            Model("Register");

            Field(i => i.FullName)
                .Caption("Full Name")
                .MaxLength(100)
                .Required();

            Field(i => i.Email)
                .Caption("Email")
                .MaxLength(100)
                .Required();

            Field(i => i.Password)
                .Caption("Password")
                .MaxLength(20)
                .Required();

            Field(i => i.ConfirmPassword)
                .Caption("Confirm Password")
                .MaxLength(20)
                .Required();

            Validate(i => i.Password == i.ConfirmPassword, "Both passwords must be the same.");
        }
    }
}
