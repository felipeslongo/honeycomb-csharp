using HoneyComb.Core.Lifecycles;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.LiveDataNet.Tests
{
    public class LiveEventTests : ILifecycleOwner
    {
        private readonly MutableLifecycle lifecycle;

        public LiveEventTests()
        {
            lifecycle = new MutableLifecycle(this);
        }

        public Lifecycle Lifecycle => lifecycle;

        public class EventChangedTests : LiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenALiveEvent_WhenInvoked_ShouldNotifySubscriber()
            {
                var liveEvent = new MutableLiveEvent<bool>();
                var isNotified = false;
                liveEvent.EventChanged += (_, eventValue) => isNotified = eventValue.PeekContent;

                liveEvent.Invoke(true);

                Assert.True(isNotified);
            }
        }

        public class InvokeTests : LiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnHandledLiveEvent_WhenIsInvoked_ShouldNotNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                var liveEvent = new MutableLiveEvent<int>();
                liveEvent.SubscribeToExecuteIfUnhandled(this, _ => { });
                var isNotified = false;
                liveEvent.SubscribeToExecuteIfUnhandled(this, eventArgs => isNotified = true);

                liveEvent.Invoke(int.MaxValue);

                Assert.False(isNotified);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnSubscribedLiveEvent_WhenIsInvoked_ShouldNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                const int invokedValue = int.MaxValue;
                var notifiedValue = default(int);
                var liveEvent = new MutableLiveEvent<int>();
                liveEvent.SubscribeToExecuteIfUnhandled(this, eventArgs => notifiedValue = eventArgs);

                liveEvent.Invoke(invokedValue);

                Assert.Equal(invokedValue, notifiedValue);
            }
        }

        public class SubscribeTests : LiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnInitializedLiveEvent_WhenIsInvoked_ShouldNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                const int inicializationValue = int.MaxValue;
                var liveEvent = new MutableLiveEvent<int>(inicializationValue);
                var isNotifyInvoked = false;

                liveEvent.Subscribe(this, _ => isNotifyInvoked = true);

                Assert.True(isNotifyInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUninitializedLiveData_WhenIsInvoked_ShouldNotNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                var liveEvent = new MutableLiveEvent<int>();
                var isNotifyInvoked = false;

                liveEvent.Subscribe(this, _ => isNotifyInvoked = true);

                Assert.False(isNotifyInvoked);
            }
        }

        public class SubscribeToExecuteIfUnhandledTests : LiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnInitializedLiveEvent_WhenIsInvoked_ShouldNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                const int inicializationValue = int.MaxValue;
                var liveEvent = new MutableLiveEvent<int>(inicializationValue);
                var isNotifyInvoked = false;

                liveEvent.SubscribeToExecuteIfUnhandled(this, _ => isNotifyInvoked = true);

                Assert.True(isNotifyInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAnUninitializedLiveData_WhenIsInvoked_ShouldNotNotifySubscriber()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                var liveEvent = new MutableLiveEvent<int>();
                var isNotifyInvoked = false;

                liveEvent.SubscribeToExecuteIfUnhandled(this, _ => isNotifyInvoked = true);

                Assert.False(isNotifyInvoked);
            }
        }
    }
}
