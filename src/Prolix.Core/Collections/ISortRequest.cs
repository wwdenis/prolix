// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Core.Collections
{
    public interface ISortRequest
    {
        string SortField { get; set; }
        bool SortDescending { get; set; }
    }
}
