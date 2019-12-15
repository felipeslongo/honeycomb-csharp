﻿using HoneyComb.Core;
using HoneyComb.Core.Lifecycles;
using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Lifecycle-aware EventHandler.
    /// Similar to <see cref="LiveData{T}"/>
    /// but dispatch events only once.
    /// </summary>
    public class LiveEvent<TEventArgs>
    {
        /// <summary>
        /// LiveData used to delegate the lifecycle-aware responsability.
        /// </summary>
        private readonly MutableLiveData<Event<TEventArgs>> _liveData = new MutableLiveData<Event<TEventArgs>>();

        public LiveEvent()
        {
        }

        public LiveEvent(TEventArgs value)
        {
            _liveData.Value = new Event<TEventArgs>(value);
        }

        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public IDisposable Subscribe(ILifecycleOwner lifecycleOwner, Action<Event<TEventArgs>> subscriber) =>
            Subscribe(lifecycleOwner, (_, args) => subscriber(args));

        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public IDisposable Subscribe(ILifecycleOwner lifecycleOwner, EventHandler<Event<TEventArgs>> subscriber)
        {
            return _liveData.BindMethod(lifecycleOwner, NotifySubscriber);

            void NotifySubscriber(Event<TEventArgs> @event) => subscriber(this, @event);
        }

        /// <summary>
        /// Subscribe to be notified only if the event is not handled.
        /// It's useless to subscribe to multiple subscribers because the first
        /// one will handle the event, and the subsequent ones will not be notified.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public IDisposable SubscribeToExecuteIfUnhandled(ILifecycleOwner lifecycleOwner, Action<TEventArgs> subscriber) =>
            SubscribeToExecuteIfUnhandled(lifecycleOwner, (_, args) => subscriber(args));

        /// <summary>
        /// Subscribe to be notified only if the event is not handled.
        /// It's useless to subscribe to multiple subscribers because the first
        /// one will handle the event, and the subsequent ones will not be notified.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public IDisposable SubscribeToExecuteIfUnhandled(ILifecycleOwner lifecycleOwner, EventHandler<TEventArgs> subscriber)
        {
            return Subscribe(lifecycleOwner, UnwrapEventAndNotifySubscriber);

            void UnwrapEventAndNotifySubscriber(object _, Event<TEventArgs> @event) => @event.ExecuteIfUnhandled(NotifySubscriber);
            void NotifySubscriber(TEventArgs content) => subscriber(this, content);
        }
    }
}
