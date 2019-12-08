using HoneyComb.Platform.System.Lifecycle;
using System;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="LiveDataNet.Lifecycle"/>.
    /// </summary>
    public sealed class LifecycleAndroid : Platform.System.Lifecycle.Lifecycle
    {
        private LifecycleDisposable _lifecycleDisposable;

        public LifecycleAndroid(ILifecycleOwner owner, LifecycleObservable lifecycleObservable) : base(owner)
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

                NotifyObserversOfDisposed();
                Dispose();
            });
        }

        private void LifecycleObservable_OnStartOrOnResume(object sender, EventArgs e)
        {
            NotifyObserversOfActive();
        }

        private void LifecycleObservable_OnPauseOrOnStop(object sender, EventArgs e)
        {
            NotifyObserversOfInactive();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
