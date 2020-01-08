using HoneyComb.TestChamber;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using TaskFactory = HoneyComb.Core.Threading.Tasks.TaskFactory;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class TaskFactoryTests
    {
        public class FromEventTests : TaskFactoryTests
        {
            public event EventHandler<int>? TestEvent;

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnEvent_WhenTestMethodInvoked_ShouldReturnAnAwaitableUnfinishedTask()
            {
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe);

                Assert.False(task.IsCompleted);
                Assert.Equal(TaskStatus.WaitingForActivation, task.Status);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnEvent_WhenTestMethodInvoked_ShouldTaskSubscribeToIt()
            {
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe);

                Assert.NotNull(TestEvent);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTask_WhenEventIsInvoked_ShouldComplete()
            {
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe);

                TestEvent!.Invoke(this, int.MaxValue);

                Assert.True(task.IsCompletedSuccessfully);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTask_WhenEventIsInvoked_ShouldUnsubscribe()
            {
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe);

                TestEvent!.Invoke(this, int.MaxValue);

                Assert.Null(TestEvent);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTaskWithCancellationToken_WhenTokenIsCancelled_ShouldMutateTaskToCancelledState()
            {
                var tokenSource = new CancellationTokenSource();
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe, tokenSource.Token);

                tokenSource.Cancel();

                Assert.True(task.IsCanceled);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedTaskWithCancellationToken_WhenTokenIsCancelled_ShouldUnsubscribe()
            {
                var tokenSource = new CancellationTokenSource();
                var task = TaskFactory.FromEvent<int>(subscribe => TestEvent += subscribe, unsubscribe => TestEvent -= unsubscribe, tokenSource.Token);

                tokenSource.Cancel();

                Assert.Null(TestEvent);
            }
        }
    }
}
