// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Ioc
{
	public abstract class Factory<T> : IFactory
		where T : class
	{
		public Factory() 
		{
			Type = typeof(T);
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

		public abstract T Create();
	}
}
