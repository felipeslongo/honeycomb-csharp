using HoneyComb.TestChamber;
using HoneyComb.UI.Dialogs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace HoneyComb.UI.Tests
{
    public class ConfirmationViewModelTests
    {
        public class IsBusyTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAShowAsyncTask_WhenInvoked_ShouldBeTrue()
            {
                var viewModel = new ConfirmationViewModel();
                _ = viewModel.ShowAsync(new ConfirmationViewState());

                var actual = viewModel.IsBusy;

                Assert.True(actual);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAShowAsyncTask_WhenTaskCompletes_ShouldBeFalse()
            {
                var viewModel = new ConfirmationViewModel();
                _ = viewModel.ShowAsync(new ConfirmationViewState());
                await viewModel.NotifyConfirmationAsync();

                var actual = viewModel.IsBusy;

                Assert.False(actual);
            }
        }

        public class NotifyCancellationTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAShowAsyncTask_WhenInvoked_ShouldTaskReturnFalse()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());

                await viewModel.NotifyCancellationAsync();

                Assert.True(task.IsCompletedSuccessfully);
                Assert.False(await task);
            }
        }

        public class NotifyConfirmationTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAShowAsyncTask_WhenInvoked_ShouldTaskReturnTrue()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());

                await viewModel.NotifyConfirmationAsync();

                Assert.True(task.IsCompletedSuccessfully);
                Assert.True(await task);
            }
        }

        public class NotifyExceptionTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAShowAsyncTask_WhenInvoked_ShouldTaskThrowException()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());
                var expectedException = new ApplicationException();

                await viewModel.NotifyExceptionAsync(expectedException);

                Assert.True(task.IsFaulted);
                await Assert.ThrowsAsync<ApplicationException>(async () => await task);
            }
        }

        public class ShowAsyncTests : ConfirmationViewModelTests
        {
            [Fact(Timeout = 100)]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenMultipleOrderedCallsToShowAsync_WhenTaskResponsesAreInvoked_ShouldReturnTheAsyncResultInTheSameCallOrder()
            {
                var viewModel = new ConfirmationViewModel();
                var task1Falted = viewModel.ShowAsync(new ConfirmationViewState());
                var task2Canceled = viewModel.ShowAsync(new ConfirmationViewState());
                var task3Confirmed = viewModel.ShowAsync(new ConfirmationViewState());

                await viewModel.Visibility.WaitVisibilityAsync();
                await viewModel.NotifyExceptionAsync(new Exception());
                await viewModel.Visibility.WaitVisibilityAsync();
                await viewModel.NotifyCancellationAsync();
                await viewModel.Visibility.WaitVisibilityAsync();
                await viewModel.NotifyConfirmationAsync();

                Assert.True(task1Falted.IsFaulted);
                Assert.False(await task2Canceled);
                Assert.True(await task3Confirmed);
            }
        }

        public class VisibilityTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAShowAsyncTask_WhenInvoked_ShouldBeTrue()
            {
                var viewModel = new ConfirmationViewModel();
                _ = viewModel.ShowAsync(new ConfirmationViewState());

                var actual = viewModel.Visibility;

                Assert.True(actual);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenAShowAsyncTask_WhenTaskCompletes_ShouldBeFalse()
            {
                var viewModel = new ConfirmationViewModel();
                _ = viewModel.ShowAsync(new ConfirmationViewState());
                await viewModel.NotifyConfirmationAsync();

                var actual = viewModel.Visibility;

                Assert.False(actual);
            }
        }
    }
}
