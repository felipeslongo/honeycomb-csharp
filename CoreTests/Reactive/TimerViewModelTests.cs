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
                
                Assert.Equal(viewModel.TimerUi.Value, viewModel.Timer.Value.ToString());
            }
            
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAInstanceWithCustomFormatter_ShouldBeCustomFormated_WhenTimerIsUpdated()
            {
                var viewModel = new TimerViewModel(TimeSpan.FromMilliseconds(100));
                viewModel.Start();
                
                await Task.Delay(viewModel.Interval);
                
                Assert.Equal(viewModel.TimerUi.Value, viewModel.Timer.Value.ToString());
            }
        }
    }
}
