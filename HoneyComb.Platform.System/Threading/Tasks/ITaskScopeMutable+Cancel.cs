namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Extension for ITaskScopeMutable.Cancel
    /// </summary>
    public static class ITaskScopeMutable_Cancel
    {
        /// <summary>
        ///     Communicates a request for cancellation of this scope's tasks.
        ///     Transitions the <see cref="ITaskScope.ScopeTask"/> into 
        ///     the <see cref="System.Threading.Tasks.TaskStatus.Canceled"/> state.
        /// </summary>
        /// <param name="this"></param>
        public static void Cancel(this ITaskScopeMutable @this)
        {
            @this.CancellationTokenSource.Cancel();
            @this.ScopeTaskCompletionSource.SetCanceled();
        }

        /// <summary>
        ///     Try to communicates a request for cancellation of this scope's tasks.
        ///     Transitions the <see cref="ITaskScope.ScopeTask"/> into 
        ///     the <see cref="System.Threading.Tasks.TaskStatus.Canceled"/> state.
        ///     
        ///     If the status in a completed state, it does nothing.
        /// </summary>
        /// <param name="this"></param>
        public static void TryCancel(this ITaskScopeMutable @this)
        {
            @this.CancellationTokenSource.Cancel();
            @this.ScopeTaskCompletionSource.TrySetCanceled();
        }
    }
}
