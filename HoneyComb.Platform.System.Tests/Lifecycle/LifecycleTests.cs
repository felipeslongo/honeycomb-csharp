using HoneyComb.Platform.System.Lifecycle;
using HoneyComb.TestChamber;
using System;
using Xunit;

namespace HoneyComb.Platform.System.Tests.Lifecycle
{
    public class LifecycleTests : ILifecycleOwner
    {
        private readonly MutableLifecycle lifecycle;

        public LifecycleTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        public System.Lifecycle.Lifecycle Lifecycle => lifecycle;

        public class NotifyStateChangeTests : LifecycleTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnValidTransition_ShouldSucceed()
            {
                var expected = LifecycleState.Active;

                lifecycle.NotifyStateChange(expected);

                Assert.Equal(expected, lifecycle.CurrentState);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnValidTransition_ShouldNotifyObservers()
            {
                Assert.Raises<EventArgs>(
                    testHandler => Lifecycle.OnActive += testHandler,
                    testHandler => Lifecycle.OnActive -= testHandler,
                    () => lifecycle.NotifyStateChange(LifecycleState.Active));
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnyState_WhenChangedToSameState_ShouldNotNotifyObservers()
            {
                var isNotified = false;
                lifecycle.NotifyStateChange(LifecycleState.Active);
                Lifecycle.OnActive += (_, __) => isNotified = true;

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.False(isNotified);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnInvalidTransition_ShouldFail()
            {
                var invalid = LifecycleState.Disposed;

                Assert.Throws<InvalidOperationException>(() => lifecycle.NotifyStateChange(invalid));
            }
        }
    }
}
