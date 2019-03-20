// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Net;
using Prolix.Http.Extensions;

namespace Prolix.Http.Client
{
	/// <summary>
	/// Represent errors related to HTTP calls
	/// </summary>
	public class HttpException : Exception
    {
		#region Constants

		public const string DEFAULT_MESSAGE = "There was a error calling the server.";

		#endregion

		#region Static Constructor

		static HttpException()
		{
			// Initialize default settings
			JsonExtensions.IgnoreErrors();

			DefaultMessage = DEFAULT_MESSAGE;
		}

		#endregion

		#region Constructors

		public HttpException()
        {
		}

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public HttpException(string message, HttpStatusCode statusCode, object errorData) : base(message)
        {
            StatusCode = statusCode;
            ErrorData = errorData;
        }

		public HttpException(Exception innerException) : base(DEFAULT_MESSAGE, innerException)
		{
		}

		#endregion

		#region Static Properties

		/// <summary>
		/// Default error message
		/// </summary>
		public static string DefaultMessage { get; set; }

		#endregion

		#region Properties

		/// <summary>
		/// HTTP status code
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }

		/// <summary>
		/// Aditional error data
		/// </summary>
        public object ErrorData { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		///Try to parse the error body response to the <see cref="ErrorData"/> property
		/// </summary>
		/// <typeparam name="ResponseType">The expected response type</typeparam>
		/// <returns>The object </returns>
		public ResponseType ParseData<ResponseType>()
			where ResponseType : class
		{
			var content = ErrorData?.ToString();

            if (string.IsNullOrWhiteSpace(content))
                return null;

			var result = JsonConvert.DeserializeObject<ResponseType>(content);

			if (result != null)
				ErrorData = result;

			return result;
		}

		#endregion
	}
}
