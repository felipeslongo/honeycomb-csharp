using System;
using Android.Arch.Lifecycle;
using Java.Interop;
using static Android.Arch.Lifecycle.Lifecycle;
using JavaObject = Java.Lang.Object;
using LifecycleAndroid = Android.Arch.Lifecycle.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    ///     Android Lifecycle-aware observer.
    ///     Use it as a Observable object to execute
    ///     code that is lifecycle dependent.
    /// </summary>
    /// <remarks>
    ///     Inspiration
    ///     - LifecycleBoundObserver:
    ///     https://android.googlesource.com/platform/frameworks/support/+/androidx-master-dev/lifecycle/lifecycle-livedata-core/src/main/java/androidx/lifecycle/LiveData.java
    /// </remarks>
    public class LifecycleObservable : JavaObject, ILifecycleObserver
    {
        private ILifecycleOwner _lifecycleOwner = null;

        internal LifecycleObservable(ILifecycleOwner lifecycleOwner)
        {
            SetLifecycleOwner(lifecycleOwner);
        }

        /// <summary>
        ///     Gets the Lifecycle <see cref="global::Android.Arch.Lifecycle.Lifecycle.CurrentState" /> current state.
        ///     Return null when this observer is removed from the <see cref="global::Android.Arch.Lifecycle.Lifecycle" /> or disposed.
        /// </summary>
        public State StateCurrent => _lifecycleOwner?.Lifecycle.CurrentState;

        /// <summary>
        ///     Gets the last <see cref="StateCurrent" /> captured.
        /// </summary>
        public State StateLastKnown { get; private set; } = State.Initialized;

        public event EventHandler OnAny;
        public event EventHandler OnCreate;
        public event EventHandler OnDestroy;
        public event EventHandler OnPause;
        public event EventHandler OnResume;
        public event EventHandler OnStart;
        public event EventHandler OnStop;

        public void SetLifecycleOwner(ILifecycleOwner lifecycleOwner)
        {
            UnsubscribeFromLifecycleOwner();
            _lifecycleOwner = lifecycleOwner;
            RefreshStateLastKnown();
            _lifecycleOwner.Lifecycle.AddObserver(this);
        }

        [Event.OnAnyAttribute]
        [Export]
        public void OnLifecycleEventOnAny()
        {
            RefreshStateLastKnown();
            OnAny?.Invoke(this, EventArgs.Empty);
        }

        [Event.OnCreateAttribute]
        [Export]
        public void OnLifecycleEventOnCreate()
        {
            OnCreate?.Invoke(this, EventArgs.Empty);
        }

        [Event.OnDestroyAttribute]
        [Export]
        public void OnLifecycleEventOnDestroy()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
            UnsubscribeFromLifecycleOwner();
        }

        [Event.OnPauseAttribute]
        [Export]
        public void OnLifecycleEventOnPause()
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }

        [Event.OnResumeAttribute]
        [Export]
        public void OnLifecycleEventOnResume()
        {
            OnResume?.Invoke(this, EventArgs.Empty);
        }

        [Event.OnStartAttribute]
        [Export]
        public void OnLifecycleEventOnStart()
        {
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        [Event.OnStopAttribute]
        [Export]
        public void OnLifecycleEventOnStop()
        {
            OnStop?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshStateLastKnown()
        {
            StateLastKnown = StateCurrent!;
        }

        private void UnsubscribeFromLifecycleOwner()
        {
            _lifecycleOwner?.Lifecycle.RemoveObserver(this);
            _lifecycleOwner = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) UnsubscribeFromLifecycleOwner();
            base.Dispose(disposing);
        }
    }
}
