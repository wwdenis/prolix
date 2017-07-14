// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Wwa.Core.Identity
{
    public class IdentityAccount
    {
        public IdentityAccount()
        {

        }

        public IdentityAccount(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsLocked { get; set; }
        public bool HasPassword { get; set; }
    }
}
