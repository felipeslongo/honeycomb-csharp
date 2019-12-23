using System;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     iOS Lifecycle-aware observer.
    ///     Use it as a Observable object to execute
    ///     code that is lifecycle dependent.
    /// </summary>
    /// <remarks>
    ///     Inspiration
    ///     - LifecycleBoundObserver:
    ///     https://android.googlesource.com/platform/frameworks/support/+/androidx-master-dev/lifecycle/lifecycle-livedata-core/src/main/java/androidx/lifecycle/LiveData.java
    /// </remarks>
    public class LifecycleObservable
    {
        public event EventHandler? LoadView;

        public event EventHandler? LoadViewIfNeeded;

        public event EventHandler? ViewDidLoad;

        public event EventHandler? ViewWillAppear;

        public event EventHandler? ViewWillLayoutSubviews;

        public event EventHandler? ViewDidLayoutSubviews;

        public event EventHandler? ViewDidAppear;

        public event EventHandler? ViewWillDisappear;

        public event EventHandler? ViewDidDisappear;

        public event EventHandler? ViewWillDismissOrRemove;

        public event EventHandler? ViewDidDismissOrRemove;

        /// <summary>
        ///     Gets the last current state captured.
        /// </summary>
        public iOSLifecycleState StateLastKnown { get; internal set; } = iOSLifecycleState.Initialized;

        protected void InvokeLoadView() => LoadView?.Invoke(this, EventArgs.Empty);

        protected void InvokeLoadViewIfNeeded() => LoadViewIfNeeded?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidLoad() => ViewDidLoad?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewWillAppear() => ViewWillAppear?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewWillLayoutSubviews() => ViewWillLayoutSubviews?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidLayoutSubviews() => ViewDidLayoutSubviews?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidAppear()
        {
            StateLastKnown = iOSLifecycleState.Appeared;
            ViewDidAppear?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeViewWillDisappear() => ViewWillDisappear?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidDisappear()
        {
            StateLastKnown = iOSLifecycleState.Disappeared;
            ViewDidDisappear?.Invoke(this, EventArgs.Empty);
        }

        protected void InvokeViewWillDismissOrRemove() => ViewWillDismissOrRemove?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidDismissOrRemove()
        {
            StateLastKnown = iOSLifecycleState.DismissedOrRemoved;
            ViewDidDismissOrRemove?.Invoke(this, EventArgs.Empty);
        }
    }
}
