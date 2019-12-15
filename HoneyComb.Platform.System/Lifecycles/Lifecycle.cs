using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Linq;
using HoneyComb.Core.Linq;

namespace HoneyComb.Core.Lifecycles
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

        public event EventHandler<EventArgs>? OnActive;
        public event EventHandler<EventArgs>? OnInactive;
        public event EventHandler<EventArgs>? OnDisposed;

        /// <summary>
        /// Returns the current state of the Lifecycle.
        /// </summary>
        public LifecycleState CurrentState { get; private set; } = LifecycleState.Initialized;

        public virtual void Dispose()
        {
            owner = null;
            ClearObservers();
        }

        /// <summary>
        /// Adds a LifecycleObserver that will be notified when the LifecycleOwner changes state.
        /// The given observer will be brought to the current state of the LifecycleOwner. 
        /// For example, if the LifecycleOwner is in Disposed state, the given observer will receive OnInactive, OnDisposed events.
        /// </summary>
        /// <param name="lifecycleObserver"></param>
        /// <returns></returns>
        /// <remarks>
        /// Credits:
        ///     https://developer.android.com/reference/android/arch/lifecycle/Lifecycle.html#addObserver(android.arch.lifecycle.LifecycleObserver)
        /// </remarks>
        public IDisposable Subscribe(ILifecycleObserver lifecycleObserver)
        {
            SynchronizeUpToCurrentState(lifecycleObserver);
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
            observers.ForeachImmutable(observer => observer.OnActive(owner!));
            OnActive?.Invoke(owner!, EventArgs.Empty);
        }

        private void NotifyObserversOfDisposed()
        {
            observers.ForeachImmutable(observer => observer.OnDisposed(owner!));
            OnDisposed?.Invoke(owner!, EventArgs.Empty);
        }

        private void NotifyObserversOfInactive()
        {
            observers.ForeachImmutable(observer => observer.OnInactive(owner!));
            OnInactive?.Invoke(owner!, EventArgs.Empty);
        }

        private void SynchronizeUpToCurrentState(ILifecycleObserver lifecycleObserver)
        {
            switch (CurrentState)
            {
                case LifecycleState.Initialized:
                    break;
                case LifecycleState.Active:
                    lifecycleObserver.OnActive(owner!);
                    break;
                case LifecycleState.Inactive:
                    lifecycleObserver.OnInactive(owner!);
                    break;
                case LifecycleState.Disposed:
                    lifecycleObserver.OnInactive(owner!);
                    lifecycleObserver.OnDisposed(owner!);
                    break;
                default:
                    break;
            }
        }
    }
}