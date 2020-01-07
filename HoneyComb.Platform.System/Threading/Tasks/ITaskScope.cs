using System.Threading;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Defines a scope for a new task.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     https://github.com/Kotlin/kotlinx.coroutines/blob/master/kotlinx-coroutines-core/common/src/CoroutineScope.kt
    ///     https://github.com/Kotlin/kotlinx.coroutines/blob/master/kotlinx-coroutines-core/common/src/Builders.common.kt
    /// </remarks>
    public interface ITaskScope
    {
        /// <summary>
        ///     Cancellation token associated with this scope.
        ///     Will be cancelled when this scope finishes.
        /// </summary>
        CancellationToken CancellationToken { get; }
    }
}
