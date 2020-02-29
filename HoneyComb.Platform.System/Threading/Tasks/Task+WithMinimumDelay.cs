using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Extension WithMinimumDelay for Task
    /// </summary>
    /// <remarks>
    ///     Credits:
    ///         https://stackoverflow.com/questions/53181980/how-can-i-await-a-minimum-amount-of-time
    /// </remarks>
    public static class Task_WithMinimumDelay
    {
        /// <summary>
        ///     Awaits the execution of the given Task
        ///     and await the minimum delay time
        ///     if the Task duration was shorter than
        ///     the delay duration.
        ///
        ///     Only the time left will be awaited.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="minimumDelay"></param>
        /// <returns></returns>
        public static async Task<T> WithMinimumDelay<T>(this Task<T> @this, TimeSpan minimumDelay)
        {
            await (@this as Task).WithMinimumDelay(minimumDelay);
            return await @this;
        }
        
        /// <summary>
        ///     Awaits the execution of the given Task
        ///     and await the minimum delay time
        ///     if the Task duration was shorter than
        ///     the delay duration.
        ///
        ///     Only the time left will be awaited.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="minimumDelay"></param>
        /// <returns></returns>
        public static async Task WithMinimumDelay(this Task @this, TimeSpan minimumDelay)
        {
            var timer = Stopwatch.StartNew();
            await @this;
            var timeLeft = minimumDelay - timer.Elapsed;
            if (timeLeft.TotalMilliseconds <= 0)
                return;

            await Task.Delay(timeLeft);
        }
    }
}
