using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading
{
    public sealed class MutexLockAsync : IDisposable
    {
        private readonly SemaphoreSlim throttler;

        public MutexLockAsync(bool isBusy = false)
        {
            throttler = SemaphoreSlimFactory.CreateMutexLock(isBusy);
        }

        public bool IsLocked => throttler.IsLocked();

        public void Dispose()
        {
            throttler.Dispose();
        }

        public void Release()
        {
            if (throttler.IsUnlocked())
                return;

            throttler.Release();
        }

        public async Task<IDisposable> WaitAsync()
        {
            await throttler.WaitAsync().ConfigureAwait(false);
            return Disposable.Create(Release);
        }
    }
}
