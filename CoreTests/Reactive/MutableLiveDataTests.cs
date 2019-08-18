using System;
using Core.Reactive;
using Core.Strings;
using Xunit;

namespace CoreTests.Reactive
{
    public class MutableLiveDataTests
    {
        private readonly object _sameValue = new object();
        private readonly object _differentValue = new object();
        
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
        
        [Fact]
        public void GivenAInitialValue_ShouldNotRaisePropertyChanged_WhenValueSetIsEquals()
        {
            var liveData = new MutableLiveData<object>(_sameValue);
            var eventWasRaised = false;
            liveData.PropertyChanged += (sender, args) => eventWasRaised = true;

            liveData.Value = _sameValue;
            
            Assert.False(eventWasRaised);
        }
    }
}