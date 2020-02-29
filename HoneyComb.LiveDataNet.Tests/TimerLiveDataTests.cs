using HoneyComb.TestChamber;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.LiveDataNet.Tests
{
    public class TimerLiveDataTests
    {
        private readonly TimeSpan interval = TimeSpan.FromMilliseconds(100);
        private readonly TimeSpan awaitInterval = TimeSpan.FromSeconds(1);

        public TimerLiveDataTests()
        {
            Timer = new TimerLiveData(interval);
            Timer.Bind(() => TimerLiveDataValue);
            Timer.BindMethod(() => Ticks++);
        }

        private TimerLiveData Timer { get; }
        public TimeSpan TimerLiveDataValue;
        public int Ticks = -1;

        public class IntervalTests : TimerLiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnStartedInstance_ShouldCyclicKeepNotifying_WhenIntervalIsTickedEachTime()
            {
                var newInterval = interval / 2;
                Timer.Start();

                Timer.Interval = newInterval;
                await Task.Delay(awaitInterval);

                Assert.Equal(awaitInterval, TimerLiveDataValue);
                Assert.Equal(20, Ticks);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnStartedInstance_ShouldNotifyUsingTheNewInterval_WhenIntervalIsChanged()
            {
                Timer.Start();


                await Task.Delay(awaitInterval);

                Assert.Equal(awaitInterval, TimerLiveDataValue);
                Assert.Equal(10, Ticks);
            }
        }

        //public class StartTests : TimerLiveDataTests
        //{
        //    [Fact]
        //    [Trait(nameof(Category), Category.Unit)]
        //    public async Task GivenAnStartedInstance_ShouldStartNotifying_WhenStartIsCalled()
        //    {
        //        Timer.Start();
        //        await Task.Delay(interval);

        //        Assert.Equal(interval, TimerLiveDataValue);
        //        Assert.Equal(1, Ticks);
        //    }
        //}

        public class StopTests : TimerLiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnStartedInstance_ShouldStopNotifying_WhenStopIsCalled()
            {
                Timer.Start();

                Timer.Stop();
                await Task.Delay(interval);

                Assert.Equal(TimeSpan.Zero, TimerLiveDataValue);
                Assert.Equal(0, Ticks);
            }
        }

        // public class ResetTests : TimerLiveDataTests
        // {
        //     [Fact]
        //     [Trait(nameof(Category), Category.Unit)]
        //     public async Task GivenAnStartedInstance_ShouldSetValueToZero_WhenResetIsCalled()
        //     {
        //         var expectedTicks = 0;
        //         Timer.Start();
        //         await Task.Delay(interval * 2);
        //         expectedTicks += 2;
        //
        //         Timer.Reset();
        //         expectedTicks++;
        //         await Task.Delay(interval);
        //         expectedTicks++;
        //
        //         Assert.Equal(interval, TimerLiveDataValue);
        //         Assert.Equal(expectedTicks, Ticks);
        //     }
        // }

        public class DisposeTests : TimerLiveDataTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAnStartedInstance_ShouldStopNotifying_WhenDisposeIsCalled()
            {
                Timer.Start();

                Timer.Dispose();
                await Task.Delay(interval);

                Assert.Equal(TimeSpan.Zero, TimerLiveDataValue);
                Assert.Equal(0, Ticks);
            }
        }
    }
}
