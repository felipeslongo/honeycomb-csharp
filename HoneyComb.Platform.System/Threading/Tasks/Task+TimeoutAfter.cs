using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Platform.SystemH.Threading.Tasks
{
    /// <summary>
    ///     Static class for the extension method TimeoutAfter in Task.
    /// </summary>
    /// <remarks>
    ///     True authors of this feature:
    ///     https://stackoverflow.com/questions/4238345/asynchronously-wait-for-taskt-to-complete-with-timeout"
    ///     https://devblogs.microsoft.com/pfxteam/crafting-a-task-timeoutafter-method/
    /// </remarks>
    public static class TaskTimeoutAfter
    {
        /// <summary>
        ///     Obtain a copy or “proxy” of the Task that will either
        ///     (A) complete within the specified time period, or
        ///     (B) will complete with an indication that it had timed out.
        /// </summary>
        /// <param name="task">Task that will be awaited</param>
        /// <param name="timeout">Timeout that should be awaited till the <paramref name="task" /> completes</param>
        /// <typeparam name="TResult">Generic result returned by <paramref name="task" /></typeparam>
        /// <returns>
        ///     The <typeparamref name="TResult" /> or
        ///     a <exception cref="TimeoutException"></exception>
        /// </returns>
        /// <exception cref="TimeoutException">If the timeout is reached before the task completes</exception>
        /// <remarks>
        ///     The returned proxy Task can complete in one of two ways:
        ///     If task completes before the specified timeout period has elapsed,
        ///     then the proxy Task finishes when task finishes,
        ///     with task’s completion status being copied to the proxy.
        ///     If task fails to complete before the specified timeout period has elapsed,
        ///     then the proxy Task finishes when the timeout period expires,
        ///     in Faulted state with a TimeoutException.
        /// </remarks>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            await (task as Task).TimeoutAfter(timeout);
            return await task; // Very important in order to propagate exceptions

            // using var timeoutCancellationTokenSource = new CancellationTokenSource();
            // var timeoutTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);
            // var completedTask = await Task.WhenAny(task, timeoutTask);
            // if (completedTask == timeoutTask) throw new TimeoutException();
            //
            // timeoutCancellationTokenSource.Cancel();
            // return await task;  // Very important in order to propagate exceptions
        }

        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            if (ShouldShortCircuit(task, timeout)) return;


            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var timeoutTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);
            var completedTask = await Task.WhenAny(task, timeoutTask);
            if (completedTask == timeoutTask) throw new TimeoutException();

            timeoutCancellationTokenSource.Cancel();
            await task; // Very important in order to propagate exceptions
        }

        private static bool ShouldShortCircuit(Task task, TimeSpan timeout)
        {
            // Short-circuit #1: infinite timeout or task already completed
            if (task.IsCompleted || timeout == Timeout.InfiniteTimeSpan)
                // Either the task has already completed or timeout will never occur.
                // No proxy necessary.
                return true;

            // Short-circuit #2: zero timeout
            if (timeout == TimeSpan.Zero)
                throw new TimeoutException(); // We've already timed out.

            return false;
        }
    }
}
