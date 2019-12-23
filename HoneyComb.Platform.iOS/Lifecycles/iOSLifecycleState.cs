namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Lifecycle States. You can consider the states as the nodes in a graph and Lifecycle.Events as the edges between these nodes.
    /// </summary>
    /// <remarks>
    /// Credits: https://developer.android.com/reference/androidx/lifecycle/Lifecycle.Event.html
    /// </remarks>
    public enum iOSLifecycleState
    {
        /// <summary>
        /// Initialized state for a LifecycleOwner. For an ViewController, this is the state when it is constructed but has not received ViewDidLoad yet.
        /// </summary>
        Initialized,
        /// <summary>
        /// Loaded state for a LifecycleOwner. For an ViewController, this is the state after ViewDidLoad was called but has not received ViewWillAppear yet.
        /// </summary>
        Loaded,
        /// <summary>
        /// Appeared state for a LifecycleOwner. For an ViewController, this is the state after ViewDidAppear was called.
        /// </summary>
        Appeared,
        /// <summary>
        /// Appeared state for a LifecycleOwner. For an ViewController, this is the state after ViewWillDisappear was called.
        /// </summary>
        Disappeared,
        /// <summary>
        /// Dismissed Or Removed as child controller state for a LifecycleOwner. After this event, this Lifecycle will not dispatch any more events.
        /// </summary>
        DismissedOrRemoved,
    }
}
