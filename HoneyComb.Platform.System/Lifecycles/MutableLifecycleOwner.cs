namespace HoneyComb.Core.Lifecycles
{
    public sealed class MutableLifecycleOwner : ILifecycleOwner
    {
        public MutableLifecycleOwner()
        {
            Lifecycle = new MutableLifecycle(this);
        }

        public Lifecycle Lifecycle { get; }
    }
}
