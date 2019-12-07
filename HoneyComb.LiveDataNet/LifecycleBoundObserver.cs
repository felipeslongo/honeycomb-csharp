using System;

namespace HoneyComb.LiveDataNet
{
    internal sealed class LifecycleBoundObserver<TEventArgs> : ILifecycleObserver
    {
        private EventHandler<TEventArgs>? _eventHandler;
        private (object sender, TEventArgs eventArgs)? _pendingEvent;
        private IDisposable? _subscription;

        public LifecycleBoundObserver(IDisposable subscription, EventHandler<TEventArgs> eventHandler)
        {
            _subscription = subscription;
            _eventHandler = eventHandler;
        }

        public bool IsActive { get; private set; }

        public void OnActive()
        {
            if (IsActive == false)
                InvokeHandlerIfHasPendingEvent();

            IsActive = true;
        }

        public void OnInactive()
        {
            IsActive = false;
        }

        public void OnDisposed()
        {
            _subscription.Dispose();
            _subscription = null;
            _pendingEvent = null;
            _eventHandler = null;
        }

        public void Invoke(object sender, TEventArgs value)
        {
            if (IsActive == false)
            {
                _pendingEvent = (sender, value);
                return;
            }

            _eventHandler!.Invoke(sender, value);
        }

        private void InvokeHandlerIfHasPendingEvent()
        {
            if (_pendingEvent is null)
                return;

            _eventHandler!.Invoke(_pendingEvent.Value.sender, _pendingEvent.Value.eventArgs);
            _pendingEvent = null;
        }
    }
}
