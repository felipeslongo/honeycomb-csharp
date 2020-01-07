using System;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    public static class EventHandler_AsTask
    {
        public static Task AsTask<TEventArgs>(this EventHandler<TEventArgs> @this)
        {
            var taskSource = new TaskCompletionSource<TEventArgs>();
            EventHandler<TEventArgs> eventHandler = (_, __) => {};
            eventHandler = (sender, args) =>
            {
                @this -= eventHandler;
                taskSource.SetResult(args);
            };

            @this += eventHandler;

            return taskSource.Task;
        }
    }
}
