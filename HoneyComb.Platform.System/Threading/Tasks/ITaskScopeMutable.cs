using System.Threading;

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
    }
}
