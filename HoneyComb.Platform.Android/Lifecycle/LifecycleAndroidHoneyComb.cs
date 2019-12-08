using HoneyComb.Platform.System.Lifecycle;
using System;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="HoneyComb.Platform.System.Lifecycle.Lifecycle"/>
    /// </summary>
    internal sealed class LifecycleAndroidHoneyComb : Platform.System.Lifecycle.Lifecycle
    {
        private LifecycleDisposable _lifecycleDisposable;

        public LifecycleAndroidHoneyComb(ILifecycleOwner owner, LifecycleObservable lifecycleObservable) : base(owner)
        {
            lifecycleObservable.OnStart += LifecycleObservable_OnStartOrOnResume;
            lifecycleObservable.OnResume += LifecycleObservable_OnStartOrOnResume;

            lifecycleObservable.OnPause += LifecycleObservable_OnPauseOrOnStop;
            lifecycleObservable.OnStop += LifecycleObservable_OnPauseOrOnStop;

            _lifecycleDisposable = LifecycleService.GetDisposable(lifecycleObservable);
            _lifecycleDisposable.Add(() =>
            {
                if (lifecycleObservable is null)
                    return;

                lifecycleObservable!.OnStart -= LifecycleObservable_OnStartOrOnResume;
                lifecycleObservable.OnResume -= LifecycleObservable_OnStartOrOnResume;

                lifecycleObservable!.OnPause -= LifecycleObservable_OnPauseOrOnStop;
                lifecycleObservable!.OnStop -= LifecycleObservable_OnPauseOrOnStop;

                NotifyStateChange(LifecycleState.Disposed);
                Dispose();
            });
        }

        private void LifecycleObservable_OnStartOrOnResume(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Active);

        private void LifecycleObservable_OnPauseOrOnStop(object sender, EventArgs e) => NotifyStateChange(LifecycleState.Inactive);

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
