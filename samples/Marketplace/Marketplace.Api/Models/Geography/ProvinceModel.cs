// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Wwa.Core.Domain;

namespace Marketplace.Api.Models.Geography
{
    public class ProvinceModel : NamedModel
    {
        public string Abbreviation { get; set; }
        public string CountryName { get; set; }
    }
}