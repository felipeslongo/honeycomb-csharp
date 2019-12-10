using System;
using System.Linq;

namespace HoneyComb.Core.Lifecycles
{
    /// <summary>
    /// Lifecycle states. You can consider the states as 
    /// the nodes in a graph and Lifecycle.Events as 
    /// the edges between these nodes.
    /// </summary>
    /// <remarks>
    /// Credits:
    ///     https://developer.android.com/reference/android/arch/lifecycle/Lifecycle.State
    /// </remarks>
    public enum LifecycleState
    {
        /// <summary>
        /// Initialized state for a LifecycleOwner.
        /// For an LifecycleOwner, this is the state 
        /// when it is constructed but has not received OnActive yet.
        /// </summary>
        Initialized,
        /// <summary>
        /// Active state for a LifecycleOwner.
        /// Can be reached multiple times.
        /// this state is reached in two cases:
        ///     - from Initialized;
        ///     - from Inactive.
        /// </summary>
        Active,
        /// <summary>
        /// Inactive state for a LifecycleOwner.
        /// Can be reached multiple times.
        /// this state is reached only from Active;
        /// </summary>
        Inactive,
        /// <summary>
        /// Destroyed state for a LifecycleOwner. After this event, 
        /// this Lifecycle will not dispatch any more events. 
        /// For instance, for an LifecicleOwner, this state is reached right before OnDispose call.
        /// </summary>
        Disposed
    }
}
