namespace HoneyComb.UI.Dialogs
{
    public sealed class ConfirmationViewState
    {
        public ConfirmationViewState(
            string title,
            string message,
            string confirm,
            string cancel
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
