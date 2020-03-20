using HoneyComb.Core.Lifecycles;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Lifecycles
{
    public class LifecycleOfLifecyclesTests : ILifecycleOwner
    {
        private readonly LifecycleOfLifecycles lifecycle;

        public LifecycleOfLifecyclesTests()
        {
            lifecycle = new LifecycleOfLifecycles(this);
        }

        public Lifecycle Lifecycle => lifecycle;

        public class DisposeTests : LifecycleOfLifecyclesTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenDisposed_ShouldBeInDisposedState()
            {
                lifecycle.Dispose();

                Assert.Equal(LifecycleState.Disposed, lifecycle.CurrentState);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenDisposed_ShouldNotifyObservers()
            {
                var observerNotified = false;
                lifecycle.OnDisposed += (_, __) => observerNotified = true;

                lifecycle.Dispose();

                Assert.True(observerNotified);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenDisposed_ShouldReleaseLifecycleReference()
            {
                var lifecycleOwner = new MutableLifecycleOwner();
                lifecycle.SetLifecycle(lifecycleOwner.Lifecycle);

                lifecycle.Dispose();

                Assert.Null(lifecycle.GetCurrentLifecycle());
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenDisposed_ShouldUnsubscribeFromLifecycleEvents()
            {
                var mutableLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                lifecycle.SetLifecycle(mutableLifecycle);
                var anyEventWasRaised = false;
                lifecycle.OnActive += (_, __) => anyEventWasRaised = true;
                lifecycle.OnInactive += (_, __) => anyEventWasRaised = true;

                lifecycle.Dispose();
                mutableLifecycle.NotifyStateChange(LifecycleState.Active);
                mutableLifecycle.NotifyStateChange(LifecycleState.Inactive);

                Assert.False(anyEventWasRaised);
            }
        }

        public class SetLifecycleTests : LifecycleOfLifecyclesTests
        {
            [Theory]
            [Trait(nameof(Category), Category.Unit)]
            [InlineData(LifecycleState.Active)]
            [InlineData(LifecycleState.Inactive)]
            public void GivenALifecycle_WhenSet_ShouldSynchronizeStateAndNotifyObservers(LifecycleState state)
            {
                var mutableLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                mutableLifecycle.NotifyStateChange(state);
                var observerNotified = false;
                lifecycle.OnActive += (_, __) => observerNotified = true;
                lifecycle.OnInactive += (_, __) => observerNotified = true;

                lifecycle.SetLifecycle(mutableLifecycle);

                Assert.Equal(state, lifecycle.CurrentState);
                Assert.True(observerNotified);
            }

            [Theory]
            [Trait(nameof(Category), Category.Unit)]
            [InlineData(LifecycleState.Active)]
            [InlineData(LifecycleState.Inactive)]
            public void GivenASetLifecycle_WhenSetLifecycleStateChange_ShouldSynchronizeStateAndNotifyObserver(
                LifecycleState state)
            {
                var mutableLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                lifecycle.SetLifecycle(mutableLifecycle);
                var observerNotified = false;
                lifecycle.OnActive += (_, __) => observerNotified = true;
                lifecycle.OnInactive += (_, __) => observerNotified = true;

                mutableLifecycle.NotifyStateChange(state);

                Assert.Equal(state, lifecycle.CurrentState);
                Assert.True(observerNotified);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenSet_ShouldSynchronizeStateToInactiveAndNotifyObservers()
            {
                var mutableLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                mutableLifecycle.NotifyStateChange(LifecycleState.Disposed);
                var observerNotified = false;
                lifecycle.OnInactive += (_, __) => observerNotified = true;

                lifecycle.SetLifecycle(mutableLifecycle);

                Assert.Equal(LifecycleState.Inactive, lifecycle.CurrentState);
                Assert.True(observerNotified);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenASetLifecycle_WhenNewLifecycleIsSet_ShouldUnsubscribeFromTheOldLifecycleEvents()
            {
                var oldLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                lifecycle.SetLifecycle(oldLifecycle);
                var anyEventFromTheOldLifecycleWasRaised = false;
                lifecycle.OnActive += (_, __) => anyEventFromTheOldLifecycleWasRaised = true;
                lifecycle.OnInactive += (_, __) => anyEventFromTheOldLifecycleWasRaised = true;

                lifecycle.SetLifecycle(new MutableLifecycleOwner().Lifecycle);
                oldLifecycle.NotifyStateChange(LifecycleState.Active);
                oldLifecycle.NotifyStateChange(LifecycleState.Inactive);

                Assert.False(anyEventFromTheOldLifecycleWasRaised);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void
                GivenASetLifecycle_WhenSetLifecycleStateChangeToDisposed_ShouldSynchronizeStateToInactiveAndNotifyObservers()
            {
                var mutableLifecycle = (MutableLifecycle) new MutableLifecycleOwner().Lifecycle;
                lifecycle.SetLifecycle(mutableLifecycle);
                var observerNotified = false;
                lifecycle.OnInactive += (_, __) => observerNotified = true;

                mutableLifecycle.NotifyStateChange(LifecycleState.Disposed);

                Assert.Equal(LifecycleState.Inactive, lifecycle.CurrentState);
                Assert.True(observerNotified);
            }
        }
    }
}
