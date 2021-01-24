using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;

namespace HoneyComb.UI.Collections
{
    public sealed class ListViewModel<T> : IList<T>
    {
        private readonly List<T> backingList = new List<T>();
        private readonly Subject<Unit> cleared = new Subject<Unit>();
        private readonly Subject<ItemAddedEvent<T>> itemAdded = new Subject<ItemAddedEvent<T>>();
        private readonly Subject<ItemRefreshedEvent<T>> itemRefreshed = new Subject<ItemRefreshedEvent<T>>();
        private readonly Subject<ItemRemovedEvent<T>> itemRemoved = new Subject<ItemRemovedEvent<T>>();
        private readonly Subject<ItemReplacedEvent<T>> itemReplaced = new Subject<ItemReplacedEvent<T>>();
        private readonly Subject<ItensAddedEvent<T>> itensAdded = new Subject<ItensAddedEvent<T>>();
        private readonly Subject<ItensRefreshedEvent<T>> itensRefreshed = new Subject<ItensRefreshedEvent<T>>();
        private readonly Subject<ItensRemovedEvent<T>> itensRemoved = new Subject<ItensRemovedEvent<T>>();

        public IObservable<Unit> Cleared => cleared;

        public IObservable<ItemAddedEvent<T>> ItemAdded => itemAdded;

        public IObservable<ItemRefreshedEvent<T>> ItemRefreshed => itemRefreshed;

        public IObservable<ItemRemovedEvent<T>> ItemRemoved => itemRemoved;

        public IObservable<ItemReplacedEvent<T>> ItemReplaced => itemReplaced;

        public IObservable<ItensAddedEvent<T>> ItensAdded => itensAdded;

        public IObservable<ItensRefreshedEvent<T>> ItensRefreshed => itensRefreshed;

        public IObservable<ItensRemovedEvent<T>> ItensRemoved => itensRemoved;

        public IEnumerator<T> GetEnumerator()
        {
            return backingList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) backingList).GetEnumerator();
        }

        public void Add(T item)
        {
            backingList.Add(item);
            var count = Count;
            itemAdded.OnNext(new ItemAddedEvent<T>(item, count));
        }

        public void Clear()
        {
            backingList.Clear();
            cleared.OnNext(Unit.Default);
        }

        public bool Contains(T item)
        {
            return backingList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            backingList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var indexOf = IndexOf(item);
            if (indexOf is -1)
                return false;

            RemoveAt(indexOf);
            return true;
        }

        public int Count => backingList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            return backingList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            backingList.Insert(index, item);
            itemAdded.OnNext(new ItemAddedEvent<T>(item, index));
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            backingList.RemoveAt(index);
            itemRemoved.OnNext(new ItemRemovedEvent<T>(item, index));
        }

        public T this[int index]
        {
            get => backingList[index];
            set
            {
                var oldItem = this[index];
                backingList[index] = value;
                itemReplaced.OnNext(new ItemReplacedEvent<T>(oldItem, value, index));
            }
        }
    }
}