// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.


namespace Prolix.Collections
{
    /// <summary>
    /// Page info
    /// </summary>
    public interface IPageInfo : IPageRequest
    {
        int PageCount { get; }
        int RecordCount { get; }
    }
}
