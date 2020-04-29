using HoneyComb.Core.Threading.Tasks;
using System;
using System.Threading.Tasks;

namespace HoneyComb.UI.Dialogs
{
    /// <summary>
    ///     ViewModel for a Confirmation Alert Dialog workflow. 
    ///     Allow thread-safe asynchronous requests for user confirmation.
    /// </summary>
    /// <remarks>
    ///     Its Platform agnostic, where each native View implementation of 
    ///     each platform should observe its state to present the Dialog View 
    ///     to the user and should notify the ViewModel of the user response.
    /// </remarks>
    public sealed class ConfirmationViewModel : IDisposable
    {
        /// <summary>
        ///     Current request being awaited for a response from the user.
        /// </summary>
        private readonly TaskCompletionSourceRecycler<bool> currentShowAsyncTask = new TaskCompletionSourceRecycler<bool>();

        public ConfirmationViewModel()
        {
            SetStateToPlaceholder();
        }

        public string Cancel { get; private set; } = string.Empty;
        public string Confirm { get; private set; } = string.Empty;
        /// <summary>
        ///     Gets an boolean indicating if there is currently a
        ///     confirmation request to the user being awaited.
        /// </summary>
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

        /// <summary>
        ///     Notify that the user cancelled (negated) the request.
        ///     It does not throw a OperationCancelledException.
        /// </summary>
        /// <returns></returns>
        public async Task NotifyCancellationAsync()
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(false);
        }

        /// <summary>
        ///     Notify that the user confirmed the request.
        /// </summary>
        /// <returns></returns>
        public async Task NotifyConfirmationAsync()
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(true);
        }

        /// <summary>
        ///     Notify that a exception happened in the workflow.
        ///     It is passed to the current awaited task.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task NotifyExceptionAsync(Exception exception)
        {
            Dismiss();
            await currentShowAsyncTask.RecycleAsync(exception);
        }

        /// <summary>
        ///     Asynchronously request for user confirmation.
        /// </summary>
        /// <remarks>
        ///      Its thread-safe in the way that a subsequent call 
        ///      awaits the previously one be finished before being 
        ///      able to mutate this ViewModel state. 
        ///      It's like a lock or mutex.
        /// </remarks>
        /// <param name="viewState">Dialog parameters.</param>
        /// <returns>Awaitable boolean indicating confirmation or cancellation (negation).</returns>
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
