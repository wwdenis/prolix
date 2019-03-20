// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using Prolix.Core.Domain;

namespace Marketplace.Domain.Models.Geography
{
    public class Country : ActiveNamedModel
    {
        public string Abbreviation { get; set; }

        public virtual IList<Province> Provinces { get; } = new List<Province>();
    }
}
