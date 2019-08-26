using Core.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreTests.Assertions
{
    public static class SynchronizationContextAssert
    {
        public static async Task ShouldBeExecutedInCurrentContextAsynchronously(Action<Action> testMethod)
        {
            try
            {
                var context = new SynchronizationContextThreadPool();
                SynchronizationContext.SetSynchronizationContext(context);
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var synchronizationContextTheadId = currentThreadId;

                testMethod(() => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId);
                await context.LastTask;

                Assert.NotEqual(currentThreadId, synchronizationContextTheadId);
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
            }
        }

        public static void ShouldBeExecutedInCurrentThreadSynchronously(Action<Action> testMethod)
        {
            SynchronizationContext.SetSynchronizationContext(null);
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            var synchronizationContextTheadId = -1;

            testMethod(() => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId);

            Assert.Equal(currentThreadId, synchronizationContextTheadId);
        }
    }
}
