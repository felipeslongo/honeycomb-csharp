using System.Threading;
using System.Threading.Tasks;

namespace Core.Threading
{
    /// <summary>
    /// <see cref="SynchronizationContext"/> implementation that delegates execution to a ThreadPool Thread using <see cref="Task.Run(System.Action)"/>.
    /// Mainly used to UnitTest porpuses.
    /// </summary>
    public class SynchronizationContextThreadPool : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            Task.Run(() => d(state));
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Task.Run(() => d(state)).Wait();
        }
    }
}
