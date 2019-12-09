using Android.Arch.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    ///     Companion object that encapsulates
    ///     both LifecyclerOwners from Android
    ///     and from Honeycomb
    /// </summary>
    public sealed class LifecycleOwners
    {
        public LifecycleOwners(ILifecycleOwner lifecycleOwner)
        {
            Android = lifecycleOwner;
            Observable = LifecycleService.GetObservable(lifecycleOwner);
            Disposable = LifecycleService.GetDisposable(Observable);
            HoneyComb = LifecycleService.GetHoneyCombLifecycleOwner(Observable);
        }

        public ILifecycleOwner Android { get; }
        public LifecycleDisposable Disposable { get; }
        public Core.Lifecycle.ILifecycleOwner HoneyComb { get; }
        public LifecycleObservable Observable { get; }
    }
}
