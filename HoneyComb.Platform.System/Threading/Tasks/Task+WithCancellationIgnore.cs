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

        public static async Task<T> WithCancellationIgnore<T>(this Task<T> task, Func<T> onCancellationValueFactory)
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
                return onCancellationValueFactory();
            }
        }

        public static async Task<T> WithCancellationIgnore<T>(this Task<T> task,
            Func<Task<T>> onCancellationValueFactoryAsync)
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
                return await onCancellationValueFactoryAsync();
            }
        }

        /// <summary>
        ///     Explicitly ignores <see cref="OperationCanceledException" />
        ///     thrown by <see cref="CancellationToken" />.
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T?> WithCancellationIgnoreNullClass<T>(this Task<T> task) where T : class
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
                return null;
            }
        }

        /// <summary>
        ///     Explicitly ignores <see cref="OperationCanceledException" />
        ///     thrown by <see cref="CancellationToken" />.
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T?> WithCancellationIgnoreNullStruct<T>(this Task<T> task) where T : struct
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
                /*Explicitly Ignoring Cancellation...#*/
                return null;
            }
        }
    }
}
