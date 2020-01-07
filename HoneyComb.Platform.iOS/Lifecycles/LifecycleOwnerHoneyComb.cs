using HoneyComb.Core.Lifecycles;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    /// iOS Lifecycle bound implementation of <see cref="ILifecycleOwner"/>
    /// </summary>
    internal sealed class LifecycleOwnerHoneyComb : ILifecycleOwner
    {
        public LifecycleOwnerHoneyComb(LifecycleObservable lifecycleObservable)
        {
            Lifecycle = new LifecycleiOSHoneyComb(this, lifecycleObservable);
        }

        public Lifecycle Lifecycle { get; private set; }
    }
}
