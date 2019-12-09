using System;
using System.Threading;
using System.Threading.Tasks;
using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Platform.System.Tests.Threading.Tasks
{
    public class TaskTimeoutAfterTests
    {
        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnCompletedTask_ShouldReturnImmediatelly()
        {
            var completedTask = Task.CompletedTask;
            var timeout = TimeSpan.FromSeconds(1);

            await completedTask.TimeoutAfter(timeout);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnFasterThanTimeoutTask_WhenTaskIsFinished_ShouldReturn()
        {
            var shortRunningTask = Task.Delay(TimeSpan.FromMilliseconds(10));
            var timeout = TimeSpan.FromMilliseconds(20);

            await shortRunningTask.TimeoutAfter(timeout);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnInfiniteTimeout_ShouldWaitForeverTheTaskReturn()
        {
            var task = Task.Delay(TimeSpan.FromMilliseconds(10));
            var infiniteTimeout = Timeout.InfiniteTimeSpan;

            await task.TimeoutAfter(infiniteTimeout);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnSlowerThanTimeoutTask_WhenTimeoutIsReached_ShouldThrowAException()
        {
            var longRunningTask = Task.Delay(TimeSpan.FromMilliseconds(20));
            var timeout = TimeSpan.FromMilliseconds(10);

            async Task TestCode()
            {
                await longRunningTask.TimeoutAfter(timeout);
            }

            await Assert.ThrowsAsync<TimeoutException>(TestCode);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnValueReturningTask_WhenTaskIsFinished_ShouldReturnTheValue()
        {
            var valueExpected = new object();
            var valueTask = Task.Run(() => valueExpected);
            var timeout = TimeSpan.FromMilliseconds(20);

            var actual = await valueTask.TimeoutAfter(timeout);

            Assert.Equal(valueExpected, actual);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnZeroTimeout_ShouldThrowAExceptionImmediately()
        {
            var task = Task.Delay(TimeSpan.FromMilliseconds(10));
            var zeroTimeout = TimeSpan.Zero;

            await Assert.ThrowsAsync<TimeoutException>(() => task.TimeoutAfter(zeroTimeout));
        }
    }
}
