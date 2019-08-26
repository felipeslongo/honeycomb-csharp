using Core.Reactive;
using Core.Threading;
using CoreTests.Assertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreTests.Reactive
{
    public class BindTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public int Property { get; set; }

        public class WithSynchronizationContextTests : BindTests
        {
            [Fact]
            public async Task GivenASynchronizationContext_ShouldBeExecutedInThatContext_WhenPropertyIsChanged()
            {                
                await SynchronizationContextAssert.ShouldBeExecutedInCurrentContextAsynchronously(assertion =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(assertion).WithSynchronizationContext(SynchronizationContext.Current);

                    liveData.Value = DifferentValue;
                });
            }

            [Fact]
            public void GivenAEmptySynchronizationContext_ShouldBeExecutedInTheCurrentThread_WhenPropertyIsChanged()
            {
                SynchronizationContextAssert.ShouldBeExecutedInCurrentThreadSynchronously(assertion =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindMethod(assertion);

                    liveData.Value = DifferentValue;
                });
            }
        }

        public class WithCurrentSynchronizationContextTests : LiveDataTests, IDisposable
        {
            [Fact]
            public async Task GivenACurrentSynchronizationContext_ShouldBeExecutedInThatContext_WhenPropertyIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var context = new SynchronizationContextThreadPool();
                SynchronizationContext.SetSynchronizationContext(context);
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var synchronizationContextTheadId = currentThreadId;
                liveData.BindMethod(() => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId)
                    .WithCurrentSynchronizationContext();

                liveData.Value = DifferentValue;
                await context.LastTask;

                Assert.NotEqual(currentThreadId, synchronizationContextTheadId);
            }

            [Fact]
            public void GivenAEmptyCurrentSynchronizationContext_ShouldBeExecutedInTheCurrentThread_WhenPropertyIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                SynchronizationContext.SetSynchronizationContext(null);
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var synchronizationContextTheadId = -1;
                liveData.BindMethod(() => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId);

                liveData.Value = DifferentValue;

                Assert.Equal(currentThreadId, synchronizationContextTheadId);
            }

            public void Dispose() => SynchronizationContext.SetSynchronizationContext(null);
        }
    }
}
