using Android.Arch.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Provides utilities and helpers related 
    /// to Android Lifecycle Components.
    /// </summary>
    public static class LifecycleService
    {
        public static LifecycleObservable GetObservable(ILifecycleOwner lifecycleOwner)
        {
            var observable = new LifecycleObservable();
            return observable;
        }
    }
}
