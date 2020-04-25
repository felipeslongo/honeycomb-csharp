using HoneyComb.Core.Threading.Tasks;
using HoneyComb.LiveDataNet;
using System;
using System.Threading.Tasks;

namespace HoneyComb.UI.Dialogs
{
    public sealed class ConfirmationViewModel : IDisposable
    {
        private readonly TaskCompletionSourceRecycler<bool> currentShowAsyncTask = new TaskCompletionSourceRecycler<bool>();

        public ConfirmationViewModel()
        {
            SetStateToPlaceholder();
        }

        public string Cancel { get; private set; } = string.Empty;
        public string Confirm { get; private set; } = string.Empty;
        public IsBusy IsBusy { get; } = new IsBusy();
        public string Message { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public Visibility Visibility { get; } = new Visibility(false);

        public void Dispose()
        {
            IsBusy.Dispose();
            Visibility.Dispose();
        }

        public ConfirmationViewState GetViewState() =>
            new ConfirmationViewState(Title, Message, Confirm, Cancel);

        public async Task NotifyCancellationAsync()
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(false);
        }

        public async Task NotifyConfirmationAsync()
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(true);
        }

        public async Task NotifyExceptionAsync(Exception exception)
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(exception);
        }

        public async Task<bool> ShowAsync(ConfirmationViewState viewState)
        {
            using (await IsBusy.WaitAsync())
            {
                SetViewState(viewState);
                Visibility.SetValue(true);
                return await currentShowAsyncTask.Task;
            }
        }

        private void Dismiss()
        {
            Visibility.SetValue(false);
            SetStateToPlaceholder();
        }

        private void SetStateToPlaceholder() => SetViewState(new ConfirmationViewState());

        private void SetViewState(ConfirmationViewState viewState)
        {
            Title = viewState.Title;
            Message = viewState.Message;
            Confirm = viewState.Confirm;
            Cancel = viewState.Cancel;
        }
    }
}
