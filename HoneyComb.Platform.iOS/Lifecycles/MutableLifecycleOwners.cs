using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyComb.Platform.iOS.Lifecycles
{
    /// <summary>
    ///     Companion object that encapsulates
    ///     both LifecyclerOwners from iOS (currently not done yet)
    ///     and from Honeycomb.
    ///     
    ///     Only the UIViewController that owns this instance
    ///     should touch it. Other objects should not be exposed to it.
    /// </summary>
    public class MutableLifecycleOwners : LifecycleOwners
    {
        public MutableLifecycleOwners(MutableLifecycleObservable lifecycleObservable) : base(lifecycleObservable)
        {
            MutableObservable = lifecycleObservable;
    }

        public MutableLifecycleObservable MutableObservable { get; }
    }
}
