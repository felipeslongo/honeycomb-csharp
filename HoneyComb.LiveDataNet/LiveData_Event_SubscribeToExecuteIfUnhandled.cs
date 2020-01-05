using HoneyComb.Core;
using HoneyComb.Core.Lifecycles;
using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Extension SubscribeToExecuteIfUnhandled for a <see cref="LiveData{HoneyComb.Core.Event}"/>.
    /// </summary>
    public static class LiveData_Event_SubscribeToExecuteIfUnhandled
    {
        /// <summary>
        /// Subscribe to be notified only if the event is not handled.
        /// It's useless to subscribe to multiple subscribers because the first
        /// one will handle the event, and the subsequent ones will not be notified.
        /// </summary>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable SubscribeToExecuteIfUnhandled<TEventArgs>(this LiveData<Event<TEventArgs>> @this, Action<TEventArgs> subscriber)
        {
            return @this.Subscribe(@event => UnwrapEventAndNotifySubscriber(@event));

            void UnwrapEventAndNotifySubscriber(Event<TEventArgs> @event) => @event.ExecuteIfUnhandled(NotifySubscriber);
            void NotifySubscriber(TEventArgs content) => subscriber(content);
        }

        /// <summary>
        /// Subscribe to be notified only if the event is not handled.
        /// It's useless to subscribe to multiple subscribers because the first
        /// one will handle the event, and the subsequent ones will not be notified.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable SubscribeToExecuteIfUnhandled<TEventArgs>(this LiveData<Event<TEventArgs>> @this, ILifecycleOwner lifecycleOwner, Action<TEventArgs> subscriber) =>
            @this.SubscribeToExecuteIfUnhandled(lifecycleOwner, (_, args) => subscriber(args));

        /// <summary>
        /// Subscribe to be notified only if the event is not handled.
        /// It's useless to subscribe to multiple subscribers because the first
        /// one will handle the event, and the subsequent ones will not be notified.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable SubscribeToExecuteIfUnhandled<TEventArgs>(this LiveData<Event<TEventArgs>> @this, ILifecycleOwner lifecycleOwner, EventHandler<TEventArgs> subscriber)
        {
            return @this.Subscribe(lifecycleOwner, UnwrapEventAndNotifySubscriber);

            void UnwrapEventAndNotifySubscriber(object _, Event<TEventArgs> @event) => @event.ExecuteIfUnhandled(NotifySubscriber);
            void NotifySubscriber(TEventArgs content) => subscriber(@this, content);
        }
    }
}
