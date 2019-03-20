// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Collections
{
    public delegate void NotifyItemChangedEventHandler<ItemType>(object sender, NotifyItemChangedEventArgs<ItemType> e)
        where ItemType : INotifyPropertyChanged;

    public sealed class NotifyItemChangedEventArgs<ItemType> : EventArgs
        where ItemType : INotifyPropertyChanged
    {
        public NotifyItemChangedEventArgs(ItemType item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }
        
        public ItemType Item { get; set; }
        public string PropertyName { get; set; }
    }
}
