using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading
{
    public static class SemaphoreSlim_Extensions
    {
        /// <summary>
        ///     Gets a boolean that indicates if the semaphore is currently Locked/Blocked
        ///     for new threads to enter.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsLocked(this SemaphoreSlim @this) => @this.CurrentCount == 0;

        /// <summary>
        ///     Gets a boolean that indicates if the semaphore is currently Unlocked/Unblocked/Available
        ///     for new threads to enter.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsUnlocked(this SemaphoreSlim @this) => @this.CurrentCount >= 1;

        /// <summary>
        ///     Asynchronously waits to enter the System.Threading.SemaphoreSlim.
        /// </summary>
        /// <param name="this"></param>
        /// <returns>
        ///     A task that will complete when the semaphore has been entered,
        ///     with a IDisposable that will release the semaphore when disposed.
        /// </returns>
        public static async Task<IDisposable> WaitDisposableAsync(this SemaphoreSlim @this)
        {
            await @this.WaitAsync();
            return Disposable.Create(() => @this.Release());
        }
    }
}
