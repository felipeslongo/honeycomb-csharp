using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    ///     Android Lifecycle-Aware implementation of an C# EventHandler wrapper.
    ///     Notify its subscribers only on <see cref="IsActive" /> state.
    ///     When inactive it captures and holds the last received pending event
    ///     and notify the subscribers when the Lifecycle get back to active state.
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public sealed class LifecycleEventHandler<TEventArgs> : IDisposable
    {
        private readonly List<EventHandler<TEventArgs>> _handlers = new List<EventHandler<TEventArgs>>();
        private bool _disposed;
        private LifecycleObservable? _lifecycleObservable;
        private (object sender, TEventArgs eventArgs)? _pendingEvent;

        public LifecycleEventHandler(LifecycleObservable lifecycleObservable)
        {
            _lifecycleObservable = lifecycleObservable;
            SubscribeToLifecycleEvents();
        }

        public bool IsActive =>
            _lifecycleObservable?.StateLastKnown.IsAtLeast(global::Android.Arch.Lifecycle.Lifecycle.State.Started) ?? false;

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            ClearSubscribers();
            UnsubscribeToLifecycleEvents();
            _pendingEvent = null;
            _lifecycleObservable = null;
            _disposed = true;
        }

        private void SubscribeToLifecycleEvents()
        {
            _lifecycleObservable!.OnResume += LifecycleObservableOnOnResumeOrOnStart;
            _lifecycleObservable!.OnStart += LifecycleObservableOnOnResumeOrOnStart;
        }

        private void UnsubscribeToLifecycleEvents()
        {
            _lifecycleObservable!.OnResume -= LifecycleObservableOnOnResumeOrOnStart;
            _lifecycleObservable!.OnStart -= LifecycleObservableOnOnResumeOrOnStart;
        }

        private void LifecycleObservableOnOnResumeOrOnStart(object sender, EventArgs e)
        {
            InvokeHandlersIfHasPendingEvent();
        }

        public event EventHandler<TEventArgs> Event
        {
            add => _handlers.Add(value);
            remove => _handlers.Remove(value);
        }

        public void Invoke(object sender, TEventArgs value)
        {
            if (IsActive == false)
            {
                _pendingEvent = (sender, value);
                return;
            }

            InvokeHandlers(sender, value);
        }

        private void InvokeHandlers(object sender, TEventArgs value)
        {
            _handlers.ForEach(handler => handler.Invoke(sender, value));
        }

        private void InvokeHandlersIfHasPendingEvent()
        {
            if (_pendingEvent is null)
                return;

            InvokeHandlers(_pendingEvent.Value.sender, _pendingEvent.Value.eventArgs);
            _pendingEvent = null;
        }

        public void ClearSubscribers()
        {
            _handlers.Clear();
        }
    }
}
