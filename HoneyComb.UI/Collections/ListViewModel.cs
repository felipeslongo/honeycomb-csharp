using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace HoneyComb.UI.Collections
{
    public sealed class ListViewModel<T>
    {
        private readonly List<T> backingList = new List<T>();

        public IObservable<ItemAddedEvent<T>> ItemAdded { get; } = new Subject<ItemAddedEvent<T>>();
        public IObservable<ItemRefreshedEvent<T>> ItemRefreshed { get; } = new Subject<ItemRefreshedEvent<T>>();
        public IObservable<ItemRemovedEvent<T>> ItemRemoved { get; } = new Subject<ItemRemovedEvent<T>>();
        public IObservable<ItemReplacedEvent<T>> ItemReplaced { get; } = new Subject<ItemReplacedEvent<T>>();
    }
}
