using System;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Basic implementation of <see cref="ITaskScope"/>.
    /// </summary>
    internal class TaskScope : ITaskScopeMutable
    {
        public CancellationToken CancellationToken => CancellationTokenSource.Token;
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        public Task ScopeTask => ScopeTaskCompletionSource.Task;
        public TaskCompletionSource<EventArgs> ScopeTaskCompletionSource { get; } = new TaskCompletionSource<EventArgs>();
    }
}