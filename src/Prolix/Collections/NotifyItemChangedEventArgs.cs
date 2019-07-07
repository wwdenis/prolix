// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Collections
{
    public delegate void NotifyItemChangedEventHandler<T>(object sender, NotifyItemChangedEventArgs<T> e)
        where T : INotifyPropertyChanged;

    public sealed class NotifyItemChangedEventArgs<T> : EventArgs
        where T : INotifyPropertyChanged
    {
        public NotifyItemChangedEventArgs(T item, string propertyName)
        {
            Item = item;
            PropertyName = propertyName;
        }
        
        public T Item { get; set; }
        public string PropertyName { get; set; }
    }
}
