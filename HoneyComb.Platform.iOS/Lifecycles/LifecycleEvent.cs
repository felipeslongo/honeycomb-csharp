namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Lifecycle Events. You can consider the states as the nodes in a graph and Lifecycle.Events as the edges between these nodes.
    /// </summary>
    /// <remarks>
    /// Credits: https://developer.android.com/reference/androidx/lifecycle/Lifecycle.Event.html
    /// </remarks>
    public enum LifecycleEvent
    {
        LoadView,

        LoadViewIfNeeded,

        ViewDidLoad,

        ViewWillAppear,

        ViewWillLayoutSubviews,

        ViewDidLayoutSubviews,

        ViewDidAppear,

        ViewWillDisappear,

        ViewDidDisappear,
    }
}
