using HoneyComb.Platform.Android.Lifecycle;
using System;

namespace HoneyComb.LiveDataNet.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="LiveDataNet.Lifecycle"/>.
    /// </summary>
    public sealed class LifecycleAndroid : LiveDataNet.Lifecycle
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

        private void LifecycleObservable_OnStartOrOnResume(object sender, System.EventArgs e)
        {
            NotifyObserversOfActive();
        }

        private void LifecycleObservable_OnPauseOrOnStop(object sender, System.EventArgs e)
        {
            NotifyObserversOfInactive();
        }

        public override void Dispose()
        {
            base.Dispose();            
        }
    }
}
