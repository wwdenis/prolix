// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;

namespace Prolix.Domain
{
    /// <summary>
    /// Identifiable model with generic key
    /// </summary>
    /// <typeparam name="T">Id type</typeparam>
    public interface IIdentifiable<T>
        where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Model Id
        /// </summary>
        T Id { get; set; }
    }

    /// <summary>
    /// Identifiable model with numeric key
    /// </summary>
    public interface IIdentifiable : IIdentifiable<int>
    {
    }
}
