using HoneyComb.Core;
using HoneyComb.LiveDataNet;
using HoneyComb.TestChamber;
using System;
using System.Collections.Generic;
using Xunit;

namespace HoneyComb.LiveEventNet.Tests
{
    public class MediatorLiveEventTests
    {
        public class AddSourcesTests : MediatorLiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithMultipleSources_WhenOneSourceIsDisposed_ShouldStopListeningToThatSourceChanges()
            {
                var source1 = new MutableLiveEvent<int>();
                var source2 = new MutableLiveEvent<int>();
                var source3 = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var subscriptions = mediator.AddSources(source1, source2, source3);
                var mediatorDispatchedValues = new List<int>();
                _ = mediator.SubscribeToExecuteIfUnhandled(mediatorDispatchedValues.Add);

                subscriptions[1].Dispose();
                source1.Invoke(1);
                source2.Invoke(2);
                source3.Invoke(3);

                Assert.Equal(new[] { 1, 3 }, mediatorDispatchedValues);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithMultipleSources_WhenSourcesChanges_ShouldSetValueUsingEachSourceValue()
            {
                var source1 = new MutableLiveEvent<int>();
                var source2 = new MutableLiveEvent<int>();
                var source3 = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                _ = mediator.AddSources(source1, source2, source3);
                var mediatorDispatchedValues = new List<int>();
                _ = mediator.SubscribeToExecuteIfUnhandled(mediatorDispatchedValues.Add);

                source1.Invoke(1);
                source2.Invoke(2);
                source3.Invoke(3);

                Assert.Equal(new[] { 1, 2, 3 }, mediatorDispatchedValues);
            }
        }

        public class AddSourceTests : MediatorLiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceChanges_ShouldSetValueUsingSourceValue()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                _ = mediator.AddSource(source);
                var mediatorEventArgs = 0;
                mediator.SubscribeToExecuteIfUnhandled(eventArgs => mediatorEventArgs = eventArgs);

                source.Invoke(int.MaxValue);

                Assert.Equal(int.MaxValue, mediatorEventArgs);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithMultipleSources_WhenSourcesChanges_ShouldSetValueUsingEachSourceValue()
            {
                var source1 = new MutableLiveEvent<int>();
                var source2 = new MutableLiveEvent<int>();
                var source3 = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                _ = mediator.AddSource(source1);
                _ = mediator.AddSource(source2);
                _ = mediator.AddSource(source3);
                var mediatorDispatchedValues = new List<int>();
                mediator.SubscribeToExecuteIfUnhandled(mediatorDispatchedValues.Add);

                source1.Invoke(1);
                source2.Invoke(2);
                source3.Invoke(3);

                Assert.Equal(new[] { 1, 2, 3 }, mediatorDispatchedValues);
            }
        }

        public class AddSourceTests_Action : MediatorLiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenADisposedOfSubscription_WhenDisposedOfAgain_ShouldDoNothing()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var subscription = mediator.AddSource(source, _ => { });
                subscription.Dispose();

                subscription.Dispose();
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceChanges_ShouldInvokeOnSourceChanged()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var isOnSourceChangedInvoked = false;
                mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);

                source.Invoke(int.MaxValue);

                Assert.True(isOnSourceChangedInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsAddedAgainWithADifferentOnSourceChanged_ShouldThrowException()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                Action<Event<int>> handlerOne = _ => { };
                Action<Event<int>> handlerTwo = _ => { };
                mediator.AddSource(source, handlerOne);

                Assert.Throws<InvalidOperationException>(() => mediator.AddSource(source, handlerTwo));
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsAddedAgainWithTheSameOnSourceChanged_ShouldInvokeOnSourceChangedJustOnce()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var onSourceChangeCallCount = 0;
                Action<Event<int>> sameHandler = _ => onSourceChangeCallCount++;
                mediator.AddSource(source, sameHandler);
                mediator.AddSource(source, sameHandler);

                source.Invoke(int.MaxValue);

                Assert.Equal(1, onSourceChangeCallCount);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSubscriptionIsDisposed_ShouldStopListeningToTheUnsubscribedSource()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var isOnSourceChangedInvoked = false;
                var subscription = mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);
                subscription.Dispose();

                source.Invoke(int.MaxValue);

                Assert.False(isOnSourceChangedInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenASourceWithoutValueSet_WhenSourceIsAdded_ShouldNotInvokeOnSourceChanged()
            {
                var sourceWithoutValue = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var isOnSourceChangedInvoked = false;

                mediator.AddSource(sourceWithoutValue, _ => isOnSourceChangedInvoked = true);

                Assert.False(isOnSourceChangedInvoked);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenASourceWithValueSet_WhenSourceIsAdded_ShouldInvokeOnSourceChanged()
            {
                var sourceWithoutValue = new MutableLiveEvent<int>();
                sourceWithoutValue.Invoke(int.MaxValue);
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var isOnSourceChangedInvoked = false;

                mediator.AddSource(sourceWithoutValue, _ => isOnSourceChangedInvoked = true);

                Assert.True(isOnSourceChangedInvoked);
            }
        }

        public class AddSourceTests_Func : MediatorLiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceChanges_ShouldSetValueConvertingSourceValue()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<bool>.CreateMediatorLiveEvent();
                mediator.AddSource(source, sourceEvent => new Event<bool>(Convert.ToBoolean(sourceEvent.PeekContent)));
                var mediatorEventArgs = false;
                mediator.SubscribeToExecuteIfUnhandled(eventArgs => mediatorEventArgs = eventArgs);

                source.Invoke(1);

                Assert.True(mediatorEventArgs);
            }
        }

        public class RemoveSourceTests : MediatorLiveEventTests
        {
            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorAndANotObservedSource_WhenSourceIsRemoved_ShouldDoNothing()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();

                mediator.RemoveSource(source);
            }

            [Fact, Trait(nameof(Category), Category.Unit)]
            public void GivenAMediatorWithASource_WhenSourceIsRemoved_ShouldStopListeningToTheUnsubscribedSource()
            {
                var source = new MutableLiveEvent<int>();
                var mediator = MediatorLiveData<int>.CreateMediatorLiveEvent();
                var isOnSourceChangedInvoked = false;
                mediator.AddSource(source, _ => isOnSourceChangedInvoked = true);
                mediator.RemoveSource(source);

                source.Invoke(int.MaxValue);

                Assert.False(isOnSourceChangedInvoked);
            }
        }
    }
}
