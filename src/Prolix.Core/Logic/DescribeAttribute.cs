// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Core.Logic
{
    /// <summary>
    /// Used to associate a model to a ModelDescriptor
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DescribeAttribute : Attribute
    {
        public DescribeAttribute(Type descriptorType)
        {
            DescriptorType = descriptorType;
        }

        /// <summary>
        /// The descriptor type
        /// </summary>
        public Type DescriptorType { get; set; }
    }
}
