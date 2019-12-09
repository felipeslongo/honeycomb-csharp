using HoneyComb.Core.Lifecycle;
using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Wrapper to wich the LiveData delegates the
    /// responsability of notify its observer
    /// respecting the lifecycle provided.
    /// </summary>
    internal sealed class LifecycleBoundObserver : ILifecycleObserver, IDisposable
    {
        private EventHandler<EventArgs>? _eventHandler;
        private (object sender, EventArgs eventArgs)? _pendingEvent;

        public LifecycleBoundObserver(EventHandler<EventArgs> eventHandler)
        {
            _eventHandler = eventHandler;
        }

        public bool IsActive { get; private set; }
        public IDisposable? Subscription { get; set; }

        public void Dispose()
        {
            Subscription?.Dispose();
            Subscription = null;
            _pendingEvent = null;
            _eventHandler = null;
        }

        public void Invoke(object sender, EventArgs value)
        {
            if (IsActive == false)
            {
                _pendingEvent = (sender, value);
                return;
            }

            _eventHandler!.Invoke(sender, value);
        }

        public void OnActive(ILifecycleOwner _)
        {
            if (IsActive == false)
                InvokeHandlerIfHasPendingEvent();

            IsActive = true;
        }

        public void OnDisposed(ILifecycleOwner _) => Dispose();

        public void OnInactive(ILifecycleOwner _) => IsActive = false;

        private void InvokeHandlerIfHasPendingEvent()
        {
            if (_pendingEvent is null)
                return;

            _eventHandler!.Invoke(_pendingEvent.Value.sender, _pendingEvent.Value.eventArgs);
            _pendingEvent = null;
        }
    }
}
