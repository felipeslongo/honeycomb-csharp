using HoneyComb.Core;
using HoneyComb.Core.Lifecycles;
using System;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Lifecycle-aware EventHandler.
    /// Similar to <see cref="LiveData{T}"/>
    /// but dispatch events only once.
    ///
    /// It's the notion that events should
    /// be consumed and handled only once.
    /// </summary>
    public class LiveEvent<TEventArgs> : LiveData<Event<TEventArgs>>
    {
        public LiveEvent() : base()
        {
            Init();
        }

        public LiveEvent(TEventArgs value) : base(new Event<TEventArgs>(value))
        {
            Init();
        }

        public LiveEvent(MutableLiveData<Event<TEventArgs>> eventSource) : base(eventSource)
        {
            Init();
        }

        /// <summary>
        /// Subscribe to be notified of new events,
        /// even if is already handled.
        /// </summary>
        public event EventHandler<Event<TEventArgs>>? EventChanged;        

        private void Init() => BindMethod(InvokeEventChanged);

        private void InvokeEventChanged(Event<TEventArgs> @event) => EventChanged?.Invoke(this, @event);
    }
}
