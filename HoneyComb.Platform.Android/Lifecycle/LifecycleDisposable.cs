using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Lifecycle-aware composite disposable collection.
    /// It will dispose all itens when an Lifecycle component
    /// is destroyed.
    ///
    /// Use it to dispose non Lifecycle-aware components,
    /// therefore avoid memory leaks in your application.
    /// </summary>
    public class LifecycleDisposable : IDisposable
    {
        private readonly List<Action> disposables = new List<Action>();

        internal LifecycleDisposable(LifecycleObservable observable)
        {
            disposables.Add(() => observable.OnDestroy -= LifecycleObservableOnDestroy);
            observable.OnDestroy += LifecycleObservableOnDestroy;
        }

        public bool IsDisposed { get; private set; }

        public void Add(Action disposable)
        {
            GuardIsDisposed();
            disposables.Add(disposable);
        }

        public void Add(IDisposable disposable) => Add(disposable.Dispose);

        public void Dispose()
        {
            GuardIsDisposed();

            disposables.ForEach(disposable => disposable());
            disposables.Clear();
            IsDisposed = true;
        }

        private void GuardIsDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(LifecycleDisposable));
        }

        private void LifecycleObservableOnDestroy(object sender, EventArgs e) => Dispose();
    }
}
