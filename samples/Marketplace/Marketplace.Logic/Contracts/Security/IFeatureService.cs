// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Security;
using Prolix.Core.Logic;

namespace Marketplace.Logic.Contracts.Security
{
    public interface IFeatureService : IRepositoryService<Feature>
    {
        Feature Get(string path);
    }
}
