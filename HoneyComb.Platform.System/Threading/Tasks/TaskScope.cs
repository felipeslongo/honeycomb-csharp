using System.Threading;

namespace HoneyComb.Core.Threading.Tasks
{
    /// <summary>
    /// Basic implementation of <see cref="ITaskScope"/>.
    /// </summary>
    internal class TaskScope : ITaskScope
    {
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public CancellationToken CancellationToken => tokenSource.Token;
    }
}