using System;
using System.Threading.Tasks;
using HoneyComb.Platform.System.Threading.Tasks;
using TaskFactory = HoneyComb.Core.Threading.Tasks.TaskFactory;

namespace HoneyComb.Core.Lifecycles.Tasks
{
    public static class Lifecycle_WhenActiveAsync
    {
        /// <summary>
        ///     Suspends / Awaits until the Lifecycle is Active.
        ///     Ensures that the Lifecycle is on Active state.
        /// </summary>
        /// <param name="this"></param>
        /// <exception cref="OperationCanceledException">
        ///     If the Lifecycle will never enter into Active state again.
        ///     Example: If it went into Disposed state.
        /// </exception>
        public static async Task WhenActiveAsync(this Lifecycle @this)
        {
            if (@this.CurrentState == LifecycleState.Active)
                return;

            await TaskFactory.FromEvent<EventArgs>(
                    handler => @this.OnActive += handler,
                    handler => @this.OnActive -= handler
                )
                .WithCancellationIgnore();
        }
    }
}
