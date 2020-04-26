using System.Threading;

namespace HoneyComb.Core.Threading
{
    /// <summary>
    ///     Factory for SemaphoreSlim usecases
    /// </summary>
    public static class SemaphoreSlimFactory
    {
        /// <summary>
        ///     Creates an instance that never executes more than one Task at a time.
        /// </summary>
        /// <param name="isBusy">If should be considered busy at start</param>
        /// <returns>SemaphoreSlim instance</returns>
        public static SemaphoreSlim CreateMutexLock(bool isBusy = false)
        {
            var initialCount = isBusy ? 0 : 1;
            return new SemaphoreSlim(initialCount, 1);
        }
    }
}
