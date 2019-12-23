using HoneyComb.Core.Lifecycles;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     Companion object that encapsulates
    ///     both LifecyclerOwners from iOS (currently not done yet)
    ///     and from Honeycomb
    /// </summary>
    public class LifecycleOwners
    {
        public LifecycleOwners(LifecycleObservable lifecycleObservable)
        {
            Observable = lifecycleObservable;
            Disposable = LifecycleService.GetDisposable(Observable);
            HoneyComb = LifecycleService.GetHoneyCombLifecycleOwner(Observable);
        }

        public LifecycleDisposable Disposable { get; }
        public ILifecycleOwner HoneyComb { get; }
        public LifecycleObservable Observable { get; }
    }
}
