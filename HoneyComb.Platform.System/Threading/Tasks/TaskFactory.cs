using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Provides convenient factory methods to produce Task from common patterns.
    /// </summary>
    public static class TaskFactory
    {
        /// <summary>
        ///     Produce an awaitable Task from a C# Event Pattern.
        ///     It will complete and unsubscribe itself when the subscribed event is invoked.
        ///
        ///     WARNING: Use it without CancellationToken when you are sure that the subscribed
        ///     event will be invoked, otherwise this task will hang and be subscribed forever.
        ///     For a safer approach consider passing a CancellationToken.
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="subscribe"></param>
        /// <param name="unsubscribe"></param>
        /// <param name="token">
        ///
        /// </param>
        /// <returns></returns>
        public static Task<TEventArgs> FromEvent<TEventArgs>(
            Action<EventHandler<TEventArgs>> subscribe,
            Action<EventHandler<TEventArgs>> unsubscribe,
            CancellationToken? token = null
            )
        {
            var taskSource = new TaskCompletionSource<TEventArgs>();
            EventHandler<TEventArgs> eventHandler = (_, __) => { };
            eventHandler = (sender, args) =>
            {
                unsubscribe(eventHandler);
                taskSource.SetResult(args);
            };

            token?.Register(() =>
            {
                unsubscribe(eventHandler);
                taskSource.SetCanceled();
            });

            subscribe(eventHandler);
            return taskSource.Task;
        }
    }
}
