using HoneyComb.Core;
using HoneyComb.Core.Lifecycles;
using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Extension Subscribe for a <see cref="LiveData{HoneyComb.Core.Event}"/>.
    /// </summary>
    public static class LiveData_Event_Subscribe
    {
        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable Subscribe<TEventArgs>(this LiveData<Event<TEventArgs>> @this, ILifecycleOwner lifecycleOwner, EventHandler<Event<TEventArgs>> subscriber)
        {
            return @this.BindMethod(lifecycleOwner, NotifySubscriber);

            void NotifySubscriber(Event<TEventArgs> @event) => subscriber(@this, @event);
        }

        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable Subscribe<TEventArgs>(this LiveData<Event<TEventArgs>> @this, Action<Event<TEventArgs>> subscriber) => @this.BindMethod(subscriber);

        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        /// <param name="lifecycleOwner">LifecycleOwner</param>
        /// <param name="subscriber">Action delegate</param>
        /// <returns>Unsubscription delegate.</returns>
        public static IDisposable Subscribe<TEventArgs>(this LiveData<Event<TEventArgs>> @this, ILifecycleOwner lifecycleOwner, Action<Event<TEventArgs>> subscriber) =>
            @this.Subscribe(lifecycleOwner, (_, args) => subscriber(args));
    }
}
