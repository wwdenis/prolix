// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace Prolix.Identity.AspNet
{
    /// <summary>
    /// Enables bearer authentication through Query String
    /// </summary>
    public class BearerTokenProvider : OAuthBearerAuthenticationProvider
    {
        #region Fields

        /// <summary>
        /// The query string key name used for authentication
        /// </summary>
        readonly string _queryKey;

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new BearerTokenProvider
        /// </summary>
        /// <param name="queryKey">The query string key name used for authentication</param>
        public BearerTokenProvider(string queryKey)
        {
            _queryKey = queryKey;
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Handles processing OAuth bearer token.
        /// </summary>
        async public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context?.Request?.Query?.Get(_queryKey);

            if (!string.IsNullOrWhiteSpace(value))
                context.Token = value;
            
            await base.RequestToken(context);
        }

        #endregion
    }
}
