using System;
using System.Threading;
using System.Threading.Tasks;
using HoneyComb.Core.Threading.Tasks;

namespace HoneyComb.Core.Lifecycles.Tasks
{
    /// <summary>
    ///     TaskScope tied to an a Lifecycle and a Main/UI SynchronizationContext.
    ///     This scope will be cancelled when the Lifecycle is destroyed/disposed.
    /// </summary>
    /// <remarks>
    ///     Credits
    ///     Source Code:
    ///     https://android.googlesource.com/platform/frameworks/support/+/refs/heads/androidx-activity-release/lifecycle/lifecycle-runtime-ktx/src/main/java/androidx/lifecycle
    ///     https://developer.android.com/topic/libraries/architecture/coroutines#lifecyclescope
    /// </remarks>
    public sealed class LifecycleScope : ITaskScope
    {
        private readonly Lifecycle lifecycle;
        private readonly CancellationTokenSource tokenSouce = new CancellationTokenSource();

        public LifecycleScope(Lifecycle lifecycle)
        {
            this.lifecycle = lifecycle;
            Init();
        }

        public CancellationToken CancellationToken => tokenSouce.Token;

        public Task ScopeTask => throw new NotImplementedException();

        public async Task Launch(Func<CancellationToken, Task> block)
        {
            if (tokenSouce.IsCancellationRequested)
                return;

            try
            {
                await block(CancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void Init()
        {
            if (lifecycle.CurrentState == LifecycleState.Disposed)
            {
                tokenSouce.Cancel();
                return;
            }

            lifecycle.OnDisposed += LifecycleOnOnDisposed;
        }

        private void LifecycleOnOnDisposed(object sender, EventArgs e)
        {
            tokenSouce.Cancel();
            lifecycle.OnDisposed -= LifecycleOnOnDisposed;
        }
    }
}
