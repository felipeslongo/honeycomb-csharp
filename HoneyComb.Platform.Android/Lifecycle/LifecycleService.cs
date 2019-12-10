using Android.Arch.Lifecycle;
using HoneyCombLifecycleOwner = HoneyComb.Core.Lifecycles.ILifecycleOwner;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Provides utilities and helpers related 
    /// to Android Lifecycle Components.
    /// </summary>
    public static class LifecycleService
    {
        public static LifecycleObservable GetObservable(ILifecycleOwner lifecycleOwner) =>
            new LifecycleObservable(lifecycleOwner);

        public static LifecycleDisposable GetDisposable(ILifecycleOwner lifecycleOwner) =>
            new LifecycleDisposable(GetObservable(lifecycleOwner));

        public static LifecycleDisposable GetDisposable(LifecycleObservable lifecycleObservable) =>
            new LifecycleDisposable(lifecycleObservable);

        public static HoneyCombLifecycleOwner GetHoneyCombLifecycleOwner(LifecycleObservable lifecycleObservable) =>
            new LifecycleOwnerHoneyComb(lifecycleObservable);
    }
}
