using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Reactive;
using Xunit;

namespace CoreTests.Reactive
{
    public class TimerViewModelTests
    {
        private int _intervalVsDelayRatio = 10;
        private TimeSpan _interval = TimeSpan.FromMilliseconds(100);
        private TimeSpan DelayOfTenTimesTheInterval => _interval * _intervalVsDelayRatio;

        public class TicksTests : TimerViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnDelayOfTenTimesTheInterval_ShouldBeEqualsToTheIntervalVsDelayRatio_WhenTheDelayIsAwaited()
            {
                var timer = new TimerViewModel(_interval);

                timer.Start();
                await Task.Delay(DelayOfTenTimesTheInterval);
                timer.Stop();

                Assert.Equal(_intervalVsDelayRatio, timer.Ticks);
            }
        }      
        
        public class ElapsedTimeTests : TimerViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnDelayOfTenTimesTheInterval_ShouldBeEqualsToTheDelay_WhenTheDelayIsAwaited()
            {
                var timer = new TimerViewModel(_interval);

                timer.Start();
                await Task.Delay(DelayOfTenTimesTheInterval);
                timer.Stop();

                Assert.Equal(DelayOfTenTimesTheInterval, timer.ElapsedTime);
            }
        }
    }
}
