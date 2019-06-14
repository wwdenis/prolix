// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Domain;

namespace Marketplace.Domain.Models.Geography
{
    public class Province : ActiveNamedModel
    {
        public string Abbreviation { get; set; }
        public int? CountryId { get; set; }

        public Country Country { get; set; }
    }
}
