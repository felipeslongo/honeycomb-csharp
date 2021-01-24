namespace HoneyComb.UI.Dialogs
{
    /// <summary>
    ///     Intent of the user in response to the Confirmation Dialog.
    /// </summary>
    public sealed class ConfirmationIntent
    {
        public ConfirmationIntent(bool confirmed, ConfirmationViewState viewState)
        {
            Confirmed = confirmed;
            ViewState = viewState;
        }

        /// <summary>
        /// Gets confirmed intent.
        /// </summary>
        public bool Confirmed { get; }
        /// <summary>
        /// Gets the state presented to the user.
        /// </summary>
        public ConfirmationViewState ViewState { get; }
    }
}
