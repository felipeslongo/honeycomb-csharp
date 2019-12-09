using HoneyComb.Core.Lifecycle;
using HoneyComb.TestChamber;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.LiveDataNet.Tests
{
    public class LiveDataTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public class BindPropertyTests : LiveDataTests
        {
            private int _field = 0;
            private int Property { get; set; }
            private int PropertyWithNoSetter => Property;

            private int Method() => 0;

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                Property = DifferentValue;

                liveData.BindProperty(this, target => target.Property);

                Assert.Equal(SameValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenInitialValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindProperty(this, target => target.Property);

                liveData.Value = DifferentValue;

                Assert.Equal(DifferentValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenADisposedBind_ShouldNotSetAnValueIntoTheProperty_WhenValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var bindDisposer = liveData.BindProperty(this, target => target.Property);
                bindDisposer.Dispose();

                liveData.Value = DifferentValue;

                Assert.Equal(SameValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPropertyWithNoSetter_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target.PropertyWithNoSetter));

                Assert.Contains(LiveData<int>.MessagePropertyHasNoSetter, exception.Message);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenANonPropertyMember_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target._field));

                Assert.Contains(LiveData<int>.MessageExpressionLambdaDoesNotReturnAProperty, exception.Message);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAMemberMethod_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target.Method()));

                Assert.Contains(LiveData<int>.MessageExpressionLambdaDoesNotReturnAProperty, exception.Message);
            }
        }

        public class BindFieldTests : LiveDataTests
        {
            private int _field;

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheField_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                _field = DifferentValue;

                liveData.BindField(this, target => target._field);

                Assert.Equal(SameValue, _field);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheField_WhenInitialValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindField(this, target => target._field);

                liveData.Value = DifferentValue;

                Assert.Equal(DifferentValue, _field);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenADisposedBind_ShouldNotSetAnValueIntoTheField_WhenValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var bindDisposer = liveData.BindField(this, target => target._field);
                bindDisposer.Dispose();

                liveData.Value = DifferentValue;

                Assert.Equal(SameValue, _field);
            }
        }

        public class BindTests : LiveDataTests
        {
            public int field;
            public int Property { get; set; }
            public int PropertyWithNoSetter => Property;

            private int Method() => 0;

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                Property = DifferentValue;

                liveData.Bind(() => Property);

                Assert.Equal(SameValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheField_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                Property = DifferentValue;

                liveData.Bind(() => field);

                Assert.Equal(SameValue, field);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenInitialValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                liveData.Value = DifferentValue;

                Assert.Equal(DifferentValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheField_WhenInitialValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => field);

                liveData.Value = DifferentValue;

                Assert.Equal(DifferentValue, field);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenADisposedBind_ShouldNotSetAnValueIntoTheProperty_WhenValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var bindDisposer = liveData.Bind(() => Property);
                bindDisposer.Dispose();

                liveData.Value = DifferentValue;

                Assert.Equal(SameValue, Property);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAPropertyWithNoSetter_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("memberLambda", () => liveData.Bind(() => PropertyWithNoSetter));

                Assert.Contains(LiveData<int>.MessagePropertyHasNoSetter, exception.Message);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAMemberMethod_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("memberLambda", () => liveData.Bind(() => Method()));

                Assert.Contains(LiveData<int>.MessageExpressionLambdaDoesNotReturnAProperty, exception.Message);
            }
        }

        public class BindMutableLiveData : LiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooBindedToBar_ShouldFooHaveTheSameValueAsBar_WhenBarValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                bar.Bind(foo);

                bar.Value = DifferentValue;

                Assert.Equal(bar.Value, foo.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenFooAndBarBindBeDisposed_ShouldFooValueBeUnchanged_WhenBarValueChanges()
            {
                var foo = new MutableLiveData<int>(SameValue);
                var bar = new MutableLiveData<int>(SameValue);
                var bind = bar.Bind(foo);
                bind.Dispose();

                bar.Value = DifferentValue;

                Assert.Equal(SameValue, foo.Value);
            }
        }

        public class PropertyChangedTests : LiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldNotRaisePropertyChanged_WhenValueSetIsEquals()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var eventWasRaised = false;
                liveData.PropertyChanged += (sender, args) => eventWasRaised = true;

                liveData.Value = SameValue;

                Assert.False(eventWasRaised);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAInitialValue_ShouldRaisePropertyChanged_WhenValueSetIsNotEquals()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                Assert.Raises<EventArgs>(
                    handler => liveData.PropertyChanged += handler,
                    handler => liveData.PropertyChanged -= handler,
                    () => liveData.Value = DifferentValue
                );
            }
        }

        public class PostValueTests : LiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenASetSynchronizationContext_ShouldBeExecutedInThatContextAsyncronously_WhenPostIsCalled()
            {
                var contextMock = new SynchronizationContextMock();
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.SynchronizationContext = contextMock;
                liveData.BindMethod(() => { });

                liveData.PostValue(DifferentValue);

                Assert.True(contextMock.PostWasCalled);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenANullSynchronizationContext_ShouldBeExecutedInCurrentThreadSyncronously_WhenPostIsCalled()
            {
                var expectedThread = Thread.CurrentThread.ManagedThreadId;
                var actualThread = -1;
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindMethod(() => actualThread = Thread.CurrentThread.ManagedThreadId);

                liveData.PostValue(DifferentValue);

                Assert.Equal(expectedThread, actualThread);
            }
        }

        public class SubscribeTests : LiveDataTests, IObserver<EventArgs>
        {
            private List<EventArgs> _onNextInvokedArgs = new List<EventArgs>();

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenASubscribedObserver_ShouldNotifyPropertyChanges_WhenValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Subscribe(this);

                liveData.Value = DifferentValue;

                Assert.Single(_onNextInvokedArgs);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAUnsubscribedObserver_ShouldNotNotifyPropertyChanges_WhenValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Subscribe(this).Dispose();

                liveData.Value = DifferentValue;

                Assert.Empty(_onNextInvokedArgs);
            }

            void IObserver<EventArgs>.OnCompleted()
            {
                throw new NotImplementedException();
            }

            void IObserver<EventArgs>.OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            void IObserver<EventArgs>.OnNext(EventArgs value) => _onNextInvokedArgs.Add(value);
        }

        public class DisposeTests : LiveDataTests
        {
            public int Property { get; set; }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnBind_ShouldBeUnbinded_WhenDispose()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                liveData.Dispose();
                liveData.Value = DifferentValue;

                Assert.Equal(SameValue, Property);
            }
        }

        public class PerformanceTests : LiveDataTests
        {
            public int Property { get; set; }

            [Fact]
            [Trait(nameof(Category), Category.Performance)]
            public void GivenOneMillionIterations_ShouldBeAtMost20TimesSlowerThanANormalPropertySet_WhenBindIsUsed()
            {
                const int iterations = 1000000;
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                var propertyTimer = new Stopwatch();
                propertyTimer.Start();
                foreach (var value in Enumerable.Range(1, iterations))
                    Property = value;
                propertyTimer.Stop();

                var bindTimer = new Stopwatch();
                bindTimer.Start();
                foreach (var value in Enumerable.Range(1, iterations))
                    liveData.Value = value;
                bindTimer.Stop();

                Assert.True(bindTimer.ElapsedMilliseconds < propertyTimer.ElapsedMilliseconds * 20,
                    $"{bindTimer.ElapsedMilliseconds} vs {propertyTimer.ElapsedMilliseconds}*20");
            }

            [Fact]
            [Trait(nameof(Category), Category.Performance)]
            public void GivenOneMillionIterations_ShouldBeExecutedInLessThan100Miliseconds_WhenBindIsUsed()
            {
                const int iterations = 1000000;
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                var timer = new Stopwatch();
                timer.Start();
                foreach (var value in Enumerable.Range(1, iterations))
                    liveData.Value = value;
                timer.Stop();

                var threshold = TimeSpan.FromMilliseconds(150).TotalMilliseconds;
                var actual = timer.ElapsedMilliseconds;
                Assert.True(actual < threshold, $"Should be executed in less than {threshold} miliseconds, but was {actual} miliseconds.");
            }
        }

        public class LifecycleSupportTests : LiveDataTests, ILifecycleOwner
        {
            private readonly MutableLifecycle lifecycle;

            public LifecycleSupportTests()
            {
                lifecycle = new MutableLifecycle(this);
            }

            public Lifecycle Lifecycle => lifecycle;
            public bool IsNotified { get; set; }

            [Fact,Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenInitializedState_WhenValueChanges_ShouldNotNotifyLifecycleBoundObservers()
            {
                var liveData = new MutableLiveData<bool>();
                liveData.Bind(this, () => IsNotified);

                liveData.Value = true;

                Assert.False(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenInitializedState_WhenStateChangesToActive_ShouldNotifyLifecycleBoundObservers()
            {
                var liveData = new MutableLiveData<bool>();
                liveData.Bind(this, () => IsNotified);
                liveData.Value = true;

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.True(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenActiveState_WhenValueChanges_ShouldNotifyLifecycleBoundObservers()
            {
                var liveData = new MutableLiveData<bool>();
                lifecycle.NotifyStateChange(LifecycleState.Active);
                liveData.Bind(this, () => IsNotified);                

                liveData.Value = true;

                Assert.True(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenActiveState_WhenStateChangesToInactive_ShouldNotNotifyLifecycleBoundObservers()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                var liveData = new MutableLiveData<bool>();                
                liveData.Bind(this, () => IsNotified);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);

                liveData.Value = true;

                Assert.False(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenActiveStateDisposedSubscription_WhenValueChanges_ShouldNotNotifyLifecycleBoundObservers()
            {
                var liveData = new MutableLiveData<bool>();
                lifecycle.NotifyStateChange(LifecycleState.Active);
                var subscription = liveData.Bind(this, () => IsNotified);
                subscription.Dispose();

                liveData.Value = true;

                Assert.False(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenInactiveState_WhenValueChanges_ShouldNotNotifyLifecycleBoundObservers()
            {                
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);
                var liveData = new MutableLiveData<bool>();
                liveData.Bind(this, () => IsNotified);                

                liveData.Value = true;

                Assert.False(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenInactiveState_WhenStateChangesToActive_ShouldNotifyLifecycleBoundObservers()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);
                var liveData = new MutableLiveData<bool>();
                liveData.Bind(this, () => IsNotified);
                liveData.Value = true;

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.True(IsNotified);
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenInactiveState_WhenStateChangesToActive_ShouldNotifyLifecycleBoundObserversWithOnlyTheMostRecentValue()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);
                var valuesDispatched = new List<int>();
                var liveData = new MutableLiveData<int>();
                liveData.BindMethod(this, valuesDispatched.Add);
                liveData.Value = 1;
                liveData.Value = 2;
                liveData.Value = 3;

                lifecycle.NotifyStateChange(LifecycleState.Active);

                Assert.Single(valuesDispatched);
                Assert.Equal(liveData.Value, valuesDispatched.Single());
            }

            [Fact, Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenDisposedState_WhenValueChanges_ShouldNotNotifyLifecycleBoundObservers()
            {
                lifecycle.NotifyStateChange(LifecycleState.Active);
                lifecycle.NotifyStateChange(LifecycleState.Inactive);
                lifecycle.NotifyStateChange(LifecycleState.Disposed);
                var liveData = new MutableLiveData<bool>();
                liveData.Bind(this, () => IsNotified);

                liveData.Value = true;

                Assert.False(IsNotified);
            }
        }

        public class GCTests : LiveDataTests
        {
            public int Property { get; set; }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnUnreferencedLiveData_ShouldBeGarbageCollected_WhenGCIsCalled()
            {
                WeakReference reference = null!;
                new Action(() =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    // Do things with service that might cause a memory leak...
                    liveData.Bind(() => Property);
                    liveData.Value = DifferentValue;

                    reference = new WeakReference(liveData, true);
                })();

                // Service should have gone out of scope about now, 
                // so the garbage collector can clean it up
                GC.Collect();
                GC.WaitForPendingFinalizers();

                Assert.Null(reference.Target);
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnReferencedLiveData_ShouldBeGen0_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                liveData.Value = DifferentValue;

                var gen = GC.GetGeneration(liveData);
                Assert.Equal(0, gen);
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnReferencedLiveData_ShouldAllocateLessThan200Bytes_WhenConstructorIsCalled()
            {
                var memoryBegin = GC.GetAllocatedBytesForCurrentThread();

                _ = new MutableLiveData<int>(SameValue);

                var memoryEnd = GC.GetAllocatedBytesForCurrentThread();
                var memory = memoryEnd - memoryBegin;
                var expected = 200;
                Assert.True(memory < expected, $"Should allocate less than {expected} bytes, but was allocated {memory} bytes.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnReferencedLiveData_ShouldAllocateLessThan20000Bytes_WhenBindIsCalled()
            {
                var memoryBegin = GC.GetAllocatedBytesForCurrentThread();
                var liveData = new MutableLiveData<int>(SameValue);

                liveData.Bind(() => Property);

                var memoryEnd = GC.GetAllocatedBytesForCurrentThread();
                var memory = memoryEnd - memoryBegin;
                var expected = 20000;
                Assert.True(memory < expected, $"Should allocate less than {expected} bytes, but was allocated {memory} bytes.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnReferencedLiveData_ShouldAllocateLessThan20000Bytes_WhenPropertyIsSet()
            {
                var memoryBegin = GC.GetAllocatedBytesForCurrentThread();
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                liveData.Value = DifferentValue;

                var memoryEnd = GC.GetAllocatedBytesForCurrentThread();
                var memory = memoryEnd - memoryBegin;
                var expected = 20000;
                Assert.True(memory < expected, $"Should allocate less than {expected} bytes, but was allocated {memory} bytes.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenOneMillionIterations_ShouldAllocateLessThan20000Bytes_WhenPropertyIsSet()
            {
                const int iterations = 1000000;
                var memoryBegin = GC.GetAllocatedBytesForCurrentThread();
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.Bind(() => Property);

                foreach (var value in Enumerable.Range(1, iterations))
                    liveData.Value = value;

                var memoryEnd = GC.GetAllocatedBytesForCurrentThread();
                var memory = memoryEnd - memoryBegin;
                var expected = 20000;
                Assert.True(memory < expected, $"Should allocate less than {expected} bytes, but was allocated {memory} bytes.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenOneMillionIterations_ShouldTheCollectionCountForeachGenBeBeZero_WhenGCCollect()
            {
                const int iterations = 1000000;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var gen0Begin = GC.CollectionCount(0);
                var gen1Begin = GC.CollectionCount(1);
                var gen2Begin = GC.CollectionCount(2);


                new Action(() =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.Bind(() => Property);

                    foreach (var value in Enumerable.Range(1, iterations))
                        liveData.Value = value;
                })();

                // Service should have gone out of scope about now, 
                // so the garbage collector can clean it up
                GC.Collect();
                GC.WaitForPendingFinalizers();
                var gen0End = GC.CollectionCount(0);
                var gen1End = GC.CollectionCount(1);
                var gen2End = GC.CollectionCount(2);
                var gen0 = gen0End - gen0Begin;
                var gen1 = gen1End - gen1Begin;
                var gen2 = gen2End - gen2Begin;
                Assert.Equal(1, gen0);
                Assert.Equal(1, gen1);
                Assert.Equal(1, gen2);
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void XXX()
            {
                var gen0Begin = GC.CollectionCount(0);
                var gen1Begin = GC.CollectionCount(1);
                var gen2Begin = GC.CollectionCount(2);

                // Service should have gone out of scope about now, 
                // so the garbage collector can clean it up
                GC.Collect();
                GC.WaitForPendingFinalizers();

                var gen0End = GC.CollectionCount(0);
                var gen1End = GC.CollectionCount(1);
                var gen2End = GC.CollectionCount(2);

                var gen0 = gen0End - gen0Begin;
                var gen1 = gen1End - gen1Begin;
                var gen2 = gen2End - gen2Begin;

                Assert.Equal(1, gen0);
                Assert.Equal(1, gen1);
                Assert.Equal(1, gen2);
            }
        }
    }
}
