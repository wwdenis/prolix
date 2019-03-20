// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;

namespace Marketplace.Domain.Models.Configuration
{
    public class Setting : ActiveNamedModel
    {
        public string Value { get; set; }
    }
}
