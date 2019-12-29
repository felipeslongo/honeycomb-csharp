﻿using System;
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
        /// Starts to listen the given source LiveData,
        /// onSourceChanged action will be called when source value was changed.
        /// </summary>
        /// <param name="source">the LiveData to listen to.</param>
        /// <param name="onSourceChanged">The observer that will receive the events.</param>
        /// <returns>Unsubscription IDisposable</returns>
        /// <exception cref="InvalidOperationException">If the given LiveData is already added as a source but with a different Observer</exception>
        public IDisposable AddSource(LiveData<T> source, Action<T> onSourceChanged)
        {
            if (GetSubscription(source) is Subscription existingSubscription)
                return ReturnExistingSubscriptionIfValid(onSourceChanged, existingSubscription);

            return CreateSubscription(source, onSourceChanged);
        }

        /// <summary>
        /// Stops to listen the given LiveData.
        ///
        /// If the passed LiveData is not a source, this method does nothing.
        /// </summary>
        /// <param name="source">LiveData to stop to listen</param>
        public void RemoveSource(LiveData<T> source) => GetSubscription(source)?.Dispose();

        private static IDisposable ReturnExistingSubscriptionIfValid(Action<T> onSourceChanged, MediatorLiveData<T>.Subscription existingSubscription)
        {
            if (existingSubscription.IsSameOnSourceChanged(onSourceChanged) == false)
                throw new InvalidOperationException("The given LiveData is already added as a source but with a different Observer");
            return existingSubscription;
        }

        private IDisposable CreateSubscription(LiveData<T> source, Action<T> onSourceChanged)
        {
            var sourceSubscription = source.BindMethod(onSourceChanged);
            var subscription = new Subscription(source, onSourceChanged, sourceSubscription);
            subscriptions.Add(subscription);
            return subscription;
        }

        private Subscription GetSubscription(LiveData<T> source) => subscriptions.FirstOrDefault(sub => sub.IsSameSource(source));

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

            internal bool IsSameOnSourceChanged(object onSourceChanged) => this.onSourceChanged == onSourceChanged;

            internal bool IsSameSource(object source) => this.source == source;
        }
    }
}
