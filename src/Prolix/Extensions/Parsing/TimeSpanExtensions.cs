// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Extensions.Parsing
{
    public static class TimeSpanExtensions
    {
        public static int TotalYears(this TimeSpan value)
        {
            return new DateTime(value.Ticks).Year;
        }
    }
}
