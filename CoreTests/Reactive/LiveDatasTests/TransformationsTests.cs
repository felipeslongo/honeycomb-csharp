using Core.Reactive.LiveDatas;
using Core.Threading;
using CoreTests.Assertions;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using Xunit;

namespace CoreTests.Reactive.LiveDatasTests
{
    public class TransformationsTests
    {
        protected const int DifferentValue = 2;
        protected const int SameValue = 1;

        public class MapTests : TransformationsTests
        {
            private readonly Func<int, string> MapFunction = input => input.ToString();

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnMapLiveData_ShouldHaveTheMappedValue_WhenCreated()
            {
                var source = new MutableLiveData<int>(SameValue);

                var returned = Transformations.Map(source, MapFunction);

                Assert.Equal(MapFunction(SameValue), returned.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnMapLiveData_ShouldEmitResultingValue_WhenSourceIsChanged()
            {
                var source = new MutableLiveData<int>(SameValue);
                var returned = Transformations.Map(source, MapFunction);

                source.Value = DifferentValue;

                Assert.Equal(MapFunction(DifferentValue), returned.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnMapLiveData_ShouldUnsubscribeFromSourceLiveData_WhenDisposed()
            {
                var source = new MutableLiveData<int>(SameValue);
                var returned = Transformations.Map(source, MapFunction);
                returned.Dispose();

                source.Value = DifferentValue;

                Assert.Equal(SameValue.ToString(), returned.Value);
            }
        }

        public class FromEventPatternTests : TransformationsTests
        {
            public event EventHandler<EventArgs> EventHandler;

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldCaptureTheEventArg_WhenEventHandlerIsInvoked()
            {
                var returned = Transformations.FromEventPattern<EventArgs>(handler => EventHandler += handler, handler => EventHandler -= handler);
                var expected = EventArgs.Empty;

                EventHandler(this, expected);

                Assert.Equal(expected, returned.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldCaptureTheLastInvocationEventArg_WhenEventHandlerIsInvokedMultipleTimes()
            {
                var returned = Transformations.FromEventPattern<EventArgs>(handler => EventHandler += handler, handler => EventHandler -= handler);
                var firstEventArgs = EventArgs.Empty;
                var lastEventArgs = new EventArgs();
                EventHandler(this, firstEventArgs);

                EventHandler(this, lastEventArgs);

                Assert.Equal(lastEventArgs, returned.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldSubscribeIntoTheEventHandler_WhenCreated()
            {
                var eventHandlerMock = new ActionMock();

                var returned = Transformations.FromEventPattern<EventArgs>(handler => eventHandlerMock.MockedAction(), _ => { });

                Assert.True(eventHandlerMock.IsInvoked);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldUnsubscribeFromTheEventHandler_WhenDisposed() =>
                SynchronizationContextSwitcher.RunWithoutContext(() =>
                {
                    var eventHandlerMock = new ActionMock();
                    var returned = Transformations.FromEventPattern<EventArgs>(handler => EventHandler += handler, _ => eventHandlerMock.MockedAction());

                    returned.Dispose();

                    Assert.True(eventHandlerMock.IsInvoked);
                });

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldUnsubscribeIntoTheCapturedSynchronizationContext_WhenDisposed() =>
                SynchronizationContextSwitcher.RunWithContext(new SynchronizationContextMock(), context =>
                {
                    var returned = Transformations.FromEventPattern<EventArgs>(handler => EventHandler += handler, handler => EventHandler -= handler);

                    returned.Dispose();

                    Assert.True(context.PostWasCalled);
                });
        }

        public class FromObservableTests : TransformationsTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromObservableLiveData_ShouldSetValueWithNewData_WhenObservableEmitsNewData()
            {
                var subject = new ReplaySubject<int>();
                var liveData = Transformations.FromObservable(subject as IObservable<int>);

                subject.OnNext(SameValue);

                Assert.Equal(SameValue, liveData.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromObservableLiveData_ShouldCaptureTheLastEmittedData_WhenObservableEmitsNewDataMultipleTimes()
            {
                var subject = new ReplaySubject<int>();
                var liveData = Transformations.FromObservable(subject as IObservable<int>);
                subject.OnNext(SameValue);

                subject.OnNext(DifferentValue);

                Assert.Equal(DifferentValue, liveData.Value);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromObservableLiveData_ShouldSubscribeIntoTheObservable_WhenCreated()
            {
                var subject = new ReplaySubject<int>();

                _ = Transformations.FromObservable(subject as IObservable<int>);

                Assert.True(subject.HasObservers);
            }


            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldUnsubscribeFromTheObservable_WhenDisposed()
            {
                var subject = new ReplaySubject<int>();
                var liveData = Transformations.FromObservable(subject as IObservable<int>);

                liveData.Dispose();

                Assert.False(subject.HasObservers);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAnFromEventPatternLiveData_ShouldUnsubscribeFromTheObservable_WhenOnCompletedInvoked()
            {
                var subject = new ReplaySubject<int>();
                _ = Transformations.FromObservable(subject as IObservable<int>);

                subject.OnCompleted();

                Assert.False(subject.HasObservers);
            }
        }
    }
}
