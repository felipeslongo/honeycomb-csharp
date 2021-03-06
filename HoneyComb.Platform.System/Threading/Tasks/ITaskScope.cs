using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Defines a scope for a new task.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     https://medium.com/@elizarov/structured-concurrency-722d765aa952
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/-coroutine-scope/
    ///     https://github.com/Kotlin/kotlinx.coroutines/blob/master/kotlinx-coroutines-core/common/src/CoroutineScope.kt
    ///     https://github.com/Kotlin/kotlinx.coroutines/blob/master/kotlinx-coroutines-core/common/src/Builders.common.kt
    ///
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/coroutine-scope.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/supervisor-scope.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/-main-scope.html
    /// </remarks>
    public interface ITaskScope
    {
        /// <summary>
        ///     Cancellation token associated with this scope.
        ///     Will be cancelled when this scope finishes.
        ///     It will never be revived/relaunched once its finished.
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        ///     Defines a task for this scope.
        ///     Allows this scope to be awaited to finish with success,
        ///     or for Exceptions or Cancellations.
        /// </summary>
        Task ScopeTask { get; }
    }
}
