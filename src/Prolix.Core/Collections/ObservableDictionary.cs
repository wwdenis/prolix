// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Prolix.Core.Collections
{
    public class ObservableDictionary<TKey, TValue> : IWeakDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Fields

        const string CountString = "Count";
        const string IndexerName = "Item[]";
        const string KeysName = "Keys";
        const string ValuesName = "Values";
        IDictionary<TKey, TValue> _dictionary;

        #endregion

        #region Properties

        protected IDictionary<TKey, TValue> Dictionary
        {
            get { return _dictionary; }
        }

        public bool IsWeak { get; set; }

        #endregion

        #region Constructors
        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public ObservableDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }
        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return Dictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue value;
            Dictionary.TryGetValue(key, out value);
            var removed = Dictionary.Remove(key);
            if (removed)
                //OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
                OnCollectionChanged();

            return removed;
        }
        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }
        
        public ICollection<TValue> Values
        {
            get { return Dictionary.Values; }
        }
        
        public TValue this[TKey key]
        {
            get
            {
                if (!IsWeak)
                    return Dictionary[key];

                TValue value;

                if (TryGetValue(key, out value))
                    return value;

                return default(TValue);
            }
            set
            {
                Insert(key, value, false);
            }
        }

        #endregion
        
        #region ICollection<KeyValuePair<TKey,TValue>> Members


        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value, true);
        }


        public void Clear()
        {
            if (Dictionary.Count > 0)
            {
                Dictionary.Clear();
                OnCollectionChanged();
            }
        }


        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Dictionary.Contains(item);
        }


        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Dictionary.CopyTo(array, arrayIndex);
        }


        public int Count
        {
            get { return Dictionary.Count; }
        }


        public bool IsReadOnly
        {
            get { return Dictionary.IsReadOnly; }
        }


        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }


        #endregion
        
        #region IEnumerable<KeyValuePair<TKey,TValue>> Members


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }


        #endregion
        
        #region IEnumerable Members


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dictionary).GetEnumerator();
        }


        #endregion
        
        #region INotifyCollectionChanged Members


        public event NotifyCollectionChangedEventHandler CollectionChanged;


        #endregion
        
        #region INotifyPropertyChanged Members


        public event PropertyChangedEventHandler PropertyChanged;


        #endregion

        #region Public Methods

        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null) throw new ArgumentNullException("items");


            if (items.Count > 0)
            {
                if (Dictionary.Count > 0)
                {
                    if (items.Keys.Any((k) => Dictionary.ContainsKey(k)))
                        throw new ArgumentException("An item with the same key has already been added.");
                    else
                        foreach (var item in items) Dictionary.Add(item);
                }
                else
                    _dictionary = new Dictionary<TKey, TValue>(items);


                OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) throw new ArgumentNullException("key");


            TValue item;
            if (Dictionary.TryGetValue(key, out item))
            {
                if (add) throw new ArgumentException("An item with the same key has already been added.");
                if (Equals(item, value)) return;
                Dictionary[key] = value;


                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                Dictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        void OnPropertyChanged()
        {
            OnPropertyChanged(CountString);
            OnPropertyChanged(IndexerName);
            OnPropertyChanged(KeysName);
            OnPropertyChanged(ValuesName);
        }



        void OnCollectionChanged()
        {
            OnPropertyChanged();
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            OnPropertyChanged();
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            OnPropertyChanged();
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }


        void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            OnPropertyChanged();
            if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems));
        }

        #endregion
    }
}
