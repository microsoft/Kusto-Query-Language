using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Utils
{
    /// <summary>
    /// A List wrapper that restricts usage to read only semantics
    /// and constructing copies with additional items using deferred copy semantics.
    /// </summary>
    public class SafeList<T> : IReadOnlyList<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> _list;
        private readonly int _length;
        private int _isOwner;

        private SafeList(List<T> list, int isOwner = 1)
        {
            _list = list;
            _length = list.Count;
            _isOwner = isOwner;
        }

        public SafeList(IEnumerable<T> items)
            : this(new List<T>(items))
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

        /// <summary>
        /// Creates a new list with the same elements as this list plus the item specified.
        /// </summary>
        public SafeList<T> WithItem(T item)
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
                var newList = Copy(_list, _length);
                newList.Add(item);
                return new SafeList<T>(newList);
            }
        }

        /// <summary>
        /// Creates a new list with the same elements as this list plus the items specified.
        /// </summary>
        public SafeList<T> WithItems(IEnumerable<T> items)
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
                var newList = Copy(_list, _length);
                newList.AddRange(items);
                return new SafeList<T>(newList);
            }
        }

        private static List<T> Copy(List<T> source, int length)
        {
            var newList = new List<T>(length);

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

        public static readonly SafeList<T> Empty = new SafeList<T>(new List<T>(0), isOwner: 0);
    }
}