using System;
using System.Threading;
using System.Threading.Tasks;
using HoneyComb.Platform.System.Threading.Tasks;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Platform.System.Tests.Threading.Tasks
{
    public class TaskWithCancellationTests
    {
        public TaskWithCancellationTests()
        {
            var tokenSource = new CancellationTokenSource();
            _tokenCancelled = tokenSource.Token;
            tokenSource.Cancel();
        }

        private readonly Task _finishedTask = Task.CompletedTask;
        private readonly Task _unfinishedTask = Task.Delay(TimeSpan.FromSeconds(10));
        private readonly CancellationToken _token = CancellationToken.None;
        private readonly CancellationToken _tokenCancelled;

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnFinishedTask_AndAnCancelledToken_ShouldReturnImmediatelly()
        {
            await _finishedTask.WithCancellation(_tokenCancelled);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnFinishedTask_ShouldReturnImmediatelly()
        {
            await _finishedTask.WithCancellation(_token);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTask_AndAnCancelledToken_ShouldThrowException()
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _unfinishedTask.WithCancellation(_tokenCancelled));
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedTask_WhenTaskIsFinished_ShouldReturn()
        {
            await _unfinishedTask.WithCancellation(_token);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnUnfinishedValueTask_WhenTaskIsFinished_ShouldReturnValue()
        {
            var valueTask = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                return int.MaxValue;
            });

            var actual = await valueTask.WithCancellation(_token);

            Assert.Equal(int.MaxValue, actual);
        }
    }
}
