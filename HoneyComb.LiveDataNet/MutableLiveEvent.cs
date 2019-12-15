using HoneyComb.Core;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Lifecycle-aware EventHandler.
    /// Similar to <see cref="LiveData{T}"/>
    /// but dispatch events only once.
    ///
    /// Like <see cref="MutableLiveData{T}"/>
    /// dont expose this to your clients,
    /// expode an <see cref="LiveEvent{TEventArgs}"/>
    /// </summary>
    public class MutableLiveEvent<TEventArgs> : LiveEvent<TEventArgs>
    {
        public MutableLiveEvent()
        {
        }

        public MutableLiveEvent(TEventArgs value) : base(value)
        {
        }

        public MutableLiveEvent(MutableLiveData<Event<TEventArgs>> eventSource) : base(eventSource)
        {
        }

        /// <summary>
        /// Invoke/Notify the subscribers with a new event.
        /// </summary>
        /// <param name="eventArgs"></param>
        public void Invoke(TEventArgs eventArgs) => liveData.Value = new Event<TEventArgs>(eventArgs);
    }
}
