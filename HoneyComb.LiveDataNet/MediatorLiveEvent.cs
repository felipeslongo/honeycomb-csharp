using HoneyComb.Core;
using HoneyComb.LiveDataNet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoneyComb.LiveEventNet
{
    /// <summary>
    /// LiveEvent subclass which may observe other LiveEvent objects
    /// and react on <see cref="LiveEvent{T}.EventChanged"/> events from them.
    /// </summary>
    /// <remarks>
    /// Inspired by the LiveData class in Android Architecture Components
    ///     - https://developer.android.com/reference/androidx/lifecycle/MediatorLiveEvent.html
    /// </remarks>
    /// <typeparam name="TEventArgs">Event Args</typeparam>
    public sealed class MediatorLiveEvent<TEventArgs> : MutableLiveEvent<TEventArgs>
    {
        private readonly List<Subscription> subscriptions = new List<Subscription>();

        /// <summary>
        /// Starts to listen the given source LiveEvent of same type,
        /// and the returned value will be set into this Mediator instance.
        /// </summary>
        /// <param name="source">the LiveEvent to listen to.</param>
        /// <returns>Unsubscription IDisposable</returns>
        public IDisposable AddSource(LiveEvent<TEventArgs> source)
        {
            return AddSource(source, OnSourceChanged);

            void OnSourceChanged(Event<TEventArgs> sourceValue) => Value = sourceValue;
        }

        /// <summary>
        /// Starts to listen the given source LiveEvent,
        /// converter action will be called when source value was changed,
        /// and the returned value will be set into this Mediator instance.
        /// </summary>
        /// <typeparam name="TSource">LiveEvent source type.</typeparam>
        /// <param name="source">the LiveEvent to listen to.</param>
        /// <param name="converter">the conversion from <typeparamref name="TSource"/> to <typeparamref name="T"/></param>
        /// <returns>Unsubscription IDisposable</returns>
        public IDisposable AddSource<TSource>(LiveEvent<TSource> source, Func<Event<TSource>, Event<TEventArgs>> converter)
        {
            return AddSource(source, OnSourceChanged);

            void OnSourceChanged(Event<TSource> sourceValue) => Value = converter(sourceValue);
        }

        /// <summary>
        /// Starts to listen the given source LiveEvent,
        /// onSourceChanged action will be called when source value was changed.
        /// </summary>
        /// <param name="source">the LiveEvent to listen to.</param>
        /// <param name="onSourceChanged">The observer that will receive the events.</param>
        /// <returns>Unsubscription IDisposable</returns>
        /// <exception cref="InvalidOperationException">If the given LiveEvent is already added as a source but with a different Observer</exception>
        public IDisposable AddSource<TSource>(LiveEvent<TSource> source, Action<Event<TSource>> onSourceChanged)
        {
            if (GetSubscription(source) is Subscription existingSubscription)
                return ReturnExistingSubscriptionIfValid(onSourceChanged, existingSubscription);

            return CreateSubscription(source, onSourceChanged);
        }

        /// <summary>
        /// Starts to listen the given sources of LiveEvent of same type,
        /// and the returned value of each source will be set into this Mediator instance.
        /// </summary>
        /// <param name="sources">the sources of type LiveEvent to listen to.</param>
        /// <returns>Unsubscriptions IDisposables. Each index position matches the source index position.</returns>
        public IDisposable[] AddSources(params LiveEvent<TEventArgs>[] sources) => sources.Select(AddSource).ToArray();

        /// <summary>
        /// Stops to listen the given LiveEvent.
        ///
        /// If the passed LiveEvent is not a source, this method does nothing.
        /// </summary>
        /// <param name="source">LiveEvent to stop to listen</param>
        public void RemoveSource<TSource>(LiveEvent<TSource> source) => GetSubscription(source)?.Dispose();

        private static IDisposable ReturnExistingSubscriptionIfValid<TSource>(Action<Event<TSource>> onSourceChanged, MediatorLiveEvent<TEventArgs>.Subscription existingSubscription)
        {
            if (existingSubscription.IsSameOnSourceChanged(onSourceChanged) == false)
                throw new InvalidOperationException("The given LiveEvent is already added as a source but with a different Observer");
            return existingSubscription;
        }

        private IDisposable CreateSubscription<TSource>(LiveEvent<TSource> source, Action<Event<TSource>> onSourceChanged)
        {
            var sourceSubscription = source.Subscribe(onSourceChanged);
            var subscription = new Subscription(source, onSourceChanged, sourceSubscription);
            subscriptions.Add(subscription);
            return subscription;
        }

        private Subscription GetSubscription<TSource>(LiveEvent<TSource> source) => subscriptions.FirstOrDefault(sub => sub.IsSameSource(source));

        /// <summary>
        /// Simple data class to wrap everithing needed to make comparisons easier.
        /// </summary>
        private class Subscription : IDisposable
        {
            private readonly object onSourceChanged;
            private readonly object source;
            private readonly IDisposable subscription;

            public Subscription(object source, object onSourceChanged, IDisposable subscription)
            {
                this.source = source;
                this.onSourceChanged = onSourceChanged;
                this.subscription = subscription;
            }

            public void Dispose() => subscription.Dispose();

            internal bool IsSameOnSourceChanged<TSource>(Action<TSource> onSourceChanged) => this.onSourceChanged == onSourceChanged as object;

            internal bool IsSameSource<TSource>(LiveEvent<TSource> source) => this.source == source;
        }
    }
}
