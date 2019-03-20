// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Collections
{
    /// <summary>
    /// Represents a collection of keys and values.
    /// "Weak" because a attempt to add a same key is allowed without throwing an exception.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class WeakDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IWeakDictionary<TKey, TValue>
    {
        public WeakDictionary() : this(new Dictionary<TKey, TValue>())
        {
			IsWeak = true;
        }

        public WeakDictionary(IDictionary<TKey, TValue> source) : base(source)
        {
			IsWeak = true;
		}

        /// <summary>
        /// Gets os sets the ability to receive duplicate key add attemps.
        /// </summary>
        public bool IsWeak { get; set; }

        public virtual new TValue this[TKey key]
        {
            get
            {
                if (!IsWeak)
                    return this[key];

                TValue value;

                if (TryGetValue(key, out value))
                    return value;

                return default(TValue);
            }
            set
            {
				if (!IsWeak || ContainsKey(key))
					Remove(key);

                Add(key, value);
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary if it does not exist and edit the key value if it does.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public virtual new void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                base.Add(key, value);
            else
                this[key] = value;
        }
    }
}
