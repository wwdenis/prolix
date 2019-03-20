// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using Prolix.Core.Extensions.Parsing;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Model change entry
    /// </summary>
    public sealed class ModelAudit
    {
        public ModelAudit()
        {
        }

        public ModelAudit(string name, object newValue, object oldValue)
        {
            Name = name;
            NewValue = newValue.ToFriendly();
            OldValue = oldValue.ToFriendly();
        }

        public string Name { get; set; }
        public string NewValue { get; set; }
        public string OldValue { get; set; }
    }
}
