namespace HoneyComb.UI.Dialogs
{
    public sealed class ConfirmationViewState
    {
        public ConfirmationViewState(
            string title = "Title",
            string message = "Message",
            string confirm = "Confirm",
            string cancel = "Cancel"
            )
        {
            Title = title;
            Message = message;
            Confirm = confirm;
            Cancel = cancel;
        }

        public string Cancel { get; }
        public string Confirm { get; }
        public string Message { get; }
        public string Title { get; }
    }
}
