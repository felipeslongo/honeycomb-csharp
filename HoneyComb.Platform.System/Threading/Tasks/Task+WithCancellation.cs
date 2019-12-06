using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Platform.System.Threading.Tasks
{
    /// <summary>
    ///     So, can you cancel non-cancelable operations? No.
    ///     Can you cancel waits on non-cancelable operations?  Sure… just be very careful when you do.
    /// </summary>
    /// <remarks>
    ///     True authors of this feature:
    ///     https://devblogs.microsoft.com/pfxteam/how-do-i-cancel-non-cancelable-async-operations/
    ///     https://stackoverflow.com/questions/10134310/how-to-cancel-a-task-in-await
    ///     https://stackoverflow.com/questions/4783865/how-do-i-abort-cancel-tpl-tasks?noredirect=1&lq=1
    /// </remarks>
    public static class TaskWithCancellation
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            await using (cancellationToken.Register(SetResultOfTaskSourceObjectInCancellationTokenCallback, tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                    throw new OperationCanceledException(cancellationToken);
            }

            return await task;
        }

        private static void SetResultOfTaskSourceObjectInCancellationTokenCallback(object? obj)
        {
            var tcs = (TaskCompletionSource<bool>) obj;
            tcs.TrySetResult(true);
        }
    }
}
