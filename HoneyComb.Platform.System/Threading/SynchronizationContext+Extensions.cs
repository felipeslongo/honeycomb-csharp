using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading
{
    /// <summary>
    /// Extensions for <see cref="SynchronizationContext"/>
    /// </summary>
    public static class SynchronizationContextExtensions
    {
        /// <summary>
        /// Experimental, use at your own discretion
        /// </summary>
        /// <param name="this"></param>
        /// <param name="executeAsync"></param>
        /// <returns></returns>
        [Experimental]
        public static async Task SendAsync(this SynchronizationContext @this, Func<Task> executeAsync)
        {
            await @this.SendAsync(async delegate
            {
                await executeAsync();
                return true;
            });
        }
        
        /// <summary>
        /// Experimental, use at your own discretion
        /// </summary>
        /// <param name="this"></param>
        /// <param name="executeAsync"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Experimental]
        public static async Task<T> SendAsync<T>(this SynchronizationContext @this, Func<Task<T>> executeAsync)
        {
            var taskSource = new TaskCompletionSource<T>();
            @this.Post(async delegate
            {
                try
                {
                    var result = await executeAsync();
                    taskSource.SetResult(result);    
                }
                catch (Exception e){taskSource.SetException(e);}
            }, null);
            return await taskSource.Task;
        }

        [Experimental]
        internal static async Task SendAsync(this SynchronizationContext @this, SendOrPostCallback sendOrPostCallback, object? state)
        {
            var taskSource = new TaskCompletionSource<bool>();
            @this.Post(delegate
            {
                try
                {
                    sendOrPostCallback(state);
                    taskSource.SetResult(true);    
                }
                catch (Exception e){taskSource.SetException(e);}
            }, null);
            await taskSource.Task;
        }
    }
}
