using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Defines a TaskScope that can be manually cancelled.
    ///     It is mutable.
    /// </summary>
    public interface ITaskScopeMutable : ITaskScope
    {
        /// <summary>
        ///     Cancellation token source associated with this scope.
        ///     Will be cancelled when this scope finishes.
        ///     It will never be revived/relaunched once its finished.
        /// </summary>
        CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        ///     Defines a task for this scope.
        ///     Allows this scope to be awaited to finish with success,
        ///     or for Exceptions or Cancellations.
        /// </summary>
        TaskCompletionSource<EventArgs> ScopeTaskCompletionSource { get; }
    }
}
