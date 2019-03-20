// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Core.Collections
{
    public interface IPageRequest
    {
        int PageNumber { get;  }
        int PageSize { get; }
    }
}
