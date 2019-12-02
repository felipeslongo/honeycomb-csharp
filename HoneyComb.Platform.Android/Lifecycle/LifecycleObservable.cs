using Android.Arch.Lifecycle;
using Java.Interop;
using System;
using JavaObject = Java.Lang.Object;
using LifecycleAndroid = Android.Arch.Lifecycle.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle-aware observer.
    /// Use it as a Observable object to execute
    /// code that is lifecycle dependent.
    /// </summary>
    public class LifecycleObservable : JavaObject, ILifecycleObserver
    {
        public EventHandler? OnAny;
        public EventHandler? OnCreate;
        public EventHandler? OnDestroy;
        public EventHandler? OnPause;
        public EventHandler? OnResume;
        public EventHandler? OnStart;
        public EventHandler? OnStop;
        private ILifecycleOwner? _lifecycleOwner;

        public void ObserveLifecycleOwner(ILifecycleOwner lifecycleOwner)
        {
            _lifecycleOwner = lifecycleOwner;
            _lifecycleOwner.Lifecycle.AddObserver(this);
        }

        [LifecycleAndroid.Event.OnAny, Export]
        public void OnLifecycleEventOnAny()
        {
            OnAny?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnCreate, Export]
        public void OnLifecycleEventOnCreate()
        {
            OnCreate?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnDestroy, Export]
        public void OnLifecycleEventOnDestroy()
        {
            OnDestroy?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnPause, Export]
        public void OnLifecycleEventOnPause()
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnResume, Export]
        public void OnLifecycleEventOnResume()
        {
            OnResume?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnStart, Export]
        public void OnLifecycleEventOnStart()
        {
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        [LifecycleAndroid.Event.OnStop, Export]
        public void OnLifecycleEventOnStop()
        {
            OnStop?.Invoke(this, EventArgs.Empty);
        }
    }
}
