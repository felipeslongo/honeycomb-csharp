using Core.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreTests.Assertions
{    
    public static class SynchronizationContextAssert
    {        
        public static async Task ShouldBeExecutedInCurrentContextAsynchronously(Action<ContextVisitor> testMethod)
        {
            ContextVisitor contextVisitor = null;
            try
            {
                contextVisitor = ContextVisitor.MakeWithCurrentContext();

                testMethod(contextVisitor);
                await contextVisitor.SynchronizationContextTask;

                Assert.True(contextVisitor.WasExecutedInContext);
            }
            finally
            {
                contextVisitor.Dispose();
            }
        }

        public static void ShouldBeExecutedInCurrentThreadSynchronously(Action<ContextVisitor> testMethod)
        {
            ContextVisitor contextVisitor = null;
            try
            {
                contextVisitor = ContextVisitor.MakeWithoutCurrentContext();

                testMethod(contextVisitor);

                Assert.True(contextVisitor.WasExecutedInCurrentThread);
            }
            finally
            {
                contextVisitor.Dispose();
            }
        }

        public class ContextVisitor : IDisposable
        {
            private readonly int _currentThreadId = Thread.CurrentThread.ManagedThreadId;
            private readonly SynchronizationContext _oldContext = SynchronizationContext.Current;
            private SynchronizationContextThreadPool _context;

            private ContextVisitor(SynchronizationContextThreadPool context)
            {
                _context = context;
                SynchronizationContext.SetSynchronizationContext(context);
            }

            public SynchronizationContext CurrentContext => _context;

            public Task SynchronizationContextTask => _context?.LastTask;

            public bool WasExecutedInContext { get; private set; } = false;
            public bool WasExecutedInCurrentThread { get; private set; } = false;

            public void CaptureContext()
            {
                var synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId;
                WasExecutedInContext = _currentThreadId != synchronizationContextTheadId;
                WasExecutedInCurrentThread = _currentThreadId == synchronizationContextTheadId;
            }

            public void Dispose() => SynchronizationContext.SetSynchronizationContext(_oldContext);

            public static ContextVisitor MakeWithCurrentContext() => new ContextVisitor(new SynchronizationContextThreadPool());
            public static ContextVisitor MakeWithoutCurrentContext() => new ContextVisitor(null);
        }
    }
}
