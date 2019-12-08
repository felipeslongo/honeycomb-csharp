using HoneyComb.Platform.System.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="HoneyComb.Platform.System.Lifecycle.ILifecycleOwner"/>
    /// </summary>
    internal sealed class LifecycleOwnerHoneyComb : ILifecycleOwner
    {
        public System.Lifecycle.Lifecycle Lifecycle { get; private set; }

        public LifecycleOwnerHoneyComb(LifecycleObservable lifecycleObservable)
        {
            Lifecycle = new LifecycleAndroidHoneyComb(this, lifecycleObservable);
        }
    }
}
