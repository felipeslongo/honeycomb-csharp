using Core.Reactive;
using System;
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
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                Property = DifferentValue;

                liveData.BindProperty(this, target => target.Property);

                Assert.Equal(SameValue, Property);
            }

            [Fact]
            public void GivenAInitialValue_ShouldSetThisValueIntoTheProperty_WhenInitialValueIsChanged()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                liveData.BindProperty(this, target => target.Property);

                liveData.Value = DifferentValue;

                Assert.Equal(DifferentValue, Property);
            }

            [Fact]
            public void GivenAPropertyWithNoSetter_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target.PropertyWithNoSetter));

                Assert.Contains(LiveData<int>.MessagePropertyHasNoSetter, exception.Message);
            }

            [Fact]
            public void GivenANonPropertyMember_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target._field));

                Assert.Contains(LiveData<int>.MessageExpressionLambdaDoesNotReturnAProperty, exception.Message);
            }

            [Fact]
            public void GivenAMemberMethod_ShouldRaiseException_WhenBindIsCalled()
            {
                var liveData = new MutableLiveData<int>(SameValue);

                var exception = Assert.Throws<ArgumentException>("propertyLambda", () => liveData.BindProperty(this, target => target.Method()));

                Assert.Contains(LiveData<int>.MessageExpressionLambdaDoesNotReturnAProperty, exception.Message);
            }
        }

        public class PropertyChangedTests : LiveDataTests
        {
            [Fact]
            public void GivenAInitialValue_ShouldNotRaisePropertyChanged_WhenValueSetIsEquals()
            {
                var liveData = new MutableLiveData<int>(SameValue);
                var eventWasRaised = false;
                liveData.PropertyChanged += (sender, args) => eventWasRaised = true;

                liveData.Value = SameValue;

                Assert.False(eventWasRaised);
            }

            [Fact]
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
    }
}