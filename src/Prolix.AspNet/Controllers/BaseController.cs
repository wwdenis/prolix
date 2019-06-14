// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Prolix.AspNet.Extensions;
using Prolix.AspNet.Results;
using Prolix.Collections;

namespace Prolix.AspNet.Controllers
{
    public class BaseController : ApiController
    {
        /// <summary>
        /// Creates a (201 Created) response with the specified values.
        /// </summary>
        /// <typeparam name="T">The type of content in the entity body.</typeparam>
        /// <param name="content">The content value to negotiate and format in the entity body.</param>
        /// <returns></returns>
        protected IHttpActionResult CreatedAt<T>(T content)
        {
            var routeName = Configuration.GetDefaultRouteName();

            if (string.IsNullOrWhiteSpace(routeName))
                return Ok();

            return CreatedAtRoute(routeName, new { id = content }, content);
        }

        /// <summary>
        /// Creates a (304 NotModified) response with the specified values.
        /// This status is sent when an update operation changed nothing.
        /// </summary>
        /// <returns></returns>
        protected IHttpActionResult NotModified()
        {
            return StatusCode(HttpStatusCode.NotModified);
        }

        /// <summary>
        /// Creates a (200 Ok) response with the specified values.
        /// </summary>
        /// <returns></returns>
        protected IHttpActionResult Page<ModelType>(PagedList<ModelType> page)
            where ModelType : class
        {
            return new PageResult<ModelType>(page, this);
        }
    }
}
