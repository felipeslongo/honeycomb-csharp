using HoneyComb.Core.Lifecycles;
using HoneyComb.TestChamber;
using System;
using Xunit;

namespace HoneyComb.Core.Tests.Lifecycles
{
    public class LifecycleTests : ILifecycleOwner
    {
        private readonly MutableLifecycle lifecycle;

        public LifecycleTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        public Lifecycles.Lifecycle Lifecycle => lifecycle;

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

            public class SubscribeTests : LifecycleTests, ILifecycleObserver
            {
                public bool OnActiveNotified { get; private set; }
                public bool OnDisposedNotified { get; private set; }
                public bool OnInactiveNotified { get; private set; }

                [Fact, Trait(nameof(Category), Category.Unit)]
                public void GivenAnValidTransition_ShouldNotifyObservers()
                {
                    Lifecycle.Subscribe(this);

                    lifecycle.NotifyStateChange(LifecycleState.Active);


                    Assert.True(OnActiveNotified);
                }

                [Fact, Trait(nameof(Category), Category.Unit)]
                public void GivenInitializedState_WhenSubscribe_ShouldNotNotifyObservers()
                {
                    Lifecycle.Subscribe(this);

                    Assert.False(OnActiveNotified);
                }

                [Fact, Trait(nameof(Category), Category.Unit)]
                public void GivenActiveState_WhenSubscribe_ShouldNotifyObservers()
                {
                    lifecycle.NotifyStateChange(LifecycleState.Active);
                    Lifecycle.Subscribe(this);

                    Assert.True(OnActiveNotified);
                }

                [Fact, Trait(nameof(Category), Category.Unit)]
                public void GivenInactiveState_WhenSubscribe_ShouldNotifyObservers()
                {
                    lifecycle.NotifyStateChange(LifecycleState.Active);
                    lifecycle.NotifyStateChange(LifecycleState.Inactive);
                    Lifecycle.Subscribe(this);

                    Assert.True(OnInactiveNotified);
                }

                [Fact, Trait(nameof(Category), Category.Unit)]
                public void GivenDisposedtate_WhenSubscribe_ShouldNotifyObservers()
                {
                    lifecycle.NotifyStateChange(LifecycleState.Active);
                    lifecycle.NotifyStateChange(LifecycleState.Inactive);
                    lifecycle.NotifyStateChange(LifecycleState.Disposed);
                    Lifecycle.Subscribe(this);

                    Assert.True(OnInactiveNotified);
                    Assert.True(OnDisposedNotified);
                }


                private void Reset()
                {
                    OnActiveNotified = false;
                    OnDisposedNotified = false;
                    OnInactiveNotified = false;
                }

                void ILifecycleObserver.OnActive(ILifecycleOwner owner) => OnActiveNotified = true;

                void ILifecycleObserver.OnDisposed(ILifecycleOwner owner) => OnDisposedNotified = true;

                void ILifecycleObserver.OnInactive(ILifecycleOwner owner) => OnInactiveNotified = true;

            }
        }
    }
}
