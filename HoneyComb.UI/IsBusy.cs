using HoneyComb.Core.Threading;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HoneyComb.UI
{
    /// <summary>
    ///     Represents the concept of an Busy state
    ///     to be used in a ViewModel.
    /// </summary>
    public sealed class IsBusy : IDisposable, IObservable<bool>
    {
        private readonly MutexLockAsync throttler;
        private readonly Lazy<IObservable<bool>> asObservable;

        public IsBusy(bool isBusy = false)
        {
            throttler = new MutexLockAsync(isBusy);
            asObservable = new Lazy<IObservable<bool>>(() => Observable.FromEventPattern<bool>(
                eventHandler => ValueChanged += eventHandler,
                eventHandler => ValueChanged -= eventHandler
                ).Select(eventPattern => eventPattern.EventArgs));
        }

        public event EventHandler<bool>? ValueChanged;

        public bool Value => throttler.IsLocked;

        public static implicit operator bool(IsBusy @this) => @this.Value;

        public void Dispose()
        {
            throttler.Dispose();
        }

        public void Release()
        {
            ValueChanged?.Invoke(this, false);
            throttler.Release();
        }

        public async Task<IDisposable> WaitAsync()
        {
            await throttler.WaitAsync().ConfigureAwait(false);
            ValueChanged?.Invoke(this, true);
            return Disposable.Create(Release);
        }

        public IDisposable Subscribe(IObserver<bool> observer) => asObservable.Value.Subscribe(observer);
    }
}
