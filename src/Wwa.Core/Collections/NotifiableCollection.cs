// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Wwa.Core.Extensions.Collections;

namespace Wwa.Core.Collections
{
    /// <summary>
    /// Represents a dynamic data collection that provides notifications 
    /// when items get added, removed, refreshed, or any item is changed
    /// </summary>
    /// <typeparam name="ItemType">The type of elements in the collection</typeparam>
    public class NotifiableCollection<ItemType> : ObservableCollection<ItemType>
		where ItemType : class, INotifyPropertyChanged
	{
		#region Events

		/// <summary>
		/// Is fired when a items changed
		/// </summary>
		public event NotifyItemChangedEventHandler<ItemType> ItemChanged;

		#endregion

		#region Constructors

		public NotifiableCollection()
		{
			NotifyItem = true;
		}

		public NotifiableCollection(bool notifyItem)
		{
			NotifyItem = notifyItem;
		}

		public NotifiableCollection(IEnumerable<ItemType> source) : base(source)
		{
			NotifyItem = true;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Calls CollectionChanged event if the a item is changed
		/// </summary>
		public bool NotifyItem { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Add a collection
		/// </summary>
		/// <param name="source">The collection source</param>
		public void AddRange(IEnumerable<ItemType> source)
		{
			if (!source?.Any() ?? false)
				return;

			foreach (var item in source)
				Add(item);
		}

		#endregion

		#region Events Handlers

		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			var newItems = e.NewItems.ToList<INotifyPropertyChanged>();
			var oldItems = e.OldItems.ToList<INotifyPropertyChanged>();

			foreach (var item in newItems)
			{
				item.PropertyChanged += Item_PropertyChanged;
			}

			foreach (var item in oldItems)
			{
				item.PropertyChanged -= Item_PropertyChanged;
			}

			base.OnCollectionChanged(e);
		}

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }

            base.ClearItems();
        }

        #endregion

        #region Events Handlers

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var item = sender as ItemType;
			var args = new NotifyItemChangedEventArgs<ItemType>(item, e.PropertyName);

            ItemChanged?.Invoke(this, args);

			if (NotifyItem)
			{
				var index = IndexOf(item);
				this[index] = item;
			}
		}

		#endregion
	}
}
