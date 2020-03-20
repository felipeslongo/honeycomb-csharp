using HoneyComb.Core.Lifecycles;
using HoneyComb.Core.Lifecycles.Threading;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Lifecycles.Threading
{
    // ReSharper disable once InconsistentNaming
    public class Lifecycle_GetDisposedCancellationTokenTests : ILifecycleOwner
    {
        public Lifecycle_GetDisposedCancellationTokenTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        private readonly MutableLifecycle lifecycle;

        public Lifecycle Lifecycle => lifecycle;

        [Theory]
        [Trait(nameof(Category), Category.Unit)]
        [InlineData(LifecycleState.Initialized)]
        [InlineData(LifecycleState.Active)]
        [InlineData(LifecycleState.Inactive)]
        public void GivenLifecycleStatesDifferentThanDisposed_WhenInvoked_ShouldNotRequestCancellation(
            LifecycleState state)
        {
            lifecycle.NotifyStateChange(state);

            var token = lifecycle.GetDisposedCancellationToken();

            Assert.False(token.IsCancellationRequested);
        }

        [Theory]
        [Trait(nameof(Category), Category.Unit)]
        [InlineData(LifecycleState.Initialized)]
        [InlineData(LifecycleState.Active)]
        [InlineData(LifecycleState.Inactive)]
        public void GivenLifecycleStateChangeDifferentThanDisposed_WhenStateIsChanged_ShouldNotRequestCancellation(
            LifecycleState state)
        {
            var token = lifecycle.GetDisposedCancellationToken();

            lifecycle.NotifyStateChange(state);

            Assert.False(token.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateChangeToDisposed_WhenStateIsChanged_ShouldRequestCancellation()
        {
            var token = lifecycle.GetDisposedCancellationToken();

            lifecycle.NotifyStateChange(LifecycleState.Disposed);

            Assert.True(token.IsCancellationRequested);
        }

        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public void GivenLifecycleStateDisposed_WhenInvoked_ShouldRequestCancellation()
        {
            lifecycle.NotifyStateChange(LifecycleState.Disposed);

            var token = lifecycle.GetDisposedCancellationToken();

            Assert.True(token.IsCancellationRequested);
        }
    }
}
