// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Marketplace.Client.Models.Security
{
    public class AccessModel
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string FullName { get; set; }
    }
}
