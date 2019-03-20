// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Represents a model metadata, responsible for  business validation, audit and model description
    /// </summary>
    public interface IModelDescriptor
	{
        /// <summary>
        /// The model friendly name
        /// </summary>
		string Name { get; set; }
	}
}
