using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Defines an object that has an generic Lifecycle.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://developer.android.com/reference/androidx/lifecycle/Lifecycle.html
    /// </remarks>
    public abstract class Lifecycle : IDisposable
    {
        private LifecycleState state = LifecycleState.Initialized;
        private readonly List<ILifecycleObserver> observers = new List<ILifecycleObserver>();
        private ILifecycleOwner? owner;

        protected Lifecycle(ILifecycleOwner owner)
        {
            this.owner = owner;
        }

        public virtual void Dispose()
        {
            owner = null;
            ClearObservers();
        }

        public IDisposable Subscribe(ILifecycleObserver lifecycleObserver)
        {
            observers.Add(lifecycleObserver);
            return Disposable.Create(() => observers.Remove(lifecycleObserver));
        }

        protected void ClearObservers() => observers.Clear();

        protected void NotifyObserversOfActive()
        {
            if (state == LifecycleState.Active)
                return;

            state = state.ChangeState(LifecycleState.Active);
            observers.ForEach(observer => observer.OnActive(owner!));
        }

        protected void NotifyObserversOfDisposed()
        {
            if (state == LifecycleState.Disposed)
                return;

            state = state.ChangeState(LifecycleState.Disposed);
            observers.ForEach(observer => observer.OnDisposed(owner!));
        }

        protected void NotifyObserversOfInactive()
        {
            if (state == LifecycleState.Inactive)
                return;

            state = state.ChangeState(LifecycleState.Inactive);
            observers.ForEach(observer => observer.OnInactive(owner!));
        }
    }
}