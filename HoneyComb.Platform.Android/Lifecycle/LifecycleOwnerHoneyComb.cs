using HoneyComb.Core.Lifecycle;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="Core.Lifecycle.ILifecycleOwner"/>
    /// </summary>
    internal sealed class LifecycleOwnerHoneyComb : ILifecycleOwner
    {
        public Core.Lifecycle.Lifecycle Lifecycle { get; private set; }

        public LifecycleOwnerHoneyComb(LifecycleObservable lifecycleObservable)
        {
            Lifecycle = new LifecycleAndroidHoneyComb(this, lifecycleObservable);
        }
    }
}
