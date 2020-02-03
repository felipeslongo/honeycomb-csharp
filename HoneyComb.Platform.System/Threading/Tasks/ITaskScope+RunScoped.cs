using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Extension for ITaskScope.RunScoped
    /// </summary>
    public static class ITaskScope_RunScoped
    {
        /// <summary>
        ///     Queues the specified work to run on the thread pool and returns a System.Threading.Tasks.Task
        ///     object that represents that work. A cancellation token allows the work to be
        ///     cancelled.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="cancellableAction">The cancellable work to execute asynchronously</param>
        /// <returns>A task that represents the work queued to execute in the thread pool.</returns>
        public static async Task RunScoped(this ITaskScope scope, Action<CancellationToken> cancellableAction) =>
            await scope.RunScoped(token =>
            {
                cancellableAction(token);
                return Task.FromResult(EventArgs.Empty);
            });

        /// <summary>
        ///     Queues the specified work to run on the thread pool and returns a proxy for the
        ///     Task(TResult) returned by function.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the proxy task.</typeparam>
        /// <param name="scope"></param>
        /// <param name="cancellableFunc">The cancellable work to execute asynchronously</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by function.</returns>
        public static async Task<T> RunScoped<T>(this ITaskScope scope, Func<CancellationToken, T> cancellableFunc) =>
            await scope.RunScoped(token =>
            {
                var result = cancellableFunc(token);
                return Task.FromResult(result);
            });

        /// <summary>
        ///     Queues the specified work to run on the thread pool and returns a System.Threading.Tasks.Task
        ///     object that represents that work. A cancellation token allows the work to be
        ///     cancelled.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="cancellableAsyncAction">The cancellable work to execute asynchronously</param>
        /// <returns>A task that represents the work queued to execute in the thread pool.</returns>
        public static async Task RunScoped(this ITaskScope scope, Func<CancellationToken, Task> cancellableAsyncAction) =>
            await scope.RunScoped(async token => 
            {
                await cancellableAsyncAction(token);
                return Task.FromResult(EventArgs.Empty);
            });
            

        /// <summary>
        ///     Queues the specified work to run on the thread pool and returns a proxy for the
        ///     Task(TResult) returned by function.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the proxy task.</typeparam>
        /// <param name="scope"></param>
        /// <param name="cancellableAsyncFunc">The cancellable work to execute asynchronously</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by function.</returns>
        public static async Task<T> RunScoped<T>(this ITaskScope scope, Func<CancellationToken, Task<T>> cancellableAsyncFunc)
        {
            scope.EnsureActive();

            try
            {
                return await scope.Run(cancellableAsyncFunc);
            }
            catch (OperationCanceledException) 
            {
                if (scope is ITaskScopeMutable mutableScope)
                    mutableScope.TryCancel();
                throw;
            }
            catch(Exception e)
            {
                if (scope is ITaskScopeMutable mutableScope)
                    mutableScope.TryFinishWithException(e);
                throw;
            }
        }
    }
}
