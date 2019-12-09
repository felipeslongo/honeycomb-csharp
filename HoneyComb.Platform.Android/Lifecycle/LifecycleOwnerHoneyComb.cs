using HoneyComb.Platform.SystemH.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="SystemH.Lifecycle.ILifecycleOwner"/>
    /// </summary>
    internal sealed class LifecycleOwnerHoneyComb : ILifecycleOwner
    {
        public SystemH.Lifecycle.Lifecycle Lifecycle { get; private set; }

        public LifecycleOwnerHoneyComb(LifecycleObservable lifecycleObservable)
        {
            Lifecycle = new LifecycleAndroidHoneyComb(this, lifecycleObservable);
        }
    }
}
