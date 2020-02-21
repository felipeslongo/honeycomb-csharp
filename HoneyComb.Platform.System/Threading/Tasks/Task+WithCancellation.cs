using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Platform.System.Threading.Tasks
{
    /// <summary>
    ///     Static class for WithCancellation extension methods
    /// </summary>
    /// <remarks>
    ///     True authors of this feature:
    ///     https://devblogs.microsoft.com/pfxteam/how-do-i-cancel-non-cancelable-async-operations/
    ///     https://stackoverflow.com/questions/10134310/how-to-cancel-a-task-in-await
    ///     https://stackoverflow.com/questions/4783865/how-do-i-abort-cancel-tpl-tasks?noredirect=1&lq=1
    /// </remarks>
    public static class TaskWithCancellation
    {
        /// <summary>
        ///     So, can you cancel non-cancelable operations? No.
        ///     Can you cancel waits on non-cancelable operations?  Sure… just be very careful when you do.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            await WithCancellation(task as Task, cancellationToken);
            return await task; // Very important in order to propagate exceptions
        }

        /// <summary>
        ///     So, can you cancel non-cancelable operations? No.
        ///     Can you cancel waits on non-cancelable operations?  Sure… just be very careful when you do.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(SetResultOfTaskSourceObjectInCancellationTokenCallback, tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            }

            await task; // Very important in order to propagate exceptions
        }

        private static void SetResultOfTaskSourceObjectInCancellationTokenCallback(object? obj)
        {
            var tcs = (TaskCompletionSource<bool>) obj!;
            tcs.TrySetResult(true);
        }
    }
}
