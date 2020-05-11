using HoneyComb.Core.Threading;
using HoneyComb.Core.Vault;
using HoneyComb.LiveDataNet;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace HoneyComb.UI
{
    /// <summary>
    ///     Represents the concept of an Busy state
    ///     to be used in a ViewModel.
    /// </summary>
    public sealed class IsBusy : IDisposable, IRestorableUIState
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

        public string? RestorationIdentifier { get; set; } = typeof(IsBusy).FullName;

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

        public void OnPreservation(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            savedInstanceState.Add(RestorationIdentifier!, Value);
        }

        public void OnRestoration(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            var wasBusy = savedInstanceState.GetBoolean(RestorationIdentifier!);
            if(wasBusy)
            {
                _ = WaitAsync();
                return;
            }
            Release();
        }
    }
}
