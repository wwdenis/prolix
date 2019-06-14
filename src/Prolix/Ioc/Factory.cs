// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Ioc
{
	public abstract class Factory<DependencyType> : IFactory
		where DependencyType : class
	{
		public Factory() 
		{
			Type = typeof(DependencyType);
		}

		public Type Type
		{
			get;
			protected set;
		}

		public object CreateInstance()
		{
			return Create();
		}

		public abstract DependencyType Create();
	}
}
