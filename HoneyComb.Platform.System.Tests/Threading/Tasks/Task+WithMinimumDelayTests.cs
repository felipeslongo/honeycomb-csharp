using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using HoneyComb.Core.Threading.Tasks;
using HoneyComb.TestChamber;
using Xunit;

namespace HoneyComb.Core.Tests.Threading.Tasks
{
    public class Task_WithMinimumDelayTests
    {
        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnShortTask_WhenTaskIsFinished_ShouldAwaitMinimumDelay()
        {
            var minimumDelay = TimeSpan.FromMilliseconds(500);
            var taskDuration = TimeSpan.FromMilliseconds(250);
            var task = Task.Run(async () =>
            {
                await Task.Delay(taskDuration);
                return Unit.Default;
            });
            var timer = Stopwatch.StartNew();

            _ = await task.WithMinimumDelay(minimumDelay);
            
            Assert.InRange(
                timer.Elapsed, 
                minimumDelay, 
                minimumDelay + TimeSpan.FromMilliseconds(10));
        }
        
        [Fact]
        [Trait(nameof(Category), Category.Unit)]
        public async Task GivenAnLongerTask_WhenTaskIsFinished_ShouldNotAwaitMinimumDelay()
        {
            var minimumDelay = TimeSpan.FromMilliseconds(250);
            var taskDuration = TimeSpan.FromMilliseconds(300);
            var task = Task.Run(async () =>
            {
                await Task.Delay(taskDuration);
                return Unit.Default;
            });
            var timer = Stopwatch.StartNew();

            _ = await task.WithMinimumDelay(minimumDelay);
            
            Assert.InRange(
                timer.Elapsed, 
                taskDuration, 
                taskDuration + TimeSpan.FromMilliseconds(10));
        }
    }
}
