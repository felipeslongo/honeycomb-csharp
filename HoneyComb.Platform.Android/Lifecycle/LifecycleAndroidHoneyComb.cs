﻿using HoneyComb.Platform.System.Lifecycle;
using System;
using AndroidState = global::Android.Arch.Lifecycle.Lifecycle.State;

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
            SynchronizeUpToCurrentState(lifecycleObservable.StateCurrent ?? lifecycleObservable.StateLastKnown);
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

        private void SynchronizeUpToCurrentState(AndroidState state)
        {
            if (state == AndroidState.Initialized)
                return;

            if (state == AndroidState.Created)
                return;

            if (state == AndroidState.Started)
            {
                NotifyStateChange(LifecycleState.Active);
                return;
            }

            if (state == AndroidState.Resumed)
            {
                NotifyStateChange(LifecycleState.Active);
                return;
            }

            if (state == AndroidState.Destroyed)
            {
                NotifyStateChange(LifecycleState.Inactive);
                NotifyStateChange(LifecycleState.Disposed);
                return;
            }
        }
    }
}
