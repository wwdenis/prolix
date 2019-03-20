// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Core.Domain
{
    /// <summary>
    /// Identifiable model with generic key
    /// </summary>
    /// <typeparam name="KeyType">Id type</typeparam>
    public interface IIdentifiable<KeyType>
        where KeyType : IComparable<KeyType>, IEquatable<KeyType>
    {
        /// <summary>
        /// Model Id
        /// </summary>
        KeyType Id { get; set; }
    }

    /// <summary>
    /// Identifiable model with numeric key
    /// </summary>
    public interface IIdentifiable : IIdentifiable<int>
    {
    }
}
