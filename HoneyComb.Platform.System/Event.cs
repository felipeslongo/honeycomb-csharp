using System;

namespace Core.Infraestrutura.UI
{
    /// <summary>
    /// Represents a regular Event
    /// that should be consumed only once,
    /// like an Android Snackbar message,
    /// a navigation event or a dialog trigger.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://medium.com/androiddevelopers/livedata-with-snackbar-navigation-and-other-events-the-singleliveevent-case-ac2622673150
    /// </remarks>
    public class Event : EventArgs
    {
        /// <summary>
        /// Returns an indicating if the event has been handled.
        /// </summary>
        public bool IsHandled { get; private set; }

        /// <summary>
        /// Executes an action that consumes
        /// the event and prevents its use again.
        /// </summary>
        /// <param name="action">Action to be executed on the vent.</param>
        public void ExecuteIfUnhandled(Action action)
        {
            if (IsHandled)
                return;

            action();
            IsHandled = true;
        }
    }

    /// <summary>
    /// Represents a regular Event
    /// that should be consumed only once,
    /// like an Android Snackbar message,
    /// a navigation event or a dialog trigger.
    /// </summary>
    /// <typeparam name="T">Event content/args to be handled.</typeparam>
    public class Event<T> : Event
    {
        /// <summary>
        /// Event content/args.
        /// </summary>
        private readonly T content;

        /// <summary>
        /// Construct an event with content.
        /// </summary>
        /// <param name="content">Content to be handled.</param>
        public Event(T content)
        {
            this.content = content;
        }

        /// <summary>
        /// Returns the content, even if it's already been handled.
        /// </summary>
        public T PeekContent => content;

        /// <summary>
        /// Executes an action that consumes the event
        /// content and prevents its use again.
        /// </summary>
        /// <param name="action">Action that consumes the event content.</param>
        public void ExecuteIfUnhandled(Action<T> action) =>
            base.ExecuteIfUnhandled(() => { action(content); });
    }
}