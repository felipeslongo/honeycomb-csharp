namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// Provides utilities and helpers related 
    /// to iOS Lifecycle Components.
    /// </summary>
    public static class LifecycleService
    {
        public static LifecycleDisposable GetDisposable(LifecycleObservable lifecycleObservable) =>
            new LifecycleDisposable(lifecycleObservable);
    }
}