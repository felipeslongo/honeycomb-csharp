using Core.Threading;
using System;
using System.Threading;

namespace Core.Reactive
{
    [Obsolete("Unstable due to high memory pressure",true)]
    public class Bind : IDisposable
    {
        private readonly EventHandler<EventArgs> _handler;
        private Action _unbind;
        private SynchronizationContext _context;

        internal Bind(EventHandler<EventArgs> handler)
        {
            _handler = handler;
        }

        public void WithCurrentSynchronizationContext() => WithSynchronizationContext(SynchronizationContext.Current);

        public void WithSynchronizationContext(SynchronizationContext context) => _context = context;

        internal void ConfigureUnbind(Action unbind) => _unbind = unbind;

        public void Dispose() => _unbind();

        internal void OnLiveDataPropertyChanged(object sender, EventArgs e)
        {
            if (_context == null)
            {
                _handler(sender, e);
                return;
            }

            _context.Post(_ => _handler(sender, e), null);
        }
    }
}
