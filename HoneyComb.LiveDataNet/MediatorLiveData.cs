using HoneyComb.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// LiveData subclass which may observe other LiveData objects
    /// and react on <see cref="LiveData{T}.PropertyChanged"/> events from them.
    /// </summary>
    /// <remarks>
    /// Inspired by the LiveData class in Android Architecture Components
    ///     - https://developer.android.com/reference/androidx/lifecycle/MediatorLiveData.html
    /// </remarks>
    /// <typeparam name="T">Wrapped type</typeparam>
    public sealed class MediatorLiveData<T> : MutableLiveData<T>
    {
        private readonly List<Subscription> subscriptions = new List<Subscription>();

        /// <summary>
        /// Starts to listen the given source LiveData of same type,
        /// and the returned value will be set into this Mediator instance.
        /// </summary>
        /// <param name="source">the LiveData to listen to.</param>
        /// <returns>Unsubscription IDisposable</returns>
        public IDisposable AddSource(LiveData<T> source)
        {
            return AddSource(source, OnSourceChanged);

            void OnSourceChanged(T sourceValue) => Value = sourceValue;
        }

        /// <summary>
        /// Starts to listen the given source LiveData,
        /// converter action will be called when source value was changed,
        /// and the returned value will be set into this Mediator instance.
        /// </summary>
        /// <typeparam name="TSource">LiveData source type.</typeparam>
        /// <param name="source">the LiveData to listen to.</param>
        /// <param name="converter">the conversion from <typeparamref name="TSource"/> to <typeparamref name="T"/></param>
        /// <returns>Unsubscription IDisposable</returns>
        public IDisposable AddSource<TSource>(LiveData<TSource> source, Func<TSource, T> converter)
        {
            return AddSource(source, OnSourceChanged);

            void OnSourceChanged(TSource sourceValue) => Value = converter(sourceValue);
        }

        /// <summary>
        /// Starts to listen the given source LiveData,
        /// onSourceChanged action will be called when source value was changed.
        /// </summary>
        /// <param name="source">the LiveData to listen to.</param>
        /// <param name="onSourceChanged">The observer that will receive the events.</param>
        /// <returns>Unsubscription IDisposable</returns>
        /// <exception cref="InvalidOperationException">If the given LiveData is already added as a source but with a different Observer</exception>
        public IDisposable AddSource<TSource>(LiveData<TSource> source, Action<TSource> onSourceChanged)
        {
            if (GetSubscription(source) is Subscription existingSubscription)
                return ReturnExistingSubscriptionIfValid(onSourceChanged, existingSubscription);

            return CreateSubscription(source, onSourceChanged);
        }

        /// <summary>
        /// Starts to listen the given sources of LiveData of same type,
        /// and the returned value of each source will be set into this Mediator instance.
        /// </summary>
        /// <param name="sources">the sources of type LiveData to listen to.</param>
        /// <returns>Unsubscriptions IDisposables. Each index position matches the source index position.</returns>
        public IDisposable[] AddSources(params LiveData<T>[] sources) => sources.Select(AddSource).ToArray();

        /// <summary>
        /// Stops to listen the given LiveData.
        ///
        /// If the passed LiveData is not a source, this method does nothing.
        /// </summary>
        /// <param name="source">LiveData to stop to listen</param>
        public void RemoveSource<TSource>(LiveData<TSource> source) => GetSubscription(source)?.Dispose();

        /// <summary>
        /// Convenience factory to create a <see cref="LiveEvent{TEventArgs}"/> mediator.
        /// which may observe other LiveEvent objects
        /// and react on <see cref="LiveEvent{T}.EventChanged"/> events from them.
        /// </summary>
        /// <returns>Mediator instance.</returns>
        public static MediatorLiveData<Event<T>> CreateMediatorLiveEvent() => new MediatorLiveData<Event<T>>();

        private static IDisposable ReturnExistingSubscriptionIfValid<TSource>(Action<TSource> onSourceChanged, MediatorLiveData<T>.Subscription existingSubscription)
        {
            if (existingSubscription.IsSameOnSourceChanged(onSourceChanged) == false)
                throw new InvalidOperationException("The given LiveData is already added as a source but with a different Observer");
            return existingSubscription;
        }

        private IDisposable CreateSubscription<TSource>(LiveData<TSource> source, Action<TSource> onSourceChanged)
        {
            var sourceSubscription = source.BindMethod(onSourceChanged);
            var subscription = new Subscription(source, onSourceChanged, sourceSubscription);
            subscriptions.Add(subscription);
            return subscription;
        }

        private Subscription GetSubscription<TSource>(LiveData<TSource> source) => subscriptions.FirstOrDefault(sub => sub.IsSameSource(source));

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

            internal bool IsSameSource<TSource>(LiveData<TSource> source) => this.source == source;
        }
    }
}
