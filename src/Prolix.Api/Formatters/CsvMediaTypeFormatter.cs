// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Prolix.Core.Extensions.Collections;

namespace Prolix.Api.Formatters
{
    public sealed class CsvMediaTypeFormatter : MediaTypeFormatter
    {
        public CsvMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
        }

        public CsvMediaTypeFormatter(MediaTypeMapping mediaTypeMapping) : this()
        {
            MediaTypeMappings.Add(mediaTypeMapping);
        }

        async public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var entityType = value.GetType();

            if (value != null)
            {
                IEnumerable data = null;

                if (value is IEnumerable)
                {
                    data = ((IEnumerable)value);
                }
                else
                {
                    data = new[] { value };
                }

                var result = data?.ToCsv();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    using (var writer = new StreamWriter(writeStream))
                    {
                        await writer.WriteAsync(result);
                    }
                }
            }
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }
    }
}
