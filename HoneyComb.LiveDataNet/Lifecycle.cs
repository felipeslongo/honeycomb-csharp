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
    public abstract class Lifecycle
    {
        public List<ILifecycleObserver> _observers = new List<ILifecycleObserver>();

        public IDisposable Subscribe(ILifecycleObserver lifecycleObserver)
        {
            _observers.Add(lifecycleObserver);
            return Disposable.Create(() => _observers.Remove(lifecycleObserver));
        }
    }
}