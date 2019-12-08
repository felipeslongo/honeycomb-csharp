namespace HoneyComb.LiveDataNet
{
    /// <summary>
    /// Callback interface for listening to <see cref="ILifecycleOwner"/> state changes.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://developer.android.com/reference/androidx/lifecycle/LifecycleObserver.html
    ///     https://developer.android.com/reference/androidx/lifecycle/DefaultLifecycleObserver.html
    ///     https://developer.android.com/reference/androidx/lifecycle/LifecycleEventObserver.html
    /// </remarks>
    public interface ILifecycleObserver
    {
        void OnActive(ILifecycleOwner owner);

        void OnDisposed(ILifecycleOwner owner);

        void OnInactive(ILifecycleOwner owner);
    }
}
