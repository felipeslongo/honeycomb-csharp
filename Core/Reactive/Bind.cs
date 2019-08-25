using Core.Threading;
using System;
using System.Threading;

namespace Core.Reactive
{
    public class Bind : IDisposable
    {
        private readonly EventHandler<EventArgs> _handler;        
        private Action _unbind;
        private SynchronizationContext _context = SynchronizationContextCurrentThread.Default;

        internal Bind(EventHandler<EventArgs> handler)
        {
            _handler = handler;
        }

        public void WithCurrentSynchronizationContext() => WithSynchronizationContext(SynchronizationContext.Current ?? SynchronizationContextCurrentThread.Default);

        public void WithSynchronizationContext(SynchronizationContext context) => _context = context;

        internal void ConfigureUnbind(Action unbind) => _unbind = unbind;

        public void Dispose() => _unbind();

        internal void OnLiveDataPropertyChanged(object sender, EventArgs e) => _context.Post(_ => _handler(sender, e), null);
    }
}
