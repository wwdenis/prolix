// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;

namespace Marketplace.Client.Models.Geography
{
    public class ProvinceModel : NamedModel
    {
        public string Abbreviation { get; set; }
        public string CountryName { get; set; }
    }
}
