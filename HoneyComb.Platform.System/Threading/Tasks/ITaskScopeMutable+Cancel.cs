namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Extension for ITaskScopeMutable.Cancel
    /// </summary>
    public static class ITaskScopeMutable_Cancel
    {
        /// <summary>
        ///     Communicates a request for cancellation of this scope's tasks.
        /// </summary>
        /// <param name="this"></param>
        public static void Cancel(this ITaskScopeMutable @this) => @this.CancellationTokenSource.Cancel();
    }
}
