using HoneyComb.Platform.System.Threading.Tasks;
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
                var task = viewModel.ShowAsync(new ConfirmationViewState());

                var actual = viewModel.IsBusy;

                Assert.True(actual);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAShowAsyncTask_WhenTaskCompletes_ShouldBeFalse()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());
                viewModel.NotifyConfirmation();

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

                viewModel.NotifyCancellation();

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

                viewModel.NotifyConfirmation();

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

                viewModel.NotifyException(expectedException);

                Assert.True(task.IsFaulted);
                await Assert.ThrowsAsync<ApplicationException>(async () => await task);
            }
        }

        public class VisibleTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAShowAsyncTask_WhenInvoked_ShouldBeTrue()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());

                var actual = viewModel.Visible;

                Assert.True(actual);
            }

            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public void GivenAShowAsyncTask_WhenTaskCompletes_ShouldBeFalse()
            {
                var viewModel = new ConfirmationViewModel();
                var task = viewModel.ShowAsync(new ConfirmationViewState());
                viewModel.NotifyConfirmation();

                var actual = viewModel.Visible;

                Assert.False(actual);
            }
        }

        public class ShowAsyncTests : ConfirmationViewModelTests
        {
            [Fact]
            [Trait(nameof(Category), Category.Unit)]
            public async Task GivenMultipleOrderedCallsToShowAsync_WhenTaskResponsesAreInvoked_ShouldReturnTheAsyncResultInTheSameCallOrder()
            {
                var viewModel = new ConfirmationViewModel();
                var task1Falted = viewModel.ShowAsync(new ConfirmationViewState());
                var task2Canceled = viewModel.ShowAsync(new ConfirmationViewState());
                var task3Confirmed = viewModel.ShowAsync(new ConfirmationViewState());

                viewModel.NotifyException(new Exception());
                await task1Falted.WithExceptionIgnore();
                viewModel.NotifyCancellation();
                await task2Canceled;
                viewModel.NotifyConfirmation();
                await task3Confirmed;

                Assert.True(task1Falted.IsFaulted);
                Assert.True(task2Canceled.IsCompletedSuccessfully);
                Assert.False(await task2Canceled);
                Assert.True(task3Confirmed.IsCompletedSuccessfully);
                Assert.True(await task3Confirmed);
            }
        }
    }
}
