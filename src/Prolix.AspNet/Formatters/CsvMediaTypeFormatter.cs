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
using Prolix.Core.Extensions.IO;

namespace Prolix.AspNet.Formatters
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
            IEnumerable data = value.ToEnumerable();
            string output = data?.ToCsv();

            // Write to the stream if not empty
            await writeStream.TryWriteText(output);
        }

        public override bool CanReadType(Type type) => false;

        public override bool CanWriteType(Type type) => true;
    }
}
