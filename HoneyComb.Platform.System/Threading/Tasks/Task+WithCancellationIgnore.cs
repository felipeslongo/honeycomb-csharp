using System;
using System.Threading.Tasks;

namespace HoneyComb.Platform.System.Threading.Tasks
{
    public static class TaskWithCancellationIgnore
    {
        /// <summary>
        ///     Explicitly ignores <see cref="OperationCanceledException" />
        ///     thrown by <see cref="CancellationToken" />.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task WithCancellationIgnore(this Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
            }
        }

        /// <summary>
        ///     Explicitly ignores <see cref="OperationCanceledException" />
        ///     thrown by <see cref="CancellationToken" />.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> WithCancellationIgnore<T>(this Task<T> task, T defaultValue = default)
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
                return defaultValue;
            }
        }
    }
}
