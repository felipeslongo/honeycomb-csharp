using System;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    ///     Returns true when the current Task is still active 
    ///     (has not completed and was not cancelled yet).
    ///     
    ///     Returns true when this Task is active – it was already started and has not completed 
    ///     nor was cancelled yet. The Task that is waiting for its children to complete is still 
    ///     considered to be active if it was not cancelled nor failed.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/ensure-active.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/is-active.html
    ///     https://kotlin.github.io/kotlinx.coroutines/kotlinx-coroutines-core/kotlinx.coroutines/-job/is-active.html
    /// </remarks>
    public static class ITaskScope_IsActive
    {
        /// <summary>
        ///     Returns true when the current Task is still active 
        ///     (has not completed and was not cancelled yet).
        ///     
        ///     Returns true when this Task is active – it was already started and has not completed 
        ///     nor was cancelled yet. The Task that is waiting for its children to complete is still 
        ///     considered to be active if it was not cancelled nor failed.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsActive(this ITaskScope @this) =>
            !@this.ScopeTask.IsCompleted;
    }
}
