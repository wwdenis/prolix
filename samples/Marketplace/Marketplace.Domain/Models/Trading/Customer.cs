// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Geography;
using Marketplace.Domain.Models.Security;
using System.Collections.Generic;
using Prolix.Core.Domain;

namespace Marketplace.Domain.Models.Trading
{
    public class Customer : ActiveNamedModel
    {
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public int? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public int? UserId { get; set; }

        public virtual Province Province { get; set; }
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }

        public virtual IList<Order> Orders { get; } = new List<Order>();
    }
}
