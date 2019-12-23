using HoneyComb.Core.Lifecycles;
using System;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="Core.Lifecycles.Lifecycle"/>
    /// </summary>
    public sealed class LifecycleiOSHoneyComb : Lifecycle
    {
        private LifecycleDisposable _lifecycleDisposable;

        public LifecycleiOSHoneyComb(ILifecycleOwner owner, LifecycleObservable lifecycleObservable) : base(owner)
        {
            SynchronizeUpToCurrentState(lifecycleObservable.StateLastKnown);

            lifecycleObservable.ViewWillAppear += LifecycleObservable_ViewWillOrDidAppear;
            lifecycleObservable.ViewDidAppear += LifecycleObservable_ViewWillOrDidAppear;

            lifecycleObservable.ViewWillDisappear += LifecycleObservable_ViewWillOrDidDisappear;
            lifecycleObservable.ViewDidDisappear += LifecycleObservable_ViewWillOrDidDisappear;

            _lifecycleDisposable = LifecycleService.GetDisposable(lifecycleObservable);
            _lifecycleDisposable.Add(() =>
            {
                if (lifecycleObservable is null)
                    return;

                lifecycleObservable.ViewWillAppear -= LifecycleObservable_ViewWillOrDidAppear;
                lifecycleObservable.ViewDidAppear -= LifecycleObservable_ViewWillOrDidAppear;

                lifecycleObservable.ViewWillDisappear -= LifecycleObservable_ViewWillOrDidDisappear;
                lifecycleObservable.ViewDidDisappear -= LifecycleObservable_ViewWillOrDidDisappear;

                NotifyStateChange(LifecycleState.Disposed);
                Dispose();
            });
        }

        private void LifecycleObservable_ViewWillOrDidAppear(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Active);

        private void LifecycleObservable_ViewWillOrDidDisappear(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Inactive);

        private void SynchronizeUpToCurrentState(iOSLifecycleState state)
        {
            switch (state)
            {
                case iOSLifecycleState.Initialized:
                    break;

                case iOSLifecycleState.Loaded:
                    break;

                case iOSLifecycleState.Appeared:
                    NotifyStateChange(LifecycleState.Active);
                    break;

                case iOSLifecycleState.Disappeared:
                    NotifyStateChange(LifecycleState.Inactive);
                    break;

                case iOSLifecycleState.DismissedOrRemoved:
                    NotifyStateChange(LifecycleState.Inactive);
                    NotifyStateChange(LifecycleState.Disposed);
                    break;

                default:
                    break;
            }
        }
    }
}
