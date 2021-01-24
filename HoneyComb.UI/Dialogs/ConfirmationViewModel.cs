using HoneyComb.Core.Threading.Tasks;
using HoneyComb.Core.Vault;
using HoneyComb.LiveDataNet;
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
    public sealed class ConfirmationViewModel : IDisposable, IRestorableUIState
    {
        /// <summary>
        ///     Current request being awaited for a response from the user.
        /// </summary>
        private readonly TaskCompletionSourceRecycler<bool> currentShowAsyncTask = new TaskCompletionSourceRecycler<bool>();

        private Guid instanceId = Guid.NewGuid();
        private MutableLiveEvent<ConfirmationIntent> onReceivedUserIntentOnRestoration = new MutableLiveEvent<ConfirmationIntent>();

        public ConfirmationViewModel()
        {
            SetStateToPlaceholder();
        }

        public string Cancel { get; private set; } = string.Empty;
        public string Confirm { get; private set; } = string.Empty;
        public IntentId IntentId { get; private set; } = IntentId.Empty;

        /// <summary>
        ///     Gets an boolean indicating if there is currently a
        ///     confirmation request to the user being awaited.
        /// </summary>
        public IsBusy IsBusy { get; } = new IsBusy();

        public string Message { get; private set; } = string.Empty;

        /// <summary>
        ///     Called at most once during the Restoration Process
        ///     when user intent is received if the dialog was awaiting
        ///     user intent during the Preservation Process.
        /// </summary>
        /// <remarks>
        ///     During process death, the original instance is killed during
        ///     Preservation and a new one is recreated during Restoration.
        ///     That means you lose the original awaitable.
        ///     Therefore you will have to fallback to this callback event.
        /// </remarks>
        public LiveEvent<ConfirmationIntent> OnReceivedUserIntentOnRestoration => onReceivedUserIntentOnRestoration;

        public string? RestorationIdentifier { get; set; } = typeof(ConfirmationViewModel).FullName;
        public string Title { get; private set; } = string.Empty;
        public Visibility Visibility { get; } = new Visibility(false);

        public void Dispose()
        {
            IsBusy.Dispose();
            Visibility.Dispose();
            onReceivedUserIntentOnRestoration.Dispose();
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

        public void OnPreservation(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            IntentId.OnPreservation(savedInstanceState);
            savedInstanceState.Add(nameof(instanceId), instanceId.ToString());
            savedInstanceState.Add(nameof(Cancel), Cancel);
            savedInstanceState.Add(nameof(Confirm), Confirm);
            IsBusy.OnPreservation(savedInstanceState);
            savedInstanceState.Add(nameof(Message), Message);
            savedInstanceState.Add(nameof(Title), Title);
            Visibility.OnPreservation(savedInstanceState);
        }

        public void OnRestoration(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            if (IsSavedInstanceStateFromSameInstance())
                return;

            IntentId.OnRestoration(savedInstanceState);
            Cancel = savedInstanceState.GetString(nameof(Cancel));
            Confirm = savedInstanceState.GetString(nameof(Confirm));
            IsBusy.OnRestoration(savedInstanceState);
            Message = savedInstanceState.GetString(nameof(Message));
            Title = savedInstanceState.GetString(nameof(Title));
            Visibility.OnRestoration(savedInstanceState);

            _ = HandleUserIntentOnRestoration();

            async Task HandleUserIntentOnRestoration()
            {
                if (IsBusy.Value is false)
                    return;

                var viewState = GetViewState();
                var intent = await currentShowAsyncTask.Task;
                IsBusy.Release();
                onReceivedUserIntentOnRestoration.Invoke(new ConfirmationIntent(intent, viewState));
            }

            bool IsSavedInstanceStateFromSameInstance() =>
                savedInstanceState.GetString(nameof(instanceId)) == instanceId.ToString();
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
            IntentId = viewState.IntentId;
        }
    }
}
