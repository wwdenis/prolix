// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;

using Prolix.Api.Extensions;
using Prolix.Core.Logic;

namespace Prolix.Api.Results
{
    public class RuleValidationResult : BadRequestResult
    {
        public RuleValidation Rule { get; }

        public RuleValidationResult(HttpRequestMessage request, RuleValidation rule) : base(request)
        {
            Rule = rule ?? throw new ArgumentNullException("rule"); 
        }

        async public override Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var message = await base.ExecuteAsync(cancellationToken);

            message.Content = Request.GetContent(Rule);

            return message;
        }
    }
}
