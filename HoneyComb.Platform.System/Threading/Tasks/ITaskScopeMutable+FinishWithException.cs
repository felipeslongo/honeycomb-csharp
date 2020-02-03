using System;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Extension for ITaskScopeMutable.FinishWithException
    /// </summary>
    public static class ITaskScopeMutable_FinishWithException
    {
        /// <summary>
        ///     Transitions the <see cref="ITaskScope.ScopeTask"/> into the <see cref="System.Threading.Tasks.TaskStatus.Faulted"/>
        ///     state and binds it to a specified exception.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="e"></param>
        public static void FinishWithException(this ITaskScopeMutable @this, Exception e) 
        {
            @this.CancellationTokenSource.Cancel();
            @this.ScopeTaskCompletionSource.SetException(e);
        }

        /// <summary>
        ///     Transitions the <see cref="ITaskScope.ScopeTask"/> into the <see cref="System.Threading.Tasks.TaskStatus.Faulted"/>
        ///     state and binds it to a specified exception.
        ///     
        ///     If the status in a completed state, it does nothing.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="e"></param>
        public static void TryFinishWithException(this ITaskScopeMutable @this, Exception e)
        {
            @this.CancellationTokenSource.Cancel();
            @this.ScopeTaskCompletionSource.TrySetException(e);
        }
    }
}
