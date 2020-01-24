using HoneyComb.Core.Lifecycles;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Lifecycles
{
    public class LifecycleAdvancedTests : ILifecycleOwner
    {
        private readonly MutableLifecycle lifecycle;

        public LifecycleAdvancedTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        public Lifecycle Lifecycle => lifecycle;

        public class OnActiveAgainTests : LifecycleAdvancedTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenActive_ShouldNotInvoke()
            {
                var isInvoked = false;
                lifecycle.Advanced.OnActiveAgain += (_, __) => isInvoked = true;

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.False(isInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenActiveAgain_ShouldInvoke()
            {
                var isInvoked = false;
                lifecycle.Advanced.OnActiveAgain += (_, __) => isInvoked = true;
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.True(isInvoked);
            }
        }

        public class OnInactiveAgainTests : LifecycleAdvancedTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenInactive_ShouldNotInvoke()
            {
                var isInvoked = false;
                lifecycle.Advanced.OnInactiveAgain += (_, __) => isInvoked = true;
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);

                Assert.False(isInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenALifecycle_WhenInactiveAgain_ShouldInvoke()
            {
                var isInvoked = false;
                lifecycle.Advanced.OnInactiveAgain += (_, __) => isInvoked = true;
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);
                lifecycle.NotifyStateChange(LifecycleState.Active);

                lifecycle.NotifyStateChange(LifecycleState.Inactive);

                Assert.True(isInvoked);
            }
        }
    }
}
