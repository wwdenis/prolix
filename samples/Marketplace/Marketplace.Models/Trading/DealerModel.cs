// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Prolix.Core.Domain;

namespace Marketplace.Models.Trading
{
    public class DealerModel : ActiveNamedModel
    {
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public int? UserId { get; set; }

        public string ProvinceAbbreviation { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }
        public string UserName { get; set; }
    }
}
