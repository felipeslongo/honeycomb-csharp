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

        public event EventHandler? ViewWillDestroy;

        public event EventHandler? ViewDidDestroy;

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

        protected void InvokeViewDidAppear() => ViewDidAppear?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewWillDisappear() => ViewWillDisappear?.Invoke(this, EventArgs.Empty);

        protected void InvokeViewDidDisappear() => ViewDidDisappear?.Invoke(this, EventArgs.Empty);
    }
}
