// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Identity
{
    [Flags]
    public enum IdentityError
    {
        Generic = 0,
        AccountNotFound = 1,
        AccountNotActive = 2,
        AccountLocked = 4,
        InvalidPassword = 8
    }
}
