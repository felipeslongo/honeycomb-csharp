using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HoneyComb.TestChamber
{
    /// <summary>
    /// Simple Mock for <see cref="SynchronizationContext"/> that captures Post and Send calls.
    /// </summary>
    public class SynchronizationContextMock : SynchronizationContext
    {
        private readonly List<SendOrPostEventArgs> _posts = new List<SendOrPostEventArgs>();
        private readonly List<SendOrPostEventArgs> _sends = new List<SendOrPostEventArgs>();

        public IReadOnlyList<SendOrPostEventArgs> Posts => _posts;
        public IReadOnlyList<SendOrPostEventArgs> Sends => _sends;
        public bool PostWasCalled => Posts.Any();

        public override void Post(SendOrPostCallback sendOrPostCallback, object state) =>
            _posts.Add(new SendOrPostEventArgs(sendOrPostCallback, state));

        public override void Send(SendOrPostCallback sendOrPostCallback, object state) =>
            _sends.Add(new SendOrPostEventArgs(sendOrPostCallback, state));

        public class SendOrPostEventArgs
        {
            public SendOrPostEventArgs(SendOrPostCallback sendOrPostCallback, object state)
            {
                SendOrPostCallback = sendOrPostCallback;
                State = state;
            }

            public SendOrPostCallback SendOrPostCallback { get; }
            public object State { get; }
        }
    }
}
