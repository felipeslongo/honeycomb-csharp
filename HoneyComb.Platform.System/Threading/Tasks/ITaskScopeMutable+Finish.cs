using System;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Extension for ITaskScopeMutable.Finish
    /// </summary>
    public static class ITaskScopeMutable_Finish
    {
        /// <summary>
        ///     Transitions the <see cref="ITaskScope.ScopeTask"/> into the <see cref="System.Threading.Tasks.TaskStatus.RanToCompletion"/>
        ///     state and binds it to a specified void result.
        /// </summary>
        /// <param name="this"></param>
        public static void Finish(this ITaskScopeMutable @this) 
        {
            @this.CancellationTokenSource.Cancel();
            @this.ScopeTaskCompletionSource.SetResult(EventArgs.Empty); 
        }
    }
}
