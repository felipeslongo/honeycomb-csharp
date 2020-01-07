using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Lifecycle-aware composite disposable collection.
    /// It will dispose all itens when an Lifecycle component
    /// is destroyed.
    ///
    /// Use it to dispose non Lifecycle-aware components,
    /// therefore avoid memory leaks in your application.
    /// </summary>
    public class LifecycleDisposable
    {
        private readonly List<Action> disposables = new List<Action>();

        public LifecycleDisposable(LifecycleObservable observable)
        {
            disposables.Add(() => 
            {
                observable.ViewWillDismissOrRemove -= LifecycleObservableViewWillOrDidDismissOrRemove;
                observable.ViewDidDismissOrRemove -= LifecycleObservableViewWillOrDidDismissOrRemove; 
            });
            observable.ViewWillDismissOrRemove += LifecycleObservableViewWillOrDidDismissOrRemove;
            observable.ViewDidDismissOrRemove += LifecycleObservableViewWillOrDidDismissOrRemove;
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

        private void LifecycleObservableViewWillOrDidDismissOrRemove(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            Dispose();
        }
    }
}
