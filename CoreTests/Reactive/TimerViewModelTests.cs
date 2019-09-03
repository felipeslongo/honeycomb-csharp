using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Reactive;
using Xunit;

namespace CoreTests.Reactive
{
    public class TimerViewModelTests
    {
        public class TimerUiTests : TimerViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAInstanceWithoutCustomFormatter_ShouldBeTimerToStringValue_WhenTimerIsUpdated()
            {
                var viewModel = new TimerViewModel(TimeSpan.FromMilliseconds(100));
                viewModel.Start();
                
                await Task.Delay(viewModel.Interval);
                
                Assert.Equal(viewModel.Timer.Value.ToString(), viewModel.TimerUi.Value);
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAInstanceWithCustomFormatter_ShouldBeCustomFormated_WhenTimerIsUpdated()
            {
                var viewModel = new TimerViewModel(
                    TimeSpan.FromMilliseconds(100), 
                    timeSpan => timeSpan.TotalMilliseconds.ToString()
                    );
                viewModel.Start();
                
                await Task.Delay(viewModel.Interval);
                
                Assert.Equal("100", viewModel.TimerUi.Value);
            }
        }
    }
}
