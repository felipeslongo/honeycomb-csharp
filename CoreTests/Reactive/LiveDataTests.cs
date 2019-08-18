using Core.Reactive;
using System;
using Xunit;

namespace CoreTests.Reactive
{
    public class LiveDataTests
    {
        private readonly object _differentValue = new object();
        private readonly object _sameValue = new object();

        public class PropertyChangedTests : LiveDataTests
        {
            [Fact]
            public void GivenAInitialValue_ShouldNotRaisePropertyChanged_WhenValueSetIsEquals()
            {
                var liveData = new MutableLiveData<object>(_sameValue);
                var eventWasRaised = false;
                liveData.PropertyChanged += (sender, args) => eventWasRaised = true;

                liveData.Value = _sameValue;

                Assert.False(eventWasRaised);
            }

            [Fact]
            public void GivenAInitialValue_ShouldRaisePropertyChanged_WhenValueSetIsNotEquals()
            {
                var liveData = new MutableLiveData<object>(_sameValue);

                Assert.Raises<EventArgs>(
                    handler => liveData.PropertyChanged += handler,
                    handler => liveData.PropertyChanged -= handler,
                    () => liveData.Value = _differentValue
                );
            }
        }
    }
}