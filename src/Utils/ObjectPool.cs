using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Kusto.Language.Utils
{
    internal class ObjectPool<T>
        where T : class
    {
        private readonly Func<T> creator;
        private readonly Action<T> resetter;
        private readonly T[] items;

        public ObjectPool(Func<T> creator, Action<T> resetter, int size = 10)
        {
            this.creator = creator;
            this.resetter = resetter;
            this.items = new T[size];
        }

        public T AllocateFromPool()
        {
            // look for item returned to pool
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i] != null)
                {
                    var item = Interlocked.Exchange(ref this.items[i], null);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }

            // make a new one
            return this.creator();
        }

        public void ReturnToPool(T item)
        {
            // clear item
            this.resetter(item);

            // look for open space to place in pool
            // if no space is found, let GC have it.
            for (int i = 0; i < this.items.Length; i++)
            {
                if (this.items[i] == null)
                {
                    var result = Interlocked.CompareExchange(ref this.items[i], item, null);
                    if (result == null)
                    {
                        break;
                    }
                }
            }
        }
    }
}