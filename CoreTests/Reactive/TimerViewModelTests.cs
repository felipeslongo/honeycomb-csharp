using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Reactive;
using Xunit;

namespace CoreTests.Reactive
{
    public class TimerViewModelTests
    {
        private TimeSpan _interval = TimeSpan.FromMilliseconds(10);
        private TimeSpan _delay = TimeSpan.FromMilliseconds(100);
        private TimeSpan ElapsedTime { get; set; }

        [Fact]
        [Trait(nameof(Category),Category.Unit)]
        public async Task Given100MillisecondsDelay_ShouldTick9Times_WhenIntervalIs10Milliseconds()
        {
            var timer = new TimerViewModel(_interval);
            var elapsedTimeStates = new List<TimeSpan>();
            timer.ElapsedTime.PropertyChanged += (sender, args) => elapsedTimeStates.Add(timer.ElapsedTime);
            elapsedTimeStates.Clear();
            
            timer.Start();
            await Task.Delay(_delay);
            timer.Stop();
            
            Assert.Equal(_delay / _interval - 1, elapsedTimeStates.Count);
        }
        
        [Fact]
        [Trait(nameof(Category),Category.Unit)]
        public async Task Given100MillisecondsDelay_ShouldLastTickBe90Milliseconds_WhenIntervalIs10Milliseconds()
        {
            var timer = new TimerViewModel(_interval);
            timer.ElapsedTime.BindProperty(this, @this => @this.ElapsedTime);

            timer.Start();
            await Task.Delay(_delay);
            timer.Stop();
            
            Assert.Equal(TimeSpan.FromMilliseconds(_delay.TotalMilliseconds - _interval.TotalMilliseconds), ElapsedTime);
        }
    }
}
