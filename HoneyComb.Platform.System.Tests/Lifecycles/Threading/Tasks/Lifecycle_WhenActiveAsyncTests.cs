using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading.Tasks;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Lifecycles.Threading.Tasks
{
    // ReSharper disable once InconsistentNaming
    public class Lifecycle_WhenActiveAsyncTests : ILifecycleOwner
    {
        public Lifecycle_WhenActiveAsyncTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        private readonly MutableLifecycle lifecycle;

        public Lifecycle Lifecycle => lifecycle;

        [Theory]
        [Trait(nameof(Category), Category.Unit)]
        [InlineData(LifecycleState.Initialized)]
        [InlineData(LifecycleState.Inactive)]
        [InlineData(LifecycleState.Disposed)]
        public void GivenLifecycleStatesDifferentThanActive_WhenAwaited_ShouldNotCompleteSucessfully(
            LifecycleState state)
        {
            lifecycle.NotifyStateChange(state);

            var awaitableTask = lifecycle.WhenActiveAsync();

            Assert.False(awaitableTask.IsCompletedSuccessfully);
        }

        [Theory]
        [Trait(nameof(Category), Category.Unit)]
        [InlineData(LifecycleState.Initialized)]
        [InlineData(LifecycleState.Inactive)]
        [InlineData(LifecycleState.Disposed)]
        public void GivenLifecycleStateChangeDifferentThanActive_WhenStateIsChanged_ShouldNotCompleteSucessfully(
            LifecycleState state)
        {
            var awaitableTask = lifecycle.WhenActiveAsync();

            lifecycle.NotifyStateChange(state);

            Assert.False(awaitableTask.IsCompletedSuccessfully);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateActive_WhenAwaited_ShouldCompleteSucessfully()
        {
            lifecycle.NotifyStateChange(LifecycleState.Active);

            var awaitableTask = lifecycle.WhenActiveAsync();

            Assert.True(awaitableTask.IsCompletedSuccessfully);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateChangeToActive_WhenStateIsChanged_ShouldCompleteSucessfully()
        {
            var awaitableTask = lifecycle.WhenActiveAsync();

            lifecycle.NotifyStateChange(LifecycleState.Active);

            Assert.True(awaitableTask.IsCompletedSuccessfully);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateChangeToDisposed_WhenStateIsChanged_ShouldCancel()
        {
            var awaitableTask = lifecycle.WhenActiveAsync();

            lifecycle.NotifyStateChange(LifecycleState.Disposed);

            Assert.True(awaitableTask.IsCanceled);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateDisposed_WhenAwaited_ShouldCancel()
        {
            lifecycle.NotifyStateChange(LifecycleState.Disposed);

            var awaitableTask = lifecycle.WhenActiveAsync();

            Assert.True(awaitableTask.IsCanceled);
        }
    }
}
