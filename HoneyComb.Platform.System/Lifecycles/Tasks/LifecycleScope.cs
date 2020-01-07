using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyComb.Core.Lifecycles.Tasks
{
    /// <summary>
    ///     TaskScope tied to an a Lifecycle and a Main/UI SynchronizationContext.
    ///     This scope will be cancelled when the is destroyed/disposed.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///         Source Code: https://android.googlesource.com/platform/frameworks/support/+/refs/heads/androidx-activity-release/lifecycle/lifecycle-runtime-ktx/src/main/java/androidx/lifecycle
    ///         https://developer.android.com/topic/libraries/architecture/coroutines#lifecyclescope
    /// </remarks>
    public sealed class LifecycleScope
    {
    }
}
