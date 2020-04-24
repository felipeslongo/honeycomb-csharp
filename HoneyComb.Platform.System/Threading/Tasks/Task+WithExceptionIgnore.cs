using System;
using System.Threading.Tasks;

namespace HoneyComb.Platform.System.Threading.Tasks
{
    public static class TaskWithExceptionIgnore
    {
        /// <summary>
        ///     Explicitly ignores any Exception
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task WithExceptionIgnore(this Task task) =>
            await WithExceptionIgnore<Exception>(task);

        /// <summary>
        ///     Explicitly ignores any Exception.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> WithExceptionIgnore<T>(this Task<T> task, T defaultValue = default) =>
            await WithExceptionIgnore<T, Exception>(task, defaultValue);

        /// <summary>
        ///     Explicitly ignores any Exception of a given type.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task WithExceptionIgnore<TException>(this Task task) where TException : Exception
        {
            try
            {
                await task;
            }
            catch (TException)
            {
                /*Explicitly Ignoring...#*/
            }
        }

        /// <summary>
        ///     Explicitly ignores any Exception of a given type.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> WithExceptionIgnore<T, TException>(this Task<T> task, T defaultValue = default)
            where TException : Exception
        {
            try
            {
                return await task;
            }
            catch (TException)
            {
                /*Explicitly Ignoring...#*/
                return defaultValue;
            }
        }
    }
}
