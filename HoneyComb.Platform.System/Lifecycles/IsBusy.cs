using HoneyComb.Core.Threading;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace HoneyComb.Core.Lifecycles
{
    public sealed class IsBusy : IDisposable
    {
        private readonly MutexLockAsync throttler;

        public IsBusy(bool isBusy = false)
        {
            throttler = new MutexLockAsync(isBusy);
        }

        public bool Value => throttler.IsLocked;

        public static implicit operator bool(IsBusy @this) => @this.Value;

        public void Dispose()
        {
            throttler.Dispose();
        }

        public void Release()
        {
            throttler.Release();
        }

        public async Task<IDisposable> WaitAsync()
        {
            await WaitAsync().ConfigureAwait(false);
            return Disposable.Create(Release);
        }
    }
}
