// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Prolix.AspNet.Results
{
    /// <summary>
    /// File attachment result
    /// </summary>
    public class FileResult : IHttpActionResult
    {
        public string FilePath { get; set; }

        public FileResult()
        {
        }

        public FileResult(string filePath)
        {
            FilePath = filePath;
        }

        async public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => BuildMessage());
        }

        HttpResponseMessage BuildMessage()
        {
            if (!File.Exists(FilePath))
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var fileName = Path.GetFileName(FilePath);
            var contents = File.ReadAllBytes(FilePath);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(contents)
            };

            var fileExtension = Path.GetExtension(FilePath);
            var contentType = MimeTypes.GetMimeType(fileExtension);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = WebUtility.UrlEncode(fileName)
                };
            }

            return response;
        }
    }
}
