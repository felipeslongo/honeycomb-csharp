using System.Threading;

namespace Core.Threading
{
    /// <summary>
    /// <see cref="SynchronizationContext"/> implementation that is dumb and always executes synchronously in the current <see cref="Thread"/>/>.
    /// Mainly used to UnitTest porpuses.
    /// </summary>
    public class SynchronizationContextCurrentThread : SynchronizationContext
    {
        public static SynchronizationContext Default { get; } = new SynchronizationContextCurrentThread();

        public override void Post(SendOrPostCallback d, object state) => Send(d, state);
        public override void Send(SendOrPostCallback d, object state) => d(state);
    }
}
