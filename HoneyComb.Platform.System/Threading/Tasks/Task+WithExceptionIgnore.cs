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
        public static async Task WithExceptionIgnore(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                /*Explicitly Ignoring...#*/
            }
        }

        /// <summary>
        ///     Explicitly ignores any Exception.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> WithExceptionIgnore<T>(this Task<T> task, T defaultValue = default)
        {
            try
            {
                return await task;
            }
            catch
            {
                /*Explicitly Ignoring...#*/
                return defaultValue;
            }
        }
    }
}
