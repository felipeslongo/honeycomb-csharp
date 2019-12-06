using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.Android.Lifecycle
{
    public sealed class LifecycleEventHandler<T>
    {
        private readonly List<EventHandler<T>> _handlers = new List<EventHandler<T>>();
        private readonly LifecycleObservable _lifecycleObservable;
        private (object sender, T value)? _pendingEvent;

        public LifecycleEventHandler(LifecycleObservable lifecycleObservable)
        {
            _lifecycleObservable = lifecycleObservable;
            _lifecycleObservable.OnResume += (sender, args) => InvokeHandlersIfHasPendingEvent();
            _lifecycleObservable.OnStart += (sender, args) => InvokeHandlersIfHasPendingEvent();
        }

        public bool IsActive =>
            _lifecycleObservable.StateLastKnown.IsAtLeast(global::Android.Arch.Lifecycle.Lifecycle.State.Started);

        public event EventHandler<T> Event
        {
            add => _handlers.Add(value);
            remove => _handlers.Remove(value);
        }

        public void Invoke(object sender, T value)
        {
            if (IsActive == false)
            {
                _pendingEvent = (sender, value);
                return;
            }

            Invokehandlers(sender, value);
        }

        private void Invokehandlers(object sender, T value)
        {
            _handlers.ForEach(handler => handler.Invoke(sender, value));
        }

        private void InvokeHandlersIfHasPendingEvent()
        {
            if (_pendingEvent is null)
                return;

            Invokehandlers(_pendingEvent.Value.sender, _pendingEvent.Value.value);
        }
    }
}
