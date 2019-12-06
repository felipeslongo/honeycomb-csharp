using System;
using System.Threading.Tasks;
using HoneyComb.Platform.System.Threading.Tasks;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Platform.System.Tests.Threading.Tasks
{
    public class TaskWithCancellationIgnoreTests
    {
        private readonly Task _unfinishedTask = Task.Delay(TimeSpan.FromMilliseconds(10));

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTask_WhenTaskFails_ShouldThrownTaskException()
        {
            async Task TaskThatWillFail()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new ApplicationException();
            }

            await Assert.ThrowsAsync<ApplicationException>(() => TaskThatWillFail().WithCancellationIgnore());
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTask_WhenTaskIsCancelled_ShouldReturn()
        {
            async Task TaskThatWillBeCancelled()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new TaskCanceledException();
            }

            await TaskThatWillBeCancelled().WithCancellationIgnore();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTask_WhenTaskIsFinished_ShouldReturn()
        {
            await _unfinishedTask.WithCancellationIgnore();
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTaskThatReturnAnValue_WhenTaskIsCancelled_ShouldReturnFactoryAsyncValue()
        {
            async Task<int> UnfinishedValueTask()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new TaskCanceledException();
            }


            var actual = await UnfinishedValueTask().WithCancellationIgnore(() => Task.FromResult(int.MaxValue));

            Assert.Equal(int.MaxValue, actual);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTaskThatReturnAnValue_WhenTaskIsCancelled_ShouldReturnFactoryValue()
        {
            async Task<int> UnfinishedValueTask()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new TaskCanceledException();
            }


            var actual = await UnfinishedValueTask().WithCancellationIgnore(() => int.MaxValue);

            Assert.Equal(int.MaxValue, actual);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTaskThatReturnAnValueObject_WhenTaskIsCancelled_ShouldReturnNull()
        {
            async Task<object> UnfinishedValueTask()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new TaskCanceledException();
            }

            var actual = await UnfinishedValueTask().WithCancellationIgnoreNullClass();

            Assert.Null(actual);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTaskThatReturnAnValueStruct_WhenTaskIsCancelled_ShouldReturnNull()
        {
            async Task<int> UnfinishedValueTask()
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                throw new TaskCanceledException();
            }

            var actual = await UnfinishedValueTask().WithCancellationIgnoreNullStruct();

            Assert.Null(actual);
        }
    }
}
