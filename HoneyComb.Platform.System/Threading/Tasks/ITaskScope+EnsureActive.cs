using System;
using System.Linq;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Ensures that current Task is active.
    ///     
    ///     If the Task is no longer active, 
    ///     throws TaskCanceledException with no inner Exception. 
    ///     
    ///     If the Task was finished with exception, 
    ///     thrown TaskCanceledException contains the original fault exception cause.
    ///     
    ///     This method is a drop-in replacement for the following code, but with more precise exception:
    ///     if (!IsActive) 
    ///         throw new TaskCanceledException()
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/ensure-active.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/is-active.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/-job/is-active.html
    /// </remarks>
    public static class ITaskScope_EnsureActive
    {
        /// <summary>
        ///     Ensures that current Task is active.
        ///     
        ///     If the Task is no longer active, 
        ///     throws TaskCanceledException with no inner Exception. 
        ///     
        ///     If the Task was finished with exception, 
        ///     thrown TaskCanceledException contains the original fault exception cause.
        ///     
        ///     This method is a drop-in replacement for the following code, but with more precise exception:
        ///     if (!IsActive) 
        ///         throw new TaskCanceledException()
        /// </summary>
        /// <param name="this"></param>
        public static void EnsureActive(this ITaskScope @this)
        {
            if (@this.IsActive())
                return;

            if (@this.ScopeTask.IsFaulted)
                throw new TaskCanceledException(
                    "This scope task is finished with an exception (faulted). It cannot run scoped children's tasks anymore.", 
                    @this.ScopeTask.Exception.InnerExceptions.Single()
                    );

            throw new TaskCanceledException("This scope task is canceled. It cannot run scoped children's tasks anymore.");
        }
    }
}
