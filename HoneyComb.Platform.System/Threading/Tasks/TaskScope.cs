using System.Threading;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Basic implementation of <see cref="ITaskScope"/>.
    /// </summary>
    internal class TaskScope : ITaskScopeMutable
    {
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public CancellationToken CancellationToken => CancellationTokenSource.Token;
    }
}