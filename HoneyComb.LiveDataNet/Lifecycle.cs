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
            observers.ForEach(observer => observer.OnActive(owner!));
        }

        protected void NotifyObserversOfDisposed()
        {
            observers.ForEach(observer => observer.OnDisposed(owner!));
        }

        protected void NotifyObserversOfInactive()
        {
            observers.ForEach(observer => observer.OnInactive(owner!));
        }
    }
}