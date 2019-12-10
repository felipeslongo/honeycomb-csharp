using HoneyComb.Core.Lifecycles;

namespace HoneyComb.Platform.Android.Lifecycle
{
    /// <summary>
    /// Android Lifecycle bound implementation of <see cref="Core.Lifecycles.ILifecycleOwner"/>
    /// </summary>
    internal sealed class LifecycleOwnerHoneyComb : ILifecycleOwner
    {
        public Core.Lifecycles.Lifecycle Lifecycle { get; private set; }

        public LifecycleOwnerHoneyComb(LifecycleObservable lifecycleObservable)
        {
            Lifecycle = new LifecycleAndroidHoneyComb(this, lifecycleObservable);
        }
    }
}
