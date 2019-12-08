using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace HoneyComb.Platform.System.Lifecycle
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

        public event EventHandler? OnActive;
        public event EventHandler? OnInactive;
        public event EventHandler? OnDisposed;

        public LifecycleState CurrentState { get; private set; } = LifecycleState.Initialized;

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

        protected void NotifyStateChange(LifecycleState state)
        {
            if (CurrentState == state)
                return;

            CurrentState = CurrentState.ChangeState(state);
            NotifyObserversOfCurrentState();
        }

        private void ClearObservers()
        {
            observers.Clear();
            OnActive = null;
            OnInactive = null;
            OnDisposed = null;
        }

        private void NotifyObserversOfCurrentState()
        {
            switch (CurrentState)
            {
                case LifecycleState.Initialized:
                    break;
                case LifecycleState.Active:
                    NotifyObserversOfActive();
                    break;
                case LifecycleState.Inactive:
                    NotifyObserversOfInactive();
                    break;
                case LifecycleState.Disposed:
                    NotifyObserversOfDisposed();
                    break;
                default:
                    break;
            }
        }

        private void NotifyObserversOfActive()
        {
            observers.ForEach(observer => observer.OnActive(owner!));
            OnActive?.Invoke(owner!, EventArgs.Empty);
        }

        private void NotifyObserversOfDisposed()
        {
            observers.ForEach(observer => observer.OnDisposed(owner!));
            OnDisposed?.Invoke(owner!, EventArgs.Empty);
        }

        private void NotifyObserversOfInactive()
        {
            observers.ForEach(observer => observer.OnInactive(owner!));
            OnInactive?.Invoke(owner!, EventArgs.Empty);
        }
    }
}