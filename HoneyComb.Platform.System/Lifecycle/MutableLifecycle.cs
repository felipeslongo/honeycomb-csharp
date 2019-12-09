namespace HoneyComb.Platform.SystemH.Lifecycle
{
    /// <summary>
    /// Lifecycle that exposes its State as mutable.
    /// </summary>
    public sealed class MutableLifecycle : Lifecycle
    {
        public MutableLifecycle(ILifecycleOwner owner) : base(owner)
        {
        }

        public new void NotifyStateChange(LifecycleState state) => base.NotifyStateChange(state);
    }
}
