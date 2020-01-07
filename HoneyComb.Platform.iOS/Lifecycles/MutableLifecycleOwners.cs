namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     Companion object that encapsulates
    ///     both LifecyclerOwners from iOS (currently not done yet)
    ///     and from Honeycomb.
    ///     
    ///     Only the UIViewController that owns this instance
    ///     should touch it. Other objects should not be exposed to it.
    ///     
    ///     Use it to favor Composition over Inheritance.
    /// </summary>
    public class MutableLifecycleOwners : LifecycleOwners
    {
        public MutableLifecycleOwners(MutableLifecycleObservable lifecycleObservable) : base(lifecycleObservable)
        {
            MutableObservable = lifecycleObservable;
            SimplifiedObservable = new MutableLifecycleObservableSimplified(MutableObservable);
    }

        /// <summary>
        /// Gets a mutable LifecycleObservable that you can use to notify
        /// lifecycle events of your UIViewController.
        /// </summary>
        public MutableLifecycleObservable MutableObservable { get; }

        /// <summary>
        /// Gets a simplified mutable LifecycleObservable that you can use to notify
        /// the bare minimum lifecycle events of your UIViewController.
        /// </summary>
        public MutableLifecycleObservableSimplified SimplifiedObservable { get; }
    }
}
