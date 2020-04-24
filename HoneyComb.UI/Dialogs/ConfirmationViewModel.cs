using HoneyComb.Core.Threading.Tasks;
using HoneyComb.LiveDataNet;
using System;
using System.Threading.Tasks;

namespace HoneyComb.UI.Dialogs
{
    public sealed class ConfirmationViewModel : IDisposable
    {
        private readonly TaskCompletionSourceRecycler<bool> currentShowAsyncTask = new TaskCompletionSourceRecycler<bool>();
        private readonly MutableLiveData<bool> visible = new MutableLiveData<bool>(false);

        public ConfirmationViewModel()
        {
            SetStateToPlaceholder();
        }

        public string Cancel { get; private set; } = string.Empty;
        public string Confirm { get; private set; } = string.Empty;
        public IsBusy IsBusy { get; } = new IsBusy();
        public string Message { get; private set; } = string.Empty;
        public string Title { get; private set; } = string.Empty;
        public LiveData<bool> Visible => visible;

        public void Dispose()
        {
            IsBusy.Dispose();
            visible.Dispose();
        }

        public ConfirmationViewState GetViewState() =>
            new ConfirmationViewState(Title, Message, Confirm, Cancel);

        public void NotifyCancellation()
        {
            currentShowAsyncTask.Recycle(false);
            Dismiss();
        }

        public void NotifyConfirmation()
        {
            currentShowAsyncTask.Recycle(true);
            Dismiss();
        }

        public void NotifyException(Exception exception)
        {
            currentShowAsyncTask.Recycle(exception);
            Dismiss();
        }

        public async Task<bool> ShowAsync(ConfirmationViewState viewState)
        {
            using (await IsBusy.WaitAsync())
            {
                SetViewState(viewState);
                visible.Value = true;
                return await currentShowAsyncTask.Task;
            }
        }

        private void Dismiss()
        {
            visible.Value = false;
            SetStateToPlaceholder();
        }

        private void SetStateToPlaceholder()
        {
            Title = "Title";
            Message = "Message";
            Confirm = "Confirm";
            Cancel = "Cancel";
        }

        private void SetViewState(ConfirmationViewState viewState)
        {
            Title = viewState.Title;
            Message = viewState.Message;
            Confirm = viewState.Confirm;
            Cancel = viewState.Cancel;
        }
    }
}
