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

            var appLifecycleSubscription = ObserveAppLifecycle(LifecycleService.GetAppLifecycleObservable(), lifecycleObservable);
            _lifecycleDisposable = LifecycleService.GetDisposable(lifecycleObservable);
            _lifecycleDisposable.Add(() =>
            {
                if (lifecycleObservable is null)
                    return;

                lifecycleObservable.ViewWillAppear -= LifecycleObservable_ViewWillOrDidAppear;
                lifecycleObservable.ViewDidAppear -= LifecycleObservable_ViewWillOrDidAppear;

                lifecycleObservable.ViewWillDisappear -= LifecycleObservable_ViewWillOrDidDisappear;
                lifecycleObservable.ViewDidDisappear -= LifecycleObservable_ViewWillOrDidDisappear;

                appLifecycleSubscription();

                NotifyStateChange(LifecycleState.Disposed);
                Dispose();
            });
        }

        private void LifecycleObservable_ViewWillOrDidAppear(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Active);

        private void LifecycleObservable_ViewWillOrDidDisappear(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Inactive);

        /// <summary>
        /// Observes the Application Global Lifecycle in order to dispatch updates for inactive state
        /// when the app goes into background...
        /// </summary>
        /// <param name="appLifecycle"></param>
        /// <param name="viewControllerLifecycle"></param>
        /// <returns></returns>
        private Action ObserveAppLifecycle(AppLifecycleObservable appLifecycle, LifecycleObservable viewControllerLifecycle)
        {
            appLifecycle.WillResignActive += OnWillResignActive;
            appLifecycle.DidBecomeActive += OnDidBecomeActive;

            return () =>
            {
                appLifecycle.WillResignActive -= OnWillResignActive;
                appLifecycle.DidBecomeActive -= OnDidBecomeActive;
            };

            void OnWillResignActive(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Inactive);

            void OnDidBecomeActive(object sender, EventArgs e) => SynchronizeUpToCurrentState(viewControllerLifecycle.StateLastKnown);
        }

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
