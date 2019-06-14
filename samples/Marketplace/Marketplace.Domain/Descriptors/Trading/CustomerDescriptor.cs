// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Trading;
using Prolix.Logic;

namespace Marketplace.Domain.Descriptors.Trading
{
    /// <summary>
    /// Business rules and metadata
    /// </summary>
    public class CustomerDescriptor : ModelDescriptor<Customer>
    {
        public CustomerDescriptor()
        {
            Model("Customer");

            Field(i => i.Active)
                .Caption("Active");

            Field(i => i.Name)
                .Caption("Name")
                .MaxLength(50)
                .Required();

            Field(i => i.PhoneNumber)
                .Caption("Phone Number")
                .MaxLength(50)
                .Required();

            Field(i => i.Address)
                .Caption("Street Address")
                .Required();

            Field(i => i.City)
                .Caption("City")
                .Required();

            Field(i => i.ZipCode)
                .Caption("Zip Code")
                .Required();

            Field(i => i.CountryId)
                .Caption("Country")
                .Required();

            Field(i => i.ProvinceId)
                .Caption("Province")
                .Required();
        }
    }
}
