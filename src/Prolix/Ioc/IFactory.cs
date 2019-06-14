// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Ioc
{
	public interface IFactory
	{
		Type Type { get; }

		object CreateInstance();
	}
}
