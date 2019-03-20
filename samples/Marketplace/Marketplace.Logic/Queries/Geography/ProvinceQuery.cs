// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Geography;
using Prolix.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Logic.Queries.Geography
{
    public class ProvinceQuery : QueryRequest<Province>
    {
        public ProvinceQuery()
        {
            // Map sort expressions
            MapSort("Name", i => i.Name);
            MapSort("CountryName", i => i.Country.Name);
            MapSort("Abbreviation", i => i.Abbreviation);

            SortField = "Name";
        }

        public string Name { get; set; }
        public int? CountryId { get; set; }
    }
}
