// Copyright 2017 (c) [Denis Da Silva]. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolix.Core.Collections
{
    public interface ITwoWayEnumerator<T> : IEnumerator<T>
    {
        bool MovePrevious();
        bool MovePrevious(int spaces);
        void MoveNext(int spaces);
        int Count();
        int CurrentIndex();
    }

    public class TwoWayEnumerator<T> : ITwoWayEnumerator<T>
    {
        IEnumerator<T> _enumerator;
        List<T> _buffer;
        int _index;

        public int CurrentIndex { get; set; }
        public int Count { get; set; }

        public TwoWayEnumerator(IEnumerator<T> enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));

            SetCount(enumerator);

            _enumerator = enumerator;
            _buffer = new List<T>();
            CurrentIndex = _index = -1;
        }

        void SetCount(IEnumerator<T> enumerator)
        {
            Count = 0;
            while (enumerator.MoveNext())
                Count++;
            enumerator.Reset();
        }

        public bool MovePrevious()
        {
            if (_index <= 0)
            {
                return false;
            }

            CurrentIndex = --_index;
            return true;
        }

        public bool MoveNext()
        {
            if (_index < _buffer.Count - 1)
            {
                CurrentIndex = ++_index;
                return true;
            }

            if (_enumerator.MoveNext())
            {
                _buffer.Add(_enumerator.Current);
                CurrentIndex = ++_index;
                return true;
            }

            return false;
        }

        public T Current
        {
            get
            {
                if (_index < 0 || _index >= _buffer.Count)
                    throw new InvalidOperationException();

                return _buffer[_index];
            }
        }

        public void Reset()
        {
            _enumerator.Reset();
            _buffer.Clear();
            CurrentIndex=_index = -1;
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MovePrevious(int spaces)
        {
            for (int i = 0; i < spaces; i++)
            {
                MovePrevious();
            }
            return true;
        }

        public void MoveNext(int spaces)
        {
            for (int i = 0; i < spaces; i++)
            {
                MoveNext();
            }
        }

        int ITwoWayEnumerator<T>.Count()
        {
            return Count;
        }

        int ITwoWayEnumerator<T>.CurrentIndex()
        {
            return CurrentIndex;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }
    }
}
