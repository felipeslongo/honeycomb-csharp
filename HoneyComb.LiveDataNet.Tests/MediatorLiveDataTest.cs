using HoneyComb.TestChamber;
using System;
using Xunit;

namespace HoneyComb.LiveDataNet.Tests
{
    public class MediatorLiveDataTest
    {
        public class AddSourceTests_Action : MediatorLiveDataTest
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenADisposedOfSubscription_WhenDisposedOfAgain_ShouldDoNothing()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                var subscription = mediator.AddSource(source, _ => { });
                subscription.Dispose();

                subscription.Dispose();
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceChanges_ShouldInvokeOnSourceChanged()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                var isOnSourceChangedInvoked = false;
                mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);

                source.Value = int.MaxValue;

                Assert.True(isOnSourceChangedInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsAddedAgainWithADifferentOnSourceChanged_ShouldThrowException()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                Action<int> handlerOne = _ => { };
                Action<int> handlerTwo = _ => { };
                mediator.AddSource(source, handlerOne);

                Assert.Throws<InvalidOperationException>(() => mediator.AddSource(source, handlerTwo));
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsAddedAgainWithTheSameOnSourceChanged_ShouldInvokeOnSourceChangedJustOnce()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                var onSourceChangeCallCount = 0;
                Action<int> sameHandler = _ => onSourceChangeCallCount++;
                mediator.AddSource(source, sameHandler);
                mediator.AddSource(source, sameHandler);

                source.Value = int.MaxValue;

                Assert.Equal(1, onSourceChangeCallCount);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSubscriptionIsDisposed_ShouldStopListeningToTheUnsubscribedSource()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                var isOnSourceChangedInvoked = false;
                var subscription = mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);
                subscription.Dispose();

                source.Value = int.MaxValue;

                Assert.False(isOnSourceChangedInvoked);
            }
        }

        public class AddSourceTests_Func : MediatorLiveDataTest
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceChanges_ShouldInvokeSetValueUsingConverter()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<bool>();
                mediator.AddSource(source, Convert.ToBoolean);

                source.Value = 1;

                Assert.True(mediator.Value);
            }
        }

        public class RemoveSourceTests : MediatorLiveDataTest
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorAndANotObservedSource_WhenSourceIsRemoved_ShouldDoNothing()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();

                mediator.RemoveSource(source);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsRemoved_ShouldStopListeningToTheUnsubscribedSource()
            {
                var source = new MutableLiveData<int>();
                var mediator = new MediatorLiveData<int>();
                var isOnSourceChangedInvoked = false;
                mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);
                mediator.RemoveSource(source);

                source.Value = int.MaxValue;

                Assert.False(isOnSourceChangedInvoked);
            }
        }
    }
}
