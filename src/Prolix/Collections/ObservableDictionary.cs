// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Prolix.Collections
{
    public class ObservableDictionary<TK, TV> : IWeakDictionary<TK, TV>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Properties

        protected IDictionary<TK, TV> Dictionary { get; private set; }

        public bool IsWeak { get; set; }

        #endregion

        #region Constructors

        public ObservableDictionary()
        {
            Dictionary = new Dictionary<TK, TV>();
        }

        public ObservableDictionary(IDictionary<TK, TV> dictionary)
        {
            Dictionary = new Dictionary<TK, TV>(dictionary);
        }

        public ObservableDictionary(IEqualityComparer<TK> comparer)
        {
            Dictionary = new Dictionary<TK, TV>(comparer);
        }

        public ObservableDictionary(int capacity)
        {
            Dictionary = new Dictionary<TK, TV>(capacity);
        }

        public ObservableDictionary(IDictionary<TK, TV> dictionary, IEqualityComparer<TK> comparer)
        {
            Dictionary = new Dictionary<TK, TV>(dictionary, comparer);
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TK> comparer)
        {
            Dictionary = new Dictionary<TK, TV>(capacity, comparer);
        }
        #endregion

        #region IDictionary<TKey,TValue> Members

        public ICollection<TK> Keys => Dictionary.Keys;

        public ICollection<TV> Values => Dictionary.Values;

        public void Add(TK key, TV value) => Insert(key, value, true);
        
        public bool ContainsKey(TK key) => Dictionary.ContainsKey(key);

        public bool TryGetValue(TK key, out TV value) => Dictionary.TryGetValue(key, out value);

        public bool Remove(TK key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Dictionary.TryGetValue(key, out TV value);

            var removed = Dictionary.Remove(key);

            if (removed)
                //OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
                OnCollectionReset();

            return removed;
        }
        
        public TV this[TK key]
        {
            get
            {
                if (!IsWeak)
                    return Dictionary[key];

                if (TryGetValue(key, out TV value))
                    return value;

                return default;
            }
            set
            {
                Insert(key, value, false);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public int Count => Dictionary.Count;

        public bool IsReadOnly => Dictionary.IsReadOnly;

        public bool Contains(KeyValuePair<TK, TV> item) => Dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex) => Dictionary.CopyTo(array, arrayIndex);

        public void Add(KeyValuePair<TK, TV> item) => Insert(item.Key, item.Value, true);

        public bool Remove(KeyValuePair<TK, TV> item) => Remove(item.Key);

        public void Clear()
        {
            if (Dictionary.Any())
            {
                Dictionary.Clear();
                OnCollectionReset();
            }
        }

        #endregion
        
        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => Dictionary.GetEnumerator();

        #endregion
        
        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dictionary).GetEnumerator();
        
        #endregion
        
        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods

        public void AddRange(IDictionary<TK, TV> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (!items.Any())
                return;

            if (!Dictionary.Any())
            {
                Dictionary = new Dictionary<TK, TV>(items);
            }
            else if (items.Keys.Any(i => Dictionary.ContainsKey(i)))
            {
                throw new ArgumentOutOfRangeException(nameof(items), "An item with the same key has already been added.");
            }
            else
            {
                foreach (var item in items)
                {
                    Dictionary.Add(item);
                }
            }

            OnCollectionAdded(items.ToArray());
        }

        #endregion

        #region Protected Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        void Insert(TK key, TV value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (Dictionary.TryGetValue(key, out TV item))
            {
                if (add)
                    throw new ArgumentOutOfRangeException(nameof(key), "An item with the same key has already been added.");

                if (Equals(item, value))
                    return;

                Dictionary[key] = value;

                OnCollectionReplaced(new KeyValuePair<TK, TV>(key, value), new KeyValuePair<TK, TV>(key, item));
            }
            else
            {
                Dictionary[key] = value;

                OnCollectionAdded(new KeyValuePair<TK, TV>(key, value));
            }
        }

        void OnPropertyChanged()
        {
            var props = new[]
            {
                nameof(Count),
                nameof(Keys),
                nameof(Values),
                "Item[]"
            };

            foreach (var prop in props)
            {
                OnPropertyChanged(prop);
            }
        }

        void OnCollectionReset()
        {
            var action = NotifyCollectionChangedAction.Reset;
            var args = new NotifyCollectionChangedEventArgs(action);

            OnPropertyChanged();
            CollectionChanged?.Invoke(this, args);
        }

        void OnCollectionAdded(KeyValuePair<TK, TV> item)
        {
            var action = NotifyCollectionChangedAction.Add;
            var args = new NotifyCollectionChangedEventArgs(action, item);

            OnPropertyChanged();
            CollectionChanged?.Invoke(this, args);
        }

        void OnCollectionAdded(IList items)
        {
            var action = NotifyCollectionChangedAction.Add;
            var args = new NotifyCollectionChangedEventArgs(action, items);

            OnPropertyChanged();
            CollectionChanged?.Invoke(this, args);
        }

        void OnCollectionReplaced(KeyValuePair<TK, TV> newItem, KeyValuePair<TK, TV> oldItem)
        {
            var action = NotifyCollectionChangedAction.Replace;
            var args = new NotifyCollectionChangedEventArgs(action, newItem, oldItem);

            OnPropertyChanged();
            CollectionChanged?.Invoke(this, args);
        }

        #endregion
    }
}
