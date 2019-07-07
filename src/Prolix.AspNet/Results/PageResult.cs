// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Prolix.AspNet.Extensions;
using Prolix.Collections;

namespace Prolix.AspNet.Results
{
    public class PageResult<T> : OkNegotiatedContentResult<PagedList<T>>
        where T : class
    {
        public PageResult(PagedList<T> content, ApiController controller) : base(content, controller)
        {
        }

        public PageResult(PagedList<T> content, IContentNegotiator contentNegotiator, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters) : base(content, contentNegotiator, request, formatters)
        {
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var message = await base.ExecuteAsync(cancellationToken);
            
            if (Content != null)
            {
                var headers = new Dictionary<string, object>
                {
                    { "X-Page-Count", Content.PageCount },
                    { "X-Page-Number", Content.PageNumber },
                    { "X-Page-Size", Content.PageSize },
                    { "X-Page-Records", Content.RecordCount }
                };

                message.Content = Request.GetContent(Content.Items);

                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, $"{header.Value}");
                }
            }

            return message;
        }
    }
}
