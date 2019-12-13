using HoneyComb.Core.Lifecycles;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Lifecycle-aware EventHandler.
    /// Similar to <see cref="LiveData{T}"/>
    /// but dispatch events only once.
    /// </summary>
    internal class LiveEvent<TEventArgs>
    {
        private readonly MutableLiveData<TEventArgs> _liveData;
        private readonly List<EventHandler<TEventArgs>> _subscribers = new List<EventHandler<TEventArgs>>();

        public LiveEvent()
        {
            _liveData = new MutableLiveData<TEventArgs>();
        }

        public LiveEvent(TEventArgs value)
        {
            _liveData = new MutableLiveData<TEventArgs>(value);
        }

        public IDisposable Subscribe(ILifecycleOwner lifecycleOwner, EventHandler<TEventArgs> subscriber)
        {            
            _subscribers.Add(subscriber);
            return Disposable.Create(() => _subscribers.Remove(subscriber));
        }
    }
}
