using HoneyComb.Core.Threading;
using HoneyComb.LiveDataNet;
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
    public sealed class IsBusy : IDisposable
    {
        private readonly MutableLiveData<bool> liveValue;
        private readonly MutexLockAsync throttler;

        public IsBusy(bool isBusy = false)
        {
            throttler = new MutexLockAsync(isBusy);
            liveValue = new MutableLiveData<bool>(isBusy);
        }

        public LiveData<bool> LiveValue => liveValue;
        public bool Value => throttler.IsLocked;

        public static implicit operator bool(IsBusy @this) => @this.Value;

        public void Dispose()
        {
            throttler.Dispose();
        }

        public void Release()
        {
            liveValue.Value = false;
            throttler.Release();
        }

        public async Task<IDisposable> WaitAsync()
        {
            await throttler.WaitAsync().ConfigureAwait(false);
            liveValue.Value = true;
            return Disposable.Create(Release);
        }
    }
}
