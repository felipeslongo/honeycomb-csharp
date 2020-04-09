namespace HoneyComb.UI.Dialogs
{
    public sealed class ConfirmationViewModel
    {
        public ConfirmationViewModel(
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

        public string Title { get; }
        public string Message { get; }
        public string Confirm { get; }
        public string Cancel { get; }
    }
}
