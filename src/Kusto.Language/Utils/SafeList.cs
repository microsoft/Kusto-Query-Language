using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// An immutable list over a normal mutable list that only supports adding new items.
    /// Only one <see cref="SafeList{T}"/> instance owns the underlying list and can add to it. 
    /// When a new <see cref="SafeList{T}"/> is constructed by adding items to the owner, it becomes the new owner.
    /// When a new <see cref="SafeList{T}"/> is constructed by adding items to a non-owner, it copies the items it is allowed to see into a new list, making it the owner of the new list.
    /// </summary>
    public class SafeList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> _list;
        private readonly int _length;
        private int _isOwner;

        private SafeList(List<T> list, bool isOwner = true)
        {
            _list = list;
            _length = list.Count;
            _isOwner = isOwner ? 1 : 0;
        }

        public SafeList(IEnumerable<T> items)
            : this(items != null ? new List<T>(items) : new List<T>(0))
        {
        }

        public int Count => _length;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _length)
                    throw new IndexOutOfRangeException();

                return _list[index];
            }
        }

        internal bool IsOwner => _isOwner != 0;

        /// <summary>
        /// Creates a new list with the same elements as this list plus the item specified.
        /// </summary>
        public SafeList<T> AddItem(T item)
        {
            // take ownership of list if possible
            var wasOwner = Interlocked.CompareExchange(ref _isOwner, 0, 1);
            if (wasOwner == 1)
            {
                _list.Add(item);
                return new SafeList<T>(_list);
            }
            else
            {
                var newList = Copy(_list, _length, _length + 1);
                newList.Add(item);
                return new SafeList<T>(newList);
            }
        }

        /// <summary>
        /// Creates a new list with the same elements as this list plus the items specified.
        /// </summary>
        public SafeList<T> AddItems(IEnumerable<T> items)
        {
            // take ownership of list if possible
            var wasOwner = Interlocked.CompareExchange(ref _isOwner, 0, 1);
            if (wasOwner == 1)
            {
                _list.AddRange(items);
                return new SafeList<T>(_list);
            }
            else
            {
                var newList = Copy(_list, _length, _length);
                newList.AddRange(items);
                return new SafeList<T>(newList);
            }
        }

        private static List<T> Copy(List<T> source, int length, int newLength)
        {
            var newList = new List<T>(newLength);

            for (int i = 0; i < length; i++)
            {
                newList.Add(source[i]);
            }

            return newList;
        }

        public Enumerator GetEnumerator() => new Enumerator(_list, 0, _length);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly IReadOnlyList<T> _list;
            private readonly int _start;
            private readonly int _end;
            private int _index;

            public Enumerator(IReadOnlyList<T> list, int start, int length)
            {
                _list = list;
                _start = start;
                _end = start + length;
                _index = start - 1;
            }

            public T Current => _index >= _start && _index < _end ? _list[_index] : default(T);
            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                _index++;
                return _index < _end;
            }

            public void Dispose() { }
            public void Reset() { }
        }

        /// <summary>
        /// An empty list
        /// </summary>
        public static readonly SafeList<T> Empty = 
            new SafeList<T>(new List<T>(0), isOwner: false); // don't let anyone add to this global list
    }
}