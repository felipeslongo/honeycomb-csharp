using HoneyComb.Platform.System.Threading.Tasks;
using System;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Represents a TaskCompletionSource that can be Recycled
    ///     to produce new Tasks instances.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public sealed class TaskCompletionSourceRecycler<TResult>
    {
        private TaskCompletionSource<TResult> taskSource = new TaskCompletionSource<TResult>();

        public Task<TResult> Task => taskSource.Task;

        public void Recycle(TResult result)
        {
            taskSource.TrySetResult(result);
            taskSource = new TaskCompletionSource<TResult>();
        }      

        public void Recycle(Exception exception)
        {
            taskSource.TrySetException(exception);
            taskSource = new TaskCompletionSource<TResult>();
        }

        public async Task<TResult> RecycleAsync(TResult result)
        {
            var previouslyTask = taskSource.Task;
            Recycle(result);
            return await previouslyTask;
        }

        public async Task<TResult> RecycleAsync(Exception exception)
        {
            var previouslyTask = taskSource.Task;
            Recycle(exception);
            return await previouslyTask.WithExceptionIgnore();
        }
    }
}
