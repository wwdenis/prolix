// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Marketplace.Domain.Models.Configuration;
using Prolix.Core.Logic;

namespace Marketplace.Logic.Contracts.Configuration
{
    public interface ISettingService : IUpdatableService<Setting>
    {
        ApplicationSetting Get();
    }
}
