using Core.Reactive;
using Core.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Xunit;

namespace CoreTests.Reactive
{
    public class LiveDataTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public class BindPropertyTests : LiveDataTests
        {
            private int _field;
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

            [Fact]
            [Trait(nameof(Category), Category.Performance)]
            public void GivenOneMillionIterations_ShouldBeAtMost45TimesSlower_WhenBindIsCalled()
            {
                const int iterations = 1000000;
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindProperty(this, target => target.Property);

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

                Assert.True(bindTimer.ElapsedMilliseconds < propertyTimer.ElapsedMilliseconds * 45,
                    $"{bindTimer.ElapsedMilliseconds} vs {propertyTimer.ElapsedMilliseconds}*45");
            }

            [Fact]
            [Trait(nameof(Category), Category.Performance)]
            public void GivenOneMillionIterations_ShouldBeExecutedInLessThan250Miliseconds_WhenBindIsCalledInEachIteration()
            {
                const int iterations = 1000000;
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindProperty(this, target => target.Property);

                var timer = new Stopwatch();
                timer.Start();
                foreach (var value in Enumerable.Range(1, iterations))
                    liveData.Value = value;
                timer.Stop();

                var threshold = TimeSpan.FromMilliseconds(250).TotalMilliseconds;
                var actual = timer.ElapsedMilliseconds;
                Assert.True(actual < threshold, $"Should be executed in less than {threshold} miliseconds, but was {actual} miliseconds.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnUnreferencedLiveData_ShouldBeGarbageCollected_WhenGCIsCalled()
            {
                WeakReference reference = null;
                new Action(() =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    // Do things with service that might cause a memory leak...
                    liveData.BindProperty(this, target => target.Property);
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
                liveData.BindProperty(this, target => target.Property);

                liveData.Value = DifferentValue;

                var gen = GC.GetGeneration(liveData);
                Assert.Equal(0, gen);
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void GivenAnReferencedLiveData_ShouldAllocateLessThan10000Bytes_WhenBindIsCalled()
            {
                var memoryBegin = GC.GetAllocatedBytesForCurrentThread();
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindProperty(this, target => target.Property);

                liveData.Value = DifferentValue;

                var memoryEnd = GC.GetAllocatedBytesForCurrentThread();
                var memory = memoryEnd - memoryBegin;
                var expected = 500;
                Assert.True(memory < expected, $"Should allocate less than {expected} bytes, but was allocated {memory} bytes.");
            }

            [Fact]
            [Trait(nameof(Category), Category.GarbageCollector)]
            public void XXX()
            {
                var gen0Begin = GC.CollectionCount(0);
                var gen1Begin = GC.CollectionCount(1);
                var gen2Begin = GC.CollectionCount(2);

                new Action(() =>
                {
                    var liveData = new MutableLiveData<int>(SameValue);
                    liveData.BindProperty(this, target => target.Property);
                    liveData.Value = DifferentValue;
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

            public void OnCompleted()
            {
                throw new NotImplementedException();
            }

            public void OnError(Exception error)
            {
                throw new NotImplementedException();
            }

            public void OnNext(EventArgs value) => _onNextInvokedArgs.Add(value);
        }

        public class WithSynchronizationContextTests : LiveDataTests
        {
            [Fact]
            public void GivenASynchronizationContext_ShouldBeExecutedInThatContext_WhenPropertyIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var context = new SynchronizationContextThreadPool();
                liveData.WithSynchronizationContext(context);
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var synchronizationContextTheadId = currentThreadId;
                liveData.PropertyChanged += (_,__) => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId;               

                liveData.Value = DifferentValue;

                Assert.NotEqual(currentThreadId, synchronizationContextTheadId);
            }

            [Fact]
            public void GivenAEmptySynchronizationContext_ShouldBeExecutedInTheCurrentThread_WhenPropertyIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var context = new SynchronizationContextThreadPool();
                liveData.WithSynchronizationContext(context);
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;
                var synchronizationContextTheadId = -1;
                liveData.PropertyChanged += (_, __) => synchronizationContextTheadId = Thread.CurrentThread.ManagedThreadId;

                liveData.Value = DifferentValue;

                Assert.Equal(currentThreadId, synchronizationContextTheadId);
            }
        }
    }
}
