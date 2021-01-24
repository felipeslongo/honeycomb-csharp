namespace HoneyComb.UI.Dialogs
{
    /// <summary>
    ///     ViewState DTO for a Confirmation Alert Dialog workflow.
    /// </summary>
    public sealed class ConfirmationViewState
    {
        public ConfirmationViewState(
            string title = "Title",
            string message = "Message",
            string confirm = "Confirm",
            string cancel = "Cancel"
            ) : this(IntentId.Empty, title, message, confirm, cancel)
        { }

        public ConfirmationViewState(
            IntentId intentId,
            string title = "Title",
            string message = "Message",
            string confirm = "Confirm",
            string cancel = "Cancel"
            )
        {
            IntentId = intentId ?? IntentId.Empty;
            Title = title;
            Message = message;
            Confirm = confirm;
            Cancel = cancel;
        }

        public string Cancel { get; }
        public string Confirm { get; }
        public IntentId IntentId { get; }
        public string Message { get; }
        public string Title { get; }
    }
}
