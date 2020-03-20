using System;
using System.Threading;
using HoneyComb.Core.Threading;

namespace HoneyComb.Core.Lifecycles.Threading
{
    // ReSharper disable once InconsistentNaming
    public static class Lifecycle_GetDisposedCancellationToken
    {
        /// <summary>
        ///     Gets an CancellationToken that will request cancellation
        ///     when this Lifecycle is on Disposed state.
        /// </summary>
        /// <param name="this"></param>
        public static CancellationToken GetDisposedCancellationToken(this Lifecycle @this)
        {
            if (@this.CurrentState == LifecycleState.Disposed)
                return new CancellationToken(true);

            var token = CancellationTokenFactory.FromEvent<EventArgs>(
                handler => @this.OnDisposed += handler,
                handler => @this.OnDisposed -= handler
            );

            return token;
        }
    }
}
